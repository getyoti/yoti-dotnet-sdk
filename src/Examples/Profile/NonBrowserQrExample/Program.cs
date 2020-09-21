using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Yoti.Auth.ProtoBuf.Qr;

namespace NonBrowserQrExample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string refId = GetRefId();
            dynamic qrCodeJson = GetQrCodeJson(refId);

            string sessionData = qrCodeJson["session_data"];
            if (string.IsNullOrEmpty(sessionData))
            {
                throw new InvalidOperationException("unable to find `session_data` in JSON response");
            }

            string callbackEndpoint = qrCodeJson["callback_endpoint"];
            if (string.IsNullOrEmpty(callbackEndpoint))
            {
                throw new InvalidOperationException("unable to find `callback_endpoint` in JSON response");
            }

            var clientWebSocket = new ClientWebSocket();

            Connect(sessionData);

            UTF8Encoding encoding = new UTF8Encoding();

            while (true)
            {
                Task.WaitAll(ParseTokenOnMessage(clientWebSocket));
            }
        }

        public static async Task Connect(string sessionData)
        {
            ClientWebSocket webSocket = null;
            try
            {
                var url = new Uri($"wss://api.yoti.com/api/v1.1/connect-sessions/{sessionData}"); ;

                webSocket = new ClientWebSocket();
                await webSocket.ConnectAsync(url, CancellationToken.None);
                await Task.WhenAll(ParseTokenOnMessage(webSocket));
            }
            catch (Exception ex)
            {
                // Log it
            }
            finally
            {
                if (webSocket != null)
                {
                    webSocket.Dispose();
                }
            }
        }

        public static async Task<string> ParseTokenOnMessage(ClientWebSocket webSocket)
        {
            byte[] buffer = new byte[1024];
            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty,
                        CancellationToken.None);
                }
                else
                {
                    TokenResponse tokenResponse =
                        Newtonsoft.Json.JsonConvert.DeserializeObject<TokenResponse>(Encoding.UTF8.GetString(buffer));

                    switch (tokenResponse.Status)
                    {
                        case "":
                            throw new InvalidOperationException("unable to find `status` in JSON response");

                        case "COMPLETED":
                            Debug.WriteLine("subscription result:");
                            Debug.WriteLine(tokenResponse.ToString());

                            return tokenResponse.Token;

                        case "ACCEPTED":
                            continue;

                        case "TIMED_OUT":
                            break;

                        case "REJECTED":
                            break;
                    }
                }
            }

            throw new InvalidOperationException("unable to retrieve token");
        }

        //enter to an infinite cycle to be able to handle every change in stream
        //while (true)
        //{
        //    dynamic response = ParseResponse(clientWebSocket).Result;

        //    switch (response["status"])
        //    {
        //        case "":
        //            throw new InvalidOperationException("unable to find `status` in JSON response");

        //        case "COMPLETED":
        //            Debug.WriteLine("subscription result:");
        //            Debug.WriteLine(response.ToString());

        //            oneTimeUseToken = response["token"];
        //            break;

        //        case "ACCEPTED":
        //            break;

        //        case "TIMED_OUT":
        //            break;

        //        case "REJECTED":
        //            break;
        //    }

        //    Task.Delay(2000);
        //}

        //if (string.IsNullOrEmpty(oneTimeUseToken))
        //{
        //    throw new InvalidOperationException("Did not get token successfully");
        //}

        //HttpWebRequest tokenRequest =
        //    (HttpWebRequest)WebRequest.Create(callbackEndpoint + oneTimeUseToken);

        //using (var response = (HttpWebResponse)tokenRequest.GetResponse())
        //{
        //    using (var responseStream = response.GetResponseStream())
        //    using (var reader = new StreamReader(responseStream, Encoding.ASCII))
        //    {
        //        Debug.WriteLine(reader.ReadToEnd());
        //    }
        //}

        //clientWebSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        //}

        private static async Task<dynamic> ParseResponse(WebSocket connection)
        {
            var buffer = WebSocket.CreateClientBuffer(4096, 4096);

            WebSocketReceiveResult result = await connection.ReceiveAsync(buffer, CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                using (var ms = new MemoryStream())
                {
                    do
                    {
                        result = await connection.ReceiveAsync(buffer, CancellationToken.None);
                        ms.Write(buffer.Array, buffer.Offset, result.Count);
                    }
                    while (!result.EndOfMessage);

                    ms.Seek(0, SeekOrigin.Begin);

                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        using (var reader = new StreamReader(result.ToString(), Encoding.ASCII))
                        {
                            return JObject.Parse(await reader.ReadToEndAsync());
                        }
                    }
                }
            }

            throw new InvalidOperationException("unable to parse response");
        }

        private async Task<(WebSocketReceiveResult, IEnumerable<byte>)> ReceiveFullMessage(
            WebSocket socket, CancellationToken cancelToken)
        {
            WebSocketReceiveResult response;
            var message = new List<byte>();

            var buffer = new byte[4096];
            do
            {
                response = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), cancelToken);
                message.AddRange(new ArraySegment<byte>(buffer, 0, response.Count));
            } while (!response.EndOfMessage);

            return (response, message);
        }

        private static string GetRefId()
        {
            GetEnvironmentVariables(out string clientSdkId, out string scenarioId);

            Uri qrCodeUri =
                new Uri($"https://api.yoti.com/api/v1.1/sessions/apps/{clientSdkId}/scenarios/{scenarioId}");

            HttpWebRequest qrCodeRequest = (HttpWebRequest)WebRequest.Create(qrCodeUri);

            using (var response = (HttpWebResponse)qrCodeRequest.GetResponse())
            {
                string responseString;

                using (var responseStream = response.GetResponseStream())
                using (var reader = new StreamReader(responseStream, Encoding.ASCII))
                {
                    responseString = reader.ReadToEnd();
                }

                string baseURL = "https://code.yoti.com/";
                int ix = responseString.IndexOf(baseURL);

                if (ix == -1)
                {
                    throw new InvalidOperationException($"response should begin with {baseURL}");
                }

                string base64QRCodeData = responseString.Substring(ix + baseURL.Length);

                byte[] decodedQRCodeData = Convert.FromBase64String(base64QRCodeData);

                QrCodeMsg qrCodeMsg = QrCodeMsg.Parser.ParseFrom(decodedQRCodeData);

                return qrCodeMsg.RefId.ToStringUtf8();
            }
        }

        private static dynamic GetQrCodeJson(string refId)
        {
            Uri sessionUri = new Uri($"https://api.yoti.com/api/v1.1/qrcodes/refs/{refId}");

            HttpWebRequest qrCodeRequest = (HttpWebRequest)WebRequest.Create(sessionUri);

            using (var response = (HttpWebResponse)qrCodeRequest.GetResponse())
            {
                string responseString;

                using (var responseStream = response.GetResponseStream())
                using (var reader = new StreamReader(responseStream, Encoding.ASCII))
                {
                    responseString = reader.ReadToEnd();
                }

                return JObject.Parse(responseString);
            }
        }

        private static void GetEnvironmentVariables(out string clientSdkId, out string scenarioId)
        {
            if (File.Exists(".env"))
            {
                Debug.WriteLine("using environment variables from .env file");
                DotNetEnv.Env.Load();
            }

            clientSdkId = Environment.GetEnvironmentVariable("YOTI_CLIENT_SDK_ID");
            scenarioId = Environment.GetEnvironmentVariable("YOTI_SCENARIO_ID");

            if (string.IsNullOrEmpty(clientSdkId) || string.IsNullOrEmpty(scenarioId))
            {
                throw new InvalidOperationException("environment variables not found. " +
                                                    "Either pass these in the .env file, or as a standard environment variable.");
            }
        }
    }
}