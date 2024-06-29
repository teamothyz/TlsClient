using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Text;

namespace TlsLib
{
    public partial class TlsClient : IDisposable
    {
        [LibraryImport("./dll/tls-client-windows-64-1.7.5.dll", EntryPoint = "request")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        private static partial IntPtr request([In] byte[] requestPayload, [MarshalAs(UnmanagedType.LPWStr)] string sessionID);

        [LibraryImport("./dll/tls-client-windows-64-1.7.5.dll", EntryPoint = "freeMemory")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        private static partial void freeMemory([MarshalAs(UnmanagedType.LPWStr)] string sessionID);

        [LibraryImport("./dll/tls-client-windows-64-1.7.5.dll", EntryPoint = "destroySession")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        private static partial IntPtr destroySession([MarshalAs(UnmanagedType.LPWStr)] string sessionID);

        private readonly string _sessionID;
        private readonly TlsRequestMessage _sessionPayload;
        public bool UseCookies { get; set; } = true;
        public readonly Dictionary<string, string> Cookies = [];

        /// <summary>
        /// Default headers. Add any thing you want as default
        /// </summary>
        public readonly Dictionary<string, string> Headers = new()
        {
            ["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7",
            ["Accept-Encoding"] = "gzip, deflate, br, zstd",
            ["Accept-Language"] = "en,de;q=0.9,fr;q=0.8,ja;q=0.7,en-US;q=0.6",
            ["Cache-Control"] = "no-cache",
            ["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36"
        };

        /// <summary>
        /// Reference https://github.com/bogdanfinn/tls-client
        /// </summary>
        public TlsClient(string tlsClientIdentifier, int timeoutSeconds = 30, bool followRedirects = true, string proxy = "")
        {
            _sessionID = Guid.NewGuid().ToString();
            _sessionPayload = new TlsRequestMessage
            {
                TlsClientIdentifier = tlsClientIdentifier,
                FollowRedirects = followRedirects,
                InsecureSkipVerify = true,
                IsByteRequest = false,
                ForceHttp1 = false,
                WithDebug = false,
                CatchPanics = false,
                WithRandomTLSExtensionOrder = true,
                SessionId = _sessionID,
                TimeoutSeconds = timeoutSeconds,
                TimeoutMilliseconds = 0,
                CertificatePinningHosts = [],
                ProxyUrl = proxy,
                IsRotatingProxy = false,
                Headers = Headers,
                HeaderOrder = [.. Headers.Keys],
                RequestUrl = string.Empty,
                RequestMethod = string.Empty,
                RequestBody = string.Empty,
                WithDefaultCookieJar = false,
                WithoutCookieJar = false,
                RequestCookies = Cookies.ToTlsCookies()
            };
        }

        public TlsRequestResponse Get(string url, Dictionary<string, string>? additionalHeaders = null)
        {
            return MakeRequest("GET", url, additionalHeaders);
        }

        public TlsRequestResponse Post(string url, Dictionary<string, string>? additionalHeaders = null, string body = "")
        {
            return MakeRequest("POST", url, additionalHeaders, body);
        }
        public TlsRequestResponse Patch(string url, Dictionary<string, string>? additionalHeaders = null, string body = "")
        {
            return MakeRequest("PATCH", url, additionalHeaders, body);
        }
        public TlsRequestResponse Put(string url, Dictionary<string, string>? additionalHeaders = null, string body = "")
        {
            return MakeRequest("PUT", url, additionalHeaders, body);
        }
        public TlsRequestResponse Delete(string url, Dictionary<string, string>? additionalHeaders = null)
        {
            return MakeRequest("DELETE", url, additionalHeaders);
        }

        public TlsRequestResponse MakeRequest(string method, string url, Dictionary<string, string>? additionalHeaders, string body = "")
        {
            _sessionPayload.RequestMethod = method;
            _sessionPayload.RequestUrl = url;
            _sessionPayload.Headers.MergeDictionary(additionalHeaders);
            _sessionPayload.RequestBody = body;
            if (UseCookies)
            {
                _sessionPayload.RequestCookies = Cookies.ToTlsCookies();
            }

            var requestJson = JsonConvert.SerializeObject(_sessionPayload);
            var requestBytes = Encoding.UTF8.GetBytes(requestJson);

            IntPtr responsePtr = request(requestBytes, _sessionID);
            var responseJson = Marshal.PtrToStringAnsi(responsePtr) ?? throw new Exception("can't get response");
            var result = JsonConvert.DeserializeObject<TlsRequestResponse>(responseJson) ?? throw new Exception("can't parse response");
            freeMemory(result.Id);
            HandleCookies(result);
            return result;
        }

        private void HandleCookies(TlsRequestResponse response)
        {
            if (!UseCookies)
            {
                return;
            }
            Cookies.MergeDictionary(response.Cookies);
        }



        public void Dispose()
        {
            freeMemory(_sessionID);
            GC.SuppressFinalize(this);
        }
    }
}
