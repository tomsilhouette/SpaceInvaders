using Newtonsoft.Json;
using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceInvaders.ViewModel;

namespace SpaceInvaders.Models
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class Database
    {
        public List<User> HighScoresList { get; set; } = new();

        public Database() 
        {
            HighScoresList.Add(new User("Tommmm7", 44));
        }

        public async Task GetHighScoreRequest()
        {
            Debug.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            string apiUrlGet = "https://quiz-5cpoinrp2-webdesignbytom.vercel.app/users/all-users";

            using var httpClient = new HttpClient();

            try
            {
                Debug.WriteLine("BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB");
                HttpResponseMessage response = await httpClient.GetAsync(apiUrlGet);

                response.EnsureSuccessStatusCode(); // Ensure the request was successful

                string responseBody = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(responseBody);

                // Parse the JSON response and extract the user data
                var jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseBody);

                var users = jsonResponse.data.users.ToObject<List<User>>();

                foreach (var user in users)
                {
                    Debug.WriteLine($"XXXXXXXXXXXXXXXXXX Username: {user.username}, Score: {user.score}");
                    HighScoresList.Add(new User(user.username, user.score));
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
            }
        }

        public async Task PostNewHighScore()
        {
            var username = "NewUser";
            var score = 100;

            var ApiUrlPost = "https://quiz-5cpoinrp2-webdesignbytom.vercel.app/users/post-score";
            using var httpClient = new HttpClient();

            var requestBody = new
            {
                username,
                score
            };

            try
            {
                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync(ApiUrlPost, content);

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                Debug.WriteLine(responseBody);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
            }
        }

    }
}
