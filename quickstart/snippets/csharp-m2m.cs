using Newtonsoft.Json;
using RestSharp;

// Request access token from Auth0
var client = new RestClient($"https://{Environment.GetEnvironmentVariable("AUTH0_DOMAIN")}/oauth/token");
var request = new RestRequest(Method.Post);
var tokenRequest = new
{
    client_id = Environment.GetEnvironmentVariable("AUTH0_CLIENT_ID"),
    client_secret = Environment.GetEnvironmentVariable("AUTH0_CLIENT_SECRET"),
    audience = Environment.GetEnvironmentVariable("AUTH0_AUDIENCE"),
    grant_type = "client_credentials"
};

request.AddJsonBody(tokenRequest);
RestResponse response = client.Execute(request);

if (!response.IsSuccessful)
{
    Console.Error.WriteLine("Error getting token: " + response.Content);
    return;
}

var tokenResponse = JsonConvert.DeserializeObject<dynamic>(response.Content);
string accessToken = tokenResponse.access_token;

// Make API request using the access token
var apiClient = new RestClient("%API_ENDPOINT%");
var apiRequest = new RestRequest(Method.Get);
apiRequest.AddHeader("Authorization", $"Bearer {accessToken}");
RestResponse apiResponse = apiClient.Execute(apiRequest);

if (!apiResponse.IsSuccessful)
{
    Console.Error.WriteLine("Error calling API: " + apiResponse.Content);
    return;
}

Console.WriteLine("Response: " + apiResponse.Content);
