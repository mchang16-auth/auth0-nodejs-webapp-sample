async function callApi() {
  const apiResponse = await fetch("%API_ENDPOINT%", {
    method: "GET",
    headers: {
      authorization: "Bearer <Your Access Token>",
    },
  });

  if (!apiResponse.ok) {
    const error = await apiResponse.text();
    throw new Error("Error calling API: " + error);
  }

  const apiData = await apiResponse.json();
  console.log("Response:", apiData);
}

callApi().catch(console.error);
