require 'uri'
require 'net/http'

api_url = URI(ENV['API_ENDPOINT'])
api_http = Net::HTTP.new(api_url.host, api_url.port)
api_request = Net::HTTP::Get.new(api_url)
api_request["authorization"] = "Bearer <Your Access Token>"
api_response = api_http.request(api_request)

unless api_response.is_a?(Net::HTTPSuccess)
  $stderr.puts "Error calling API: #{api_response.body}"
  exit 1
end

puts "Response: #{api_response.read_body}"
