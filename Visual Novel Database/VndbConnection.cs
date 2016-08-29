using System;
using System.Diagnostics;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Happy_Search
{
    /// <summary>
    /// Class for establishing connection with VNDB API and interacting with it.
    /// </summary>
    public class VndbConnection
    {
        private const string VndbHost = "api.vndb.org";
        private const ushort VndbPort = 19535; // always use TLS, because why not.
        private const byte EndOfStreamByte = 0x04;
        private Stream _stream;
        private TcpClient _tcpClient;
        internal Response LastResponse;
        internal LogInStatus LogIn = LogInStatus.No;
        internal APIStatus Status = APIStatus.Closed;

        /// <summary>
        /// Open stream with VNDB API.
        /// </summary>
        public void Open()
        {
            var complete = false;
            while (!complete)
            {
                try
                {
                    _tcpClient = new TcpClient();
                    _tcpClient.Connect(VndbHost, VndbPort);
                    var sslStream = new SslStream(_tcpClient.GetStream());
                    sslStream.AuthenticateAsClient(VndbHost);
                    _stream = sslStream;
                    complete = true;
                }
                catch (IOException e)
                {
                    Debug.Print(e.StackTrace);
                    Debug.Print("Conn Open Error");
                }
            }
        }

        /// <summary>
        /// Log into VNDB API, optionally using username/password.
        /// </summary>
        /// <param name="clientName">Name of Client accessing VNDB API</param>
        /// <param name="clientVersion">Version of Client accessing VNDB API</param>
        /// <param name="username">Username of user to log in as</param>
        /// <param name="password">Password of user to log in as</param>
        public void Login(string clientName, string clientVersion, string username = null, char[] password = null)
        {
            string loginBuffer;

            if (username != null && password != null)
            {
                loginBuffer =
                    $"login {{\"protocol\":1,\"client\":\"{clientName}\",\"clientver\":{clientVersion},\"username\":\"{username}\",\"password\":\"{new string(password)}\"}}";
                Query(loginBuffer);
                if (LastResponse.Type == ResponseType.Ok) LogIn = LogInStatus.YesWithCredentials;
            }
            else
            {
                loginBuffer = $"login {{\"protocol\":1,\"client\":\"{clientName}\",\"clientver\":{clientVersion}}}";
                Query(loginBuffer);
                if (LastResponse.Type == ResponseType.Ok) LogIn = LogInStatus.Yes;
            }
        }

        internal void Query(string command)
        {
            Status = APIStatus.Busy;
            byte[] encoded = Encoding.UTF8.GetBytes(command);
            var requestBuffer = new byte[encoded.Length + 1];
            Buffer.BlockCopy(encoded, 0, requestBuffer, 0, encoded.Length);
            requestBuffer[encoded.Length] = EndOfStreamByte;
            _stream.Write(requestBuffer, 0, requestBuffer.Length);
            var responseBuffer = new byte[4096];
            var totalRead = 0;
            while (true)
            {
                var currentRead = _stream.Read(responseBuffer, totalRead, responseBuffer.Length - totalRead);
                if (currentRead == 0) throw new Exception("Connection closed while reading login response");
                totalRead += currentRead;
                if (IsCompleteMessage(responseBuffer, totalRead)) break;
                if (totalRead != responseBuffer.Length) continue;
                var biggerBadderBuffer = new byte[responseBuffer.Length*2];
                Buffer.BlockCopy(responseBuffer, 0, biggerBadderBuffer, 0, responseBuffer.Length);
                responseBuffer = biggerBadderBuffer;
            }
            LastResponse = Parse(responseBuffer, totalRead);
            Status = LastResponse.Type != ResponseType.Unknown ? APIStatus.Ready : APIStatus.Error;
        }

        internal async Task QueryAsync(string query)
        {
            byte[] encoded = Encoding.UTF8.GetBytes(query);
            var requestBuffer = new byte[encoded.Length + 1];
            Buffer.BlockCopy(encoded, 0, requestBuffer, 0, encoded.Length);
            requestBuffer[encoded.Length] = EndOfStreamByte;
            await _stream.WriteAsync(requestBuffer, 0, requestBuffer.Length);
            var responseBuffer = new byte[4096];
            var totalRead = 0;
            while (true)
            {
                var currentRead = await _stream.ReadAsync(responseBuffer, totalRead, responseBuffer.Length - totalRead);
                if (currentRead == 0) throw new Exception("Connection closed while reading login response");
                totalRead += currentRead;
                if (IsCompleteMessage(responseBuffer, totalRead)) break;
                if (totalRead != responseBuffer.Length) continue;
                var biggerBadderBuffer = new byte[responseBuffer.Length*2];
                Buffer.BlockCopy(responseBuffer, 0, biggerBadderBuffer, 0, responseBuffer.Length);
                responseBuffer = biggerBadderBuffer;
            }
            LastResponse = Parse(responseBuffer, totalRead);
            switch (LastResponse.Type)
            {
                case ResponseType.Ok:
                case ResponseType.Results:
                case ResponseType.DBStats:
                    Status = APIStatus.Ready;
                    break;
                case ResponseType.Error:
                    Status = LastResponse.Error.ID.Equals("throttled") ? APIStatus.Throttled : APIStatus.Ready;
                    break;
                case ResponseType.Unknown:
                    Status = APIStatus.Error;
                    break;
            }
        }

        /// <summary>
        /// Close connection with VNDB API
        /// </summary>
        public void Close()
        {
            _tcpClient.GetStream().Close();
            _tcpClient.Close();
            Status = APIStatus.Closed;
        }


        private static bool IsCompleteMessage(byte[] message, int bytesUsed)
        {
            if (bytesUsed == 0)
            {
                throw new Exception("You have a bug, dummy. You should have at least one byte here.");
            }

            // ASSUMPTION: simple request-response protocol, so we should see at most one message in a given byte array.
            // So, there's no need to walk the whole array looking for validity - just be lazy and check the last byte for EOS.
            return message[bytesUsed - 1] == EndOfStreamByte;
        }

        private static Response Parse(byte[] message, int bytesUsed)
        {
            if (!IsCompleteMessage(message, bytesUsed))
            {
                throw new Exception("You have a bug, dummy.");
            }

            var stringifiedResponse = Encoding.UTF8.GetString(message, 0, bytesUsed - 1);
            var firstSpace = stringifiedResponse.IndexOf(' ');
            var firstWord = firstSpace != -1 ? stringifiedResponse.Substring(0, firstSpace) : stringifiedResponse;
            var payload = firstSpace > 0 ? stringifiedResponse.Substring(firstSpace) : "";
            if (firstSpace == bytesUsed - 1)
            {
                // protocol violation!
                throw new Exception("Protocol violation: last character in response is first space");
            }
            switch (firstWord)
            {
                case "ok":
                    return new Response(ResponseType.Ok, payload);
                case "results":
                    return new Response(ResponseType.Results, payload);
                case "dbstats":
                    return new Response(ResponseType.DBStats, payload);
                case "error":
                    return new Response(ResponseType.Error, payload);
                default:
                    return new Response(ResponseType.Unknown, payload);
            }
        }

        internal enum LogInStatus
        {
            No,
            Yes,
            YesWithCredentials
        }

        internal enum APIStatus
        {
            Ready,
            Busy,
            Throttled,
            Error,
            Closed
        }
    }

    /// <summary>
    /// Holds API's response to commands.
    /// </summary>
    public class Response
    {
        /// <summary>
        /// If response is of type 'error', holds ErrorResponse
        /// </summary>
        public ErrorResponse Error;
        /// <summary>
        /// Response in JSON format
        /// </summary>
        public string JsonPayload;
        /// <summary>
        /// Type of response
        /// </summary>
        public ResponseType Type;

        /// <summary>
        /// Constructor for Response
        /// </summary>
        /// <param name="type">Type of response</param>
        /// <param name="jsonPayload">Response in JSON format</param>
        public Response(ResponseType type, string jsonPayload)
        {
            Type = type;
            JsonPayload = jsonPayload;
            if (type == ResponseType.Error) Error = JsonConvert.DeserializeObject<ErrorResponse>(jsonPayload);
        }
    }


    /// <summary>
    /// Type of API Response
    /// </summary>
    public enum ResponseType
    {
        /// <summary>
        /// Returned by login command
        /// </summary>
        Ok,
        /// <summary>
        /// Returned by get commands 
        /// </summary>
        Results,
        /// <summary>
        /// Returned by dbstats command
        /// </summary>
        DBStats,
        /// <summary>
        /// Returned when there is an error
        /// </summary>
        Error,
        /// <summary>
        /// Returned in all other cases
        /// </summary>
        Unknown
    }
}