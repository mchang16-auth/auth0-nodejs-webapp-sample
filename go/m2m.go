package main

import (
	"bytes"
	"encoding/json"
	"fmt"
	"io"
	"log"
	"net/http"
	"os"
	"strings"
)

func main() {
	fmt.Println("Requesting access token from https://%AUTH0_DOMAIN%/oauth/token...")

	tokenRes, err := http.Post("https://%AUTH0_DOMAIN%/oauth/token", "application/json", strings.NewReader(`{
		"client_id":     "%AUTH0_CLIENT_ID%",
		"client_secret": "`+os.Getenv("AUTH0_CLIENT_SECRET")+`",
		"audience":      "%AUTH0_AUDIENCE%",
		"scope":         "%AUTH0_SCOPE%",
		"grant_type":    "client_credentials"
	}`))
	if err != nil {
		log.Fatalf("Request failed: %v", err)
	}
	defer tokenRes.Body.Close()

	if tokenRes.StatusCode != http.StatusOK {
		body, _ := io.ReadAll(tokenRes.Body)
		log.Fatalf("Failed to get token (%d): %s", tokenRes.StatusCode, body)
	}

	var token map[string]interface{}
	json.NewDecoder(tokenRes.Body).Decode(&token)

	fmt.Printf("Access token obtained. Scopes: %s. Expires in %.0f seconds.\n", token["scope"], token["expires_in"])
	// In production, cache this token and only renew it before it expires —
	// no need to request a new one for every API call.

	fmt.Println("Calling API at %API_ENDPOINT%...")
	apiReq, _ := http.NewRequest("%HTTP_METHOD%", "%API_ENDPOINT%", nil)
	apiReq.Header.Set("Authorization", token["token_type"].(string)+" "+token["access_token"].(string))

	apiRes, err := http.DefaultClient.Do(apiReq)
	if err != nil {
		log.Fatalf("Request failed: %v", err)
	}
	defer apiRes.Body.Close()

	if apiRes.StatusCode != http.StatusOK {
		body, _ := io.ReadAll(apiRes.Body)
		log.Fatalf("API error (%d): %s", apiRes.StatusCode, body)
	}

	apiBody, _ := io.ReadAll(apiRes.Body)
	var pretty bytes.Buffer
	json.Indent(&pretty, apiBody, "", "  ")
	fmt.Println("API response:")
	fmt.Println(pretty.String())

	parts := strings.SplitN(token["access_token"].(string), ".", 3)
	fmt.Printf("\ninspect the token at https://jwt.io/#value=%s.%s\n", parts[0], parts[1])
}
