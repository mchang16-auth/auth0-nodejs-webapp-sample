using Newtonsoft.Json;
using RestSharp;

string GetAccessToken()
{
    var client = new RestClient($"https://{Environment.GetEnvironmentVariable("AUTH0_DOMAIN")}/oauth/token");
    var request = new RestRequest(Method.Post);
    var tokenRequest = new
    {
        client_id = Environment.GetEnvironmentVariable("AUTH0_CLIENT_ID"),
        client_secret = Environment.GetEnvironmentVariable("AUTH0_CLIENT_SECRET"),
        audience = Environment.GetEnvironmentVariable("AUTH0_AUDIENCE"),
        scope = Environment.GetEnvironmentVariable("AUTH0_SCOPE"),
        grant_type = "client_credentials"
    };

    request.AddJsonBody(tokenRequest);
    RestResponse response = client.Execute(request);

    if (!response.IsSuccessful)
    {
        throw new Exception("Error getting token: " + response.Content);
    }

    var tokenResponse = JsonConvert.DeserializeObject<dynamic>(response.Content);
    return (string)tokenResponse.access_token;
}

string CallApi(string accessToken)
{
    var apiClient = new RestClient(Environment.GetEnvironmentVariable("API_ENDPOINT"));
    var apiRequest = new RestRequest(Method.Get);
    apiRequest.AddHeader("Authorization", $"Bearer {accessToken}");
    RestResponse apiResponse = apiClient.Execute(apiRequest);

    if (!apiResponse.IsSuccessful)
    {
        throw new Exception("Error calling API: " + apiResponse.Content);
    }

    return apiResponse.Content;
}

var accessToken = GetAccessToken();
var result = CallApi(accessToken);
Console.WriteLine("Response: " + result);
