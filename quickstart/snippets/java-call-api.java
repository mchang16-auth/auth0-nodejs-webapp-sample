import kong.unirest.HttpResponse;
import kong.unirest.Unirest;

public class CallApi {
  public static void main(String[] args) {
    HttpResponse<String> apiResponse = Unirest.get("%API_ENDPOINT%")
      .header("authorization", "Bearer <Your Access Token>")
      .asString();

    if (apiResponse.getStatus() != 200) {
      System.err.println("Error calling API: " + apiResponse.getBody());
      return;
    }

    System.out.println("Response: " + apiResponse.getBody());
  }
}
