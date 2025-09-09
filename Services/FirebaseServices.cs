namespace Campus_Connect.Services
{
    using Firebase.Database;
    using Firebase.Database.Query;
    using FirebaseAdmin;
    using System.Net.Http.Json;


    public class FirebaseServices
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly FirebaseClient _db = new FirebaseClient("https://campus-connect-14d4f-default-rtdb.firebaseio.com/");

        public FirebaseServices(HttpClient httpClient, IConfiguration config)
        {

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
                var json = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();
                return json["idToken"].ToString();
            }
            return null;
        }

        public async Task<bool> AddUserToCommmunity(string userID, string communityID)
        {
            try
            {
                await _db
                    .Child("Communities")
                    .Child(communityID)
                    .Child("Users")
                    .Child(userID)
                    .PutAsync(true);
                return true;
            }catch(Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        
        }
    }
}
