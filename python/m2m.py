import os
import json
from urllib.request import Request, urlopen

print("Requesting access token from https://%AUTH0_DOMAIN%/oauth/token...")

token_res = urlopen(Request(
    "https://%AUTH0_DOMAIN%/oauth/token",
    headers={"content-type": "application/json"},
    data=json.dumps({
        "client_id": "%AUTH0_CLIENT_ID%",
        "client_secret": os.getenv("AUTH0_CLIENT_SECRET"),
        "audience": "%AUTH0_AUDIENCE%",
        "scope": "%AUTH0_SCOPE%",
        "grant_type": "client_credentials"
    }).encode()
))

token = json.loads(token_res.read())
print("Access token obtained.")
# In production, cache this token and only renew it before it expires --
# no need to request a new one for every API call.

print("Calling API at %API_ENDPOINT%...")

api_res = urlopen(Request(
    "%API_ENDPOINT%",
    headers={"authorization": token["token_type"] + " " + token["access_token"]}
))

print("API response:", api_res.read().decode())
