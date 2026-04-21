package main

import (
  "fmt"
  "io"
  "log"
  "net/http"
)

func main() {
  apiReq, _ := http.NewRequest("GET", "%API_ENDPOINT%", nil)
  apiReq.Header.Add("authorization", "Bearer <Your Access Token>")
  apiRes, _ := http.DefaultClient.Do(apiReq)
  defer apiRes.Body.Close()
  apiBody, _ := io.ReadAll(apiRes.Body)

  if apiRes.StatusCode != http.StatusOK {
    log.Fatalf("Error calling API: %s", string(apiBody))
  }

  fmt.Println("Response:", string(apiBody))
}
