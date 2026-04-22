async function getAccessToken() {
  const response = await fetch(
    `https://${process.env.AUTH0_DOMAIN}/oauth/token`,
    {
      method: "POST",
      headers: { "content-type": "application/json" },
      body: JSON.stringify({
        client_id: process.env.AUTH0_CLIENT_ID,
        client_secret: process.env.AUTH0_CLIENT_SECRET,
        audience: process.env.AUTH0_AUDIENCE,
        scope: process.env.AUTH0_SCOPE,
        grant_type: "client_credentials",
      }),
    },
  );

  if (!response.ok) {
    const error = await response.text();
    throw new Error("Error getting token: " + error);
  }

  const tokenData = await response.json();
  return tokenData.access_token;
}

async function callAPI(accessToken) {
  const apiResponse = await fetch(process.env.API_ENDPOINT, {
    method: "GET",
    headers: {
      authorization: `Bearer ${accessToken}`,
    },
  });

  if (!apiResponse.ok) {
    const error = await apiResponse.text();
    throw new Error("Error calling API: " + error);
  }

  const apiData = await apiResponse.json();
  console.log("Response:", apiData);
}

async function main() {
  try {
    const accessToken = await getAccessToken();
    await callAPI(accessToken);
  } catch (error) {
    console.error("Error:", error);
  }
}

main();
