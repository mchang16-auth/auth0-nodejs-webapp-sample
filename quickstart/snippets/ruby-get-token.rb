require 'uri'
require 'net/http'
require 'json'

url = URI("https://#{ENV['AUTH0_DOMAIN']}/oauth/token")

http = Net::HTTP.new(url.host, url.port)
http.use_ssl = true

request = Net::HTTP::Post.new(url)
request["content-type"] = 'application/json'
request.body = {
  client_id: ENV['AUTH0_CLIENT_ID'],
  client_secret: ENV['AUTH0_CLIENT_SECRET'],
  audience: ENV['AUTH0_AUDIENCE'],
  grant_type: "client_credentials"
}.to_json

response = http.request(request)

unless response.is_a?(Net::HTTPSuccess)
  $stderr.puts "Error getting token: #{response.body}"
  exit 1
end

access_token = JSON.parse(response.read_body)['access_token']
puts "Access Token: #{access_token}"
