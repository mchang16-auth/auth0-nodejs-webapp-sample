using RestSharp;

var apiClient = new RestClient(Environment.GetEnvironmentVariable("API_ENDPOINT"));
var apiRequest = new RestRequest(Method.Get);
apiRequest.AddHeader("Authorization", "Bearer <Your Access Token>");
RestResponse apiResponse = apiClient.Execute(apiRequest);

if (!apiResponse.IsSuccessful)
{
    throw new Exception("Error calling API: " + apiResponse.Content);
}

Console.WriteLine("Response: " + apiResponse.Content);
