import requests

api_response = requests.get(
    os.getenv("API_ENDPOINT"),
    headers={"authorization": "Bearer <Your Access Token>"}
)

if not api_response.ok:
    print("Error calling API:", api_response.text)
    exit(1)

print("Response:", api_response.text)
