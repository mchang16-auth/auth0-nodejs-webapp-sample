import os
import requests

def get_access_token():
    response = requests.post(
        f"https://{os.getenv('AUTH0_DOMAIN')}/oauth/token",
        headers={"content-type": "application/json"},
        json={
            "client_id": os.getenv("AUTH0_CLIENT_ID"),
            "client_secret": os.getenv("AUTH0_CLIENT_SECRET"),
            "audience": os.getenv("AUTH0_AUDIENCE"),
            "grant_type": "client_credentials"
        }
    )

    if not response.ok:
        print("Error getting token:", response.text)
        exit(1)

    return response.json()["access_token"]

def call_api(access_token):
    api_response = requests.get(
        "%API_ENDPOINT%",
        headers={"authorization": "Bearer " + access_token}
    )

    if not api_response.ok:
        print("Error calling API:", api_response.text)
        exit(1)

    print("Response:", api_response.text)

access_token = get_access_token()
call_api(access_token)
