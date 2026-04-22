import os
import requests

response = requests.post(
    f"https://{os.getenv('AUTH0_DOMAIN')}/oauth/token",
    headers={"content-type": "application/json"},
    json={
        "client_id": os.getenv("AUTH0_CLIENT_ID"),
        "client_secret": os.getenv("AUTH0_CLIENT_SECRET"),
        "audience": os.getenv("AUTH0_AUDIENCE"),
        "scope": os.getenv("AUTH0_SCOPE"),
        "grant_type": "client_credentials"
    }
)

if not response.ok:
    print("Error getting token:", response.text)
    exit(1)

access_token = response.json()["access_token"]
print("Access Token:", access_token)
