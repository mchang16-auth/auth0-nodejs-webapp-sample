async function getToken() {
  const response = await fetch(
    `https://${process.env.AUTH0_DOMAIN}/oauth/token`,
    {
      method: "POST",
      headers: { "content-type": "application/json" },
      body: JSON.stringify({
        client_id: process.env.AUTH0_CLIENT_ID,
        client_secret: process.env.AUTH0_CLIENT_SECRET,
        audience: process.env.AUTH0_AUDIENCE,
        grant_type: "client_credentials",
      }),
    },
  );

  if (!response.ok) {
    const error = await response.text();
    throw new Error("Error getting token: " + error);
  }

  const tokenData = await response.json();
  console.log("Access Token:", tokenData.access_token);
}

getToken().catch(console.error);
