require 'uri'
require 'net/http'
require 'json'

puts "Requesting access token from https://%AUTH0_DOMAIN%/oauth/token..."

token = JSON.parse(Net::HTTP.post(URI("https://%AUTH0_DOMAIN%/oauth/token"), {
  client_id: "%AUTH0_CLIENT_ID%",
  client_secret: ENV['AUTH0_CLIENT_SECRET'],
  audience: "%AUTH0_AUDIENCE%",
  scope: "%AUTH0_SCOPE%",
  grant_type: "client_credentials"
}.to_json, "content-type" => "application/json").body)

header, payload = token['access_token'].split('.')[0..1]
puts "Access token obtained. Scopes: #{token['scope']}. Expires in #{token['expires_in'].to_i} seconds.\n\n"
# In production, cache this token and only renew it before it expires --
# no need to request a new one for every API call.

puts "Calling API at %API_ENDPOINT%..."

api_uri = URI("%API_ENDPOINT%")
req = Net::HTTP.const_get("%HTTP_METHOD%".capitalize).new(api_uri)
req["authorization"] = "#{token['token_type']} #{token['access_token']}"
res = Net::HTTP.start(api_uri.hostname, api_uri.port, use_ssl: true) { |http| http.request(req) }
puts "API response:"
puts JSON.pretty_generate(JSON.parse(res.body))
puts "\ninspect the token at https://jwt.io/#value=#{header}.#{payload}"
