using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Visual_Novel_Database
{
    //This class handles the connection to the API
    //meaning: Connection, Login, Send request and receive response
    //
    //forked from https://github.com/FredTheBarber/VndbClient back in mid 2015
    class APIConnection
    {
        private string _loginString; //Login string
        //private string LoginStringUser; //Login String with Username and Pass

        private TcpClient _tcpClient;
        private Stream _stream;

        private string _jsonPayLoad; //Contains API response (in json format)

        private readonly byte _endOfCommand = 0x04; //Has to be attached to end of command (EOF)

        #region properties

        public string Jsonresponse
        {
            get
            {
                return _jsonPayLoad;
            }
            set
            {
                _jsonPayLoad = value;
            }
        }
        #endregion properties

        #region enums

        //Handles responsetype
        public enum ResponseType
        {
            Ok,
            Error,
            Unknown
        }

        #endregion enums

        #region methods

        private static readonly Tuple<string, ResponseType>[] ResponseTypeMap = {
            new Tuple<string, ResponseType>("Ok", ResponseType.Ok),
            new Tuple<string, ResponseType>("Error", ResponseType.Error)
        };

        //Opens connection
        public bool Open(bool useTLS = false)
        {
            _tcpClient = new TcpClient();
            try
            {
                _tcpClient.Connect("api.vndb.org", useTLS ? 19535 : 19534);
            }
            catch (SocketException)
            {
                Debug.Print("Error connecting to VNDB (Is your Internet on?)");
                return false;
            }
            _stream = _tcpClient.GetStream();
            return true;
        }

        //Login
        public int Login(string clientName, string clientVersion, string username = null, char[] password = null)
        {
            if (username != null && password != null) _loginString =
                                $"login {{\"protocol\":1,\"client\":\"{clientName}\",\"clientver\":{clientVersion}, \"username\":\"{username}\",\"password\":\"{password}\"}}";
            else _loginString = $"login {{\"protocol\":1,\"client\":\"{clientName}\",\"clientver\":{clientVersion}}}";

            var response = IssueCommand(_loginString);

            if ((response == ResponseType.Error) || (response == ResponseType.Unknown))
                return 1;
            return 0;
        }

        //Issue command
        public int Query(string query)
        {
            var response = IssueCommand(query);

            return response == ResponseType.Error ? 1 : 0;
        }

        //Issue request and handle response
        public ResponseType IssueCommand(string command)
        {
            byte[] encoded = Encoding.UTF8.GetBytes(command); //encode request to UTF8 format
            byte[] requestBuffer = new byte[encoded.Length + 1];

            Buffer.BlockCopy(encoded, 0, requestBuffer, 0, encoded.Length);

            requestBuffer[encoded.Length] = _endOfCommand; //Attach EOF to request

            _stream.Write(requestBuffer, 0, requestBuffer.Length);  //Send request

            byte[] responseBuffer = new byte[4096];

            int totalRead = 0;

            while (true) //Read all bytes
            {
                int currentRead = _stream.Read(responseBuffer, totalRead, responseBuffer.Length - totalRead); //read from stream

                if (currentRead == 0)
                    throw new Exception("Connection closed while reading login response");

                totalRead += currentRead;

                if (IsCompleteMessage(responseBuffer, totalRead)) //Check if message is complete
                    break;

                if (totalRead == responseBuffer.Length) //Embiggen buffer if necessary
                {
                    byte[] biggerBuffer = new byte[responseBuffer.Length * 2];
                    Buffer.BlockCopy(responseBuffer, 0, biggerBuffer, 0, responseBuffer.Length);
                    responseBuffer = biggerBuffer;
                }
            }

            string response = Encoding.UTF8.GetString(responseBuffer, 0, totalRead - 1); //Get in string format

            int firstspace = response.IndexOf(' '); //Look for whitespace

            if (firstspace == -1) //Only got 'OK' back -> no jsonpayload (just logged in)
            {
                Tuple<string, ResponseType> responseTypeEntry = ResponseTypeMap.FirstOrDefault(l => string.Compare(l.Item1, response, StringComparison.OrdinalIgnoreCase) == 0);

                if (responseTypeEntry != null)
                    return responseTypeEntry.Item2;

            }
            else //Got response + jsonpayload
            {
                string responseTypeString = response.Substring(0, firstspace);

                Tuple<string, ResponseType> responseTypeEntry = ResponseTypeMap.FirstOrDefault(l => string.Compare(l.Item1, responseTypeString, StringComparison.OrdinalIgnoreCase) == 0);

                if (responseTypeEntry != null)
                {
                    _jsonPayLoad = response.Substring(firstspace + 1);
                    return responseTypeEntry.Item2;
                }
                _jsonPayLoad = response.Substring(firstspace + 1);
                return ResponseType.Unknown;
            }
            return (ResponseType.Error);
        }

        //Check if message is complete
        public bool IsCompleteMessage(byte[] message, int bytesUsed)
        {

            if (bytesUsed == 0)
                throw new Exception("You have a bug, in IsCompleteMessage");

            if (message[bytesUsed - 1] == _endOfCommand) //Look for EOF byte
                return true;
            return false;
        }

        //Close connection
        public void CloseConnection()
        {
            _tcpClient.Close();
        }

        #endregion methods

    }


}

