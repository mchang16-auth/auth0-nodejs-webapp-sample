using System.Net.Http;
using System.Text.Json;

var client = new HttpClient();

Console.WriteLine("Requesting access token from https://%AUTH0_DOMAIN%/oauth/token...");

var secret = Environment.GetEnvironmentVariable("AUTH0_CLIENT_SECRET");
var tokenRes = await client.PostAsync("https://%AUTH0_DOMAIN%/oauth/token",
  new StringContent(
    "{\"client_id\":\"%AUTH0_CLIENT_ID%\","
    + "\"client_secret\":\"" + secret + "\","
    + "\"audience\":\"%AUTH0_AUDIENCE%\","
    + "\"scope\":\"%AUTH0_SCOPE%\","
    + "\"grant_type\":\"client_credentials\"}",
    System.Text.Encoding.UTF8, "application/json"));

tokenRes.EnsureSuccessStatusCode();
var token = JsonDocument.Parse(await tokenRes.Content.ReadAsStringAsync()).RootElement;
var accessToken = token.GetProperty("access_token").GetString()!;
var parts = accessToken.Split('.');
Console.WriteLine($"Access token obtained. Scopes: {token.GetProperty("scope")}. Expires in {token.GetProperty("expires_in")} seconds.\n");
// In production, cache this token and only renew it before it expires --
// no need to request a new one for every API call.

Console.WriteLine("Calling API at %API_ENDPOINT%...");
var apiReq = new HttpRequestMessage(HttpMethod.Get, "%API_ENDPOINT%");
apiReq.Headers.Add("Authorization", $"{token.GetProperty("token_type")} {accessToken}");
var apiRes = await client.SendAsync(apiReq);
apiRes.EnsureSuccessStatusCode();

Console.WriteLine("API response:");
using (var w = new Utf8JsonWriter(Console.OpenStandardOutput(), new JsonWriterOptions { Indented = true })) {
  JsonDocument.Parse(await apiRes.Content.ReadAsStringAsync()).WriteTo(w);
}
Console.WriteLine($"\n\ninspect the token at https://jwt.io/#value={parts[0]}.{parts[1]}");
