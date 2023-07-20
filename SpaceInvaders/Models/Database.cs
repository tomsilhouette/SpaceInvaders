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
        }

        public async Task GetHighScoreRequest()
        {
            string apiUrlGet = "https://quiz-5cpoinrp2-webdesignbytom.vercel.app/users/all-users";

            using var httpClient = new HttpClient();

            try
            {
                HighScoresList.Clear();

                HttpResponseMessage response = await httpClient.GetAsync(apiUrlGet);

                response.EnsureSuccessStatusCode(); // Ensure the request was successful

                string responseBody = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(responseBody);

                // Parse the JSON response and extract the user data
                var jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseBody);

                var users = jsonResponse.data.users;

                foreach (var user in users)
                {
                    HighScoresList.Add(new User((string) user.username, (int) user.score));
                }

                HighScoresList = HighScoresList.OrderByDescending(u => u.Score).ToList();

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
            }
        }

        public async Task PostNewHighScore(string Username, int FinishingScore)
        {
            var username = Username;
            var score = FinishingScore;

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
                var content = new StringContent(json, Encoding.UTF8, "application/json");

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
