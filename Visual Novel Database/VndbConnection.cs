using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
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
        private const string CertificateFile = "Program Data\\Certificates\\isrgrootx1.pem";
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
            FormMain.LogToFile($"Attempting to open connection to {VndbHost}:{VndbPort}");
            var complete = false;
            var retries = 0;
            while (!complete && retries < 5)
            {
                try
                {
                    retries++;
                    FormMain.LogToFile($"Trying for the {retries}'th time...");
                    _tcpClient = new TcpClient();
                    _tcpClient.Connect(VndbHost, VndbPort);
                    FormMain.LogToFile("TCP Client connection made...");
                    var sslStream = new SslStream(_tcpClient.GetStream());
                    FormMain.LogToFile("SSL Stream received...");
                    if (File.Exists(CertificateFile))
                    {
                        var certs = new X509CertificateCollection();
                        var certFiles = Directory.GetFiles("Program Data\\Certificates");
                        foreach (var certFile in certFiles)
                        {
                            certs.Add(X509Certificate.CreateFromCertFile(certFile));
                        }
                        foreach (var cert in certs)
                        {
                            FormMain.LogToFile("Local Certificate data - subject/issuer/format/effectivedate/expirationdate");
                            FormMain.LogToFile(cert.Subject);
                            FormMain.LogToFile(cert.Issuer);
                            FormMain.LogToFile(cert.GetFormat());
                            FormMain.LogToFile(cert.GetEffectiveDateString());
                            FormMain.LogToFile(cert.GetExpirationDateString());
                        }
                        sslStream.AuthenticateAsClient(VndbHost, certs, SslProtocols.Tls12, true);
                    }
                    else
                    {
                        FormMain.LogToFile("Certificate not found, trying without it...");
                        sslStream.AuthenticateAsClient(VndbHost);
                    }
                    FormMain.LogToFile("SSL Stream authenticated...");
                    if (sslStream.RemoteCertificate != null)
                    {
                        FormMain.LogToFile("Remote Certificate data - subject/issuer/format/effectivedate/expirationdate");
                        var subject = sslStream.RemoteCertificate.Subject;
                        FormMain.LogToFile(subject);
                        FormMain.LogToFile(sslStream.RemoteCertificate.Issuer);
                        FormMain.LogToFile(sslStream.RemoteCertificate.GetFormat());
                        FormMain.LogToFile(sslStream.RemoteCertificate.GetEffectiveDateString());
                        FormMain.LogToFile(sslStream.RemoteCertificate.GetExpirationDateString());
                        if (!subject.Substring(3).Equals(VndbHost))
                        {
                            FormMain.LogToFile($"Certificate received isn't for {VndbHost} so connection is closed");
                            Status = APIStatus.Error;
                            return;
                        }
                    }
                    _stream = sslStream;
                    complete = true;
                    FormMain.LogToFile($"Connected after {retries} tries.");
                }
                catch (IOException e)
                {
                    FormMain.LogToFile("Conn Open Error");
                    FormMain.LogToFile(e.Message);
                    FormMain.LogToFile(e.StackTrace);
                }
                catch (AuthenticationException e)
                {
                    FormMain.LogToFile("Conn Authentication Error");
                    FormMain.LogToFile(e.Message);
                    FormMain.LogToFile(e.StackTrace);
                    if (e.InnerException == null) continue;
                    FormMain.LogToFile(e.InnerException.Message);
                    FormMain.LogToFile(e.InnerException.StackTrace);
                }
                catch (Exception ex) when (ex is ArgumentNullException || ex is InvalidOperationException)
                {
                    FormMain.LogToFile("Conn Other Error");
                    FormMain.LogToFile(ex.Message);
                    FormMain.LogToFile(ex.StackTrace);
                }
            }
            if (retries != 5) return;
            FormMain.LogToFile($"Failed to connect after {retries} tries.");
            Status = APIStatus.Error;
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
                    $"login {{\"protocol\":1,\"client\":\"{clientName}\",\"clientver\":\"{clientVersion}\",\"username\":\"{username}\",\"password\":\"{new string(password)}\"}}";
                Query(loginBuffer);
                if (LastResponse.Type == ResponseType.Ok) LogIn = LogInStatus.YesWithCredentials;
            }
            else
            {
                loginBuffer = $"login {{\"protocol\":1,\"client\":\"{clientName}\",\"clientver\":\"{clientVersion}\"}}";
                Query(loginBuffer);
                if (LastResponse.Type == ResponseType.Ok) LogIn = LogInStatus.Yes;
            }
        }

        internal void Query(string command)
        {
            if (Status == APIStatus.Error) return;
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
                var biggerBadderBuffer = new byte[responseBuffer.Length * 2];
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
                var biggerBadderBuffer = new byte[responseBuffer.Length * 2];
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
            try
            {
                _tcpClient.GetStream().Close();
                _tcpClient.Close();
            }
            catch (ObjectDisposedException e)
            {
                FormMain.LogToFile("Failed to close connection.");
                FormMain.LogToFile(e.Message);
                FormMain.LogToFile(e.StackTrace);
            }
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