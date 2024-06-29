namespace TlsLib
{
    public static class Extensions
    {
        public static List<TlsCookie> ToTlsCookies(this Dictionary<string, string> cookies)
        {
            return cookies.Select(c => new TlsCookie
            {
                Name = c.Key,
                Value = c.Value
            }).ToList();
        }

        public static Dictionary<string, string> ToDictionary(this List<TlsCookie> cookies)
        {
            var cookieDictionary = new Dictionary<string, string>();
            foreach (var cookie in cookies)
            {
                cookieDictionary[cookie.Name] = cookie.Value;
            }
            return cookieDictionary;
        }

        public static Dictionary<string, string> MergeDictionary(this Dictionary<string, string> originalHeaders, Dictionary<string, string>? additionalHeaders)
        {
            if (additionalHeaders == null)
            {
                return originalHeaders;
            }
            foreach (var header in additionalHeaders)
            {
                originalHeaders[header.Key] = header.Value;
            }
            return originalHeaders;
        }
    }
}
