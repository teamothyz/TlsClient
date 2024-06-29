namespace TlsLib
{
    public class TlsRequestMessage
    {
        public string TlsClientIdentifier { get; set; } = "FireFox110";
        public bool FollowRedirects { get; set; } = true;
        public bool InsecureSkipVerify { get; set; } = false;
        public bool WithoutCookieJar { get; set; } = false;
        public bool WithDefaultCookieJar { get; set; } = false;
        public bool IsByteRequest { get; set; } = false;
        public bool ForceHttp1 { get; set; } = false;
        public bool WithDebug { get; set; } = false;
        public bool CatchPanics { get; set; } = false;
        public bool WithRandomTLSExtensionOrder { get; set; } = false;
        public string SessionId { get; set; } = "Nada";
        public int TimeoutSeconds { get; set; } = 30;
        public int TimeoutMilliseconds { get; set; } = 0;
        public Dictionary<string, string> CertificatePinningHosts { get; set; } = [];
        public string ProxyUrl { get; set; } = string.Empty;
        public bool IsRotatingProxy { get; set; } = false;
        public Dictionary<string, string> Headers { get; set; } = [];
        public List<string> HeaderOrder { get; set; } = null!;
        public string RequestUrl { get; set; } = null!;
        public string RequestMethod { get; set; } = null!;
        public string RequestBody { get; set; } = null!;
        public List<TlsCookie> RequestCookies { get; set; } = [];
    }
}
