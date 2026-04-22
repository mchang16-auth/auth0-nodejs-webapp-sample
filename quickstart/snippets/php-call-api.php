<?php

$apiCurl = curl_init();

curl_setopt_array($apiCurl, array(
  CURLOPT_URL => getenv("API_ENDPOINT"),
  CURLOPT_RETURNTRANSFER => true,
  CURLOPT_ENCODING => "",
  CURLOPT_MAXREDIRS => 10,
  CURLOPT_TIMEOUT => 30,
  CURLOPT_HTTP_VERSION => CURL_HTTP_VERSION_1_1,
  CURLOPT_CUSTOMREQUEST => "GET",
  CURLOPT_HTTPHEADER => array(
    "authorization: Bearer <Your Access Token>"
  ),
));

$apiResponse = curl_exec($apiCurl);
$apiErr = curl_error($apiCurl);
$apiHttpCode = curl_getinfo($apiCurl, CURLINFO_HTTP_CODE);
curl_close($apiCurl);

if ($apiErr) {
  fwrite(STDERR, "cURL Error: " . $apiErr . "\n");
} elseif ($apiHttpCode !== 200) {
  fwrite(STDERR, "Error calling API: HTTP " . $apiHttpCode . " - " . $apiResponse . "\n");
} else {
  echo "Response: " . $apiResponse;
}
