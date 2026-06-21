<?php

echo "Requesting access token from https://%AUTH0_DOMAIN%/oauth/token...\n";

$token = json_decode(file_get_contents("https://%AUTH0_DOMAIN%/oauth/token", false, stream_context_create([
  "http" => [
    "method" => "POST",
    "header" => "content-type: application/json",
    "content" => json_encode([
      "client_id" => "%AUTH0_CLIENT_ID%",
      "client_secret" => getenv("AUTH0_CLIENT_SECRET"),
      "audience" => "%AUTH0_AUDIENCE%",
      "scope" => "%AUTH0_SCOPE%",
      "grant_type" => "client_credentials"
    ])
  ]
])), true);

[$header, $payload] = explode('.', $token['access_token']);
echo "Access token obtained. Scopes: {$token['scope']}. Expires in {$token['expires_in']} seconds.\n\n";
// In production, cache this token and only renew it before it expires --
// no need to request a new one for every API call.

echo "Calling API at %API_ENDPOINT%...\n";

$apiRes = file_get_contents("%API_ENDPOINT%", false, stream_context_create([
  "http" => [
    "method" => "%HTTP_METHOD%",
    "header" => "authorization: {$token['token_type']} {$token['access_token']}"
  ]
]));

echo "API response:\n" . json_encode(json_decode($apiRes), JSON_PRETTY_PRINT) . "\n";
echo "\ninspect the token at https://jwt.io/#value={$header}.{$payload}\n";
