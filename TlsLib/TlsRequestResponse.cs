namespace TlsLib
{
    public class TlsRequestResponse
    {
        public string Id { get; set; } = null!;
        public string Body { get; set; } = null!;
        public Dictionary<string, string> Cookies { get; set; } = null!;
        public Dictionary<string, List<string>> Headers { get; set; } = [];
        public int Status { get; set; }
        public string Target { get; set; } = null!;
        public string UsedProtocol { get; set; } = null!;
    }
}
