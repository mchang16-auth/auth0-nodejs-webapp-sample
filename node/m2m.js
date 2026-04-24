console.log("Requesting access token from https://%AUTH0_DOMAIN%/oauth/token...");

const tokenResponse = await fetch("https://%AUTH0_DOMAIN%/oauth/token", {
  method: "POST",
  headers: { "content-type": "application/json" },
  body: JSON.stringify({
    client_id: "%AUTH0_CLIENT_ID%",
    client_secret: process.env.AUTH0_CLIENT_SECRET,
    audience: "%AUTH0_AUDIENCE%",
    scope: "%AUTH0_SCOPE%",
    grant_type: "client_credentials",
  }),
});

if (!tokenResponse.ok) {
  console.error(`Failed to get token (${tokenResponse.status}):`, await tokenResponse.text());
  process.exit(1);
}

const { access_token, expires_in, scope, token_type } = await tokenResponse.json();
const [header, payload] = access_token.split(".");
console.log(`Access token obtained. Scopes: ${scope}. Expires in ${expires_in} seconds.\n`);
// In production, cache this token and only renew it before it expires —
// no need to request a new one for every API call.

console.log("Calling API at %API_ENDPOINT%...");
const apiResponse = await fetch("%API_ENDPOINT%", {
  headers: { authorization: `${token_type} ${access_token}` },
});

if (!apiResponse.ok) {
  console.error(`API error (${apiResponse.status}):`, await apiResponse.text());
  process.exit(1);
}

console.log("API response:", await apiResponse.json());
console.log(`inspect the token at https://jwt.io/#value=${header}.${payload}\n`);
