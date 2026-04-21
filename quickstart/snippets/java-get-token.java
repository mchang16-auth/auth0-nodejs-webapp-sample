import kong.unirest.HttpResponse;
import kong.unirest.Unirest;
import org.json.JSONObject;

public class GetToken {
  public static void main(String[] args) {
    JSONObject tokenRequestBody = new JSONObject()
      .put("client_id", System.getenv("AUTH0_CLIENT_ID"))
      .put("client_secret", System.getenv("AUTH0_CLIENT_SECRET"))
      .put("audience", System.getenv("AUTH0_AUDIENCE"))
      .put("grant_type", "client_credentials");

    HttpResponse<String> response = Unirest.post("https://" + System.getenv("AUTH0_DOMAIN") + "/oauth/token")
      .header("content-type", "application/json")
      .body(tokenRequestBody.toString())
      .asString();

    if (response.getStatus() != 200) {
      System.err.println("Error getting token: " + response.getBody());
      return;
    }

    String accessToken = new JSONObject(response.getBody()).getString("access_token");
    System.out.println("Access Token: " + accessToken);
  }
}
