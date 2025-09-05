namespace Campus_Connect.Services
{
using FirebaseAdmin;
    using System.Net.Http.Json;


    public class FirebaseServices
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public FirebaseServices(HttpClient httpClient, IConfiguration config) {
        
            _httpClient = httpClient;
            _apiKey = config["Firebase:ApiKey"];
        }
        public async Task<string?> LoginAsync(string email, string password)
        {
            var request = new
            {
                email = email,
                password = password,
                returnSecureToken = true
            };
            var response = await _httpClient.PostAsJsonAsync(
                $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={_apiKey}", request
                );

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadFromJsonAsync<Dictionary<string,object>>();
                return json["idToken"].ToString();
            }
            return null;
        }
    }
}
