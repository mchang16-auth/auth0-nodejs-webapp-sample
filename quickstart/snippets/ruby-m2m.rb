require 'uri'
require 'net/http'
require 'json'

def get_access_token
  url = URI("https://#{ENV['AUTH0_DOMAIN']}/oauth/token")

  http = Net::HTTP.new(url.host, url.port)
  http.use_ssl = true

  request = Net::HTTP::Post.new(url)
  request["content-type"] = 'application/json'
  request.body = {
    client_id: ENV['AUTH0_CLIENT_ID'],
    client_secret: ENV['AUTH0_CLIENT_SECRET'],
    audience: ENV['AUTH0_AUDIENCE'],
    scope: ENV['AUTH0_SCOPE'],
    grant_type: "client_credentials"
  }.to_json

  response = http.request(request)

  unless response.is_a?(Net::HTTPSuccess)
    $stderr.puts "Error getting token: #{response.body}"
    exit 1
  end

  JSON.parse(response.read_body)['access_token']
end

def call_api(access_token)
  api_url = URI(ENV['API_ENDPOINT'])
  api_http = Net::HTTP.new(api_url.host, api_url.port)
  api_request = Net::HTTP::Get.new(api_url)
  api_request["authorization"] = "Bearer #{access_token}"
  api_response = api_http.request(api_request)

  unless api_response.is_a?(Net::HTTPSuccess)
    $stderr.puts "Error calling API: #{api_response.body}"
    exit 1
  end

  puts "Response: #{api_response.read_body}"
end

access_token = get_access_token
call_api(access_token)
