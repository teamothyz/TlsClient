using TlsLib;

//proxy: http://user:pass@ip:host or http://ip:host
var client = new TlsClient(Constaints.TlsClientIdentifiersDictionary[TlsClientIdentifier.Chrome_124], proxy: "");
client.Headers["Origin"] = "https://www.google.com";
client.Headers["Referer"] = "https://www.google.com/";
var response = client.Get("https://www.google.com");
Console.WriteLine($"Cookies: ");
foreach (var cookie in response.Cookies)
{
    Console.WriteLine($"{cookie.Key}: {cookie.Value}");
}
Console.ReadKey();