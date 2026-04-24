import java.net.*;
import java.net.http.*;
import java.net.http.HttpRequest.BodyPublishers;
import java.net.http.HttpResponse.BodyHandlers;
import java.util.regex.*;

public class m2m {
  public static void main(String[] args) throws Exception {
    System.out.println("Requesting access token from https://%AUTH0_DOMAIN%/oauth/token...");

    var client = HttpClient.newHttpClient();

    var tokenRes = client.send(HttpRequest.newBuilder()
      .uri(URI.create("https://%AUTH0_DOMAIN%/oauth/token"))
      .header("content-type", "application/json")
      .POST(BodyPublishers.ofString("{"
        + "\"client_id\": \"%AUTH0_CLIENT_ID%\","
        + "\"client_secret\": \"" + System.getenv("AUTH0_CLIENT_SECRET") + "\","
        + "\"audience\": \"%AUTH0_AUDIENCE%\","
        + "\"scope\": \"%AUTH0_SCOPE%\","
        + "\"grant_type\": \"client_credentials\""
        + "}"))
      .build(), BodyHandlers.ofString());

    if (tokenRes.statusCode() != 200) {
      System.err.println("Failed to get token (" + tokenRes.statusCode() + "): " + tokenRes.body());
      System.exit(1);
    }

    // Normally you'd use a JSON library (e.g. Jackson, Gson) to parse this.
    // Regex here just to keep the quickstart dependency-free.
    var m = Pattern.compile("\"access_token\"\\s*:\\s*\"([^\"]+)\"").matcher(tokenRes.body());
    if (!m.find()) {
      System.err.println("No access_token in response");
      System.exit(1);
    }
    var accessToken = m.group(1);
    System.out.println("Access token obtained.");
    // In production, cache this token and only renew it before it expires --
    // no need to request a new one for every API call.

    System.out.println("Calling API at %API_ENDPOINT%...");
    var apiRes = client.send(HttpRequest.newBuilder()
      .uri(URI.create("%API_ENDPOINT%"))
      .header("Authorization", "Bearer " + accessToken)
      .build(), BodyHandlers.ofString());

    if (apiRes.statusCode() != 200) {
      System.err.println("API error (" + apiRes.statusCode() + "): " + apiRes.body());
      System.exit(1);
    }

    System.out.println("API response: " + apiRes.body());
  }
}
