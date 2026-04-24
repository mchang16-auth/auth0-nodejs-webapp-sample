using System.Net.Http;
using System.Text.Json;

var client = new HttpClient();

Console.WriteLine("Requesting access token from https://%AUTH0_DOMAIN%/oauth/token...");

var tokenRes = await client.PostAsync("https://%AUTH0_DOMAIN%/oauth/token",
  new StringContent($$"""
    {
      "client_id": "%AUTH0_CLIENT_ID%",
      "client_secret": "{{Environment.GetEnvironmentVariable("AUTH0_CLIENT_SECRET")}}",
      "audience": "%AUTH0_AUDIENCE%",
      "scope": "%AUTH0_SCOPE%",
      "grant_type": "client_credentials"
    }
    """, System.Text.Encoding.UTF8, "application/json"));

tokenRes.EnsureSuccessStatusCode();
var token = JsonDocument.Parse(await tokenRes.Content.ReadAsStringAsync()).RootElement;
Console.WriteLine("Access token obtained.\n");
// In production, cache this token and only renew it before it expires --
// no need to request a new one for every API call.

Console.WriteLine("Calling API at %API_ENDPOINT%...");
var apiReq = new HttpRequestMessage(HttpMethod.Get, "%API_ENDPOINT%");
apiReq.Headers.Add("Authorization", $"{token.GetProperty("token_type")} {token.GetProperty("access_token")}");
var apiRes = await client.SendAsync(apiReq);
apiRes.EnsureSuccessStatusCode();

Console.Write("API response: ");
using (var w = new Utf8JsonWriter(Console.OpenStandardOutput(), new JsonWriterOptions { Indented = true })) {
  JsonDocument.Parse(await apiRes.Content.ReadAsStringAsync()).WriteTo(w);
}
Console.WriteLine();
