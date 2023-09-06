using Entities;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace TestProjectDDDApi;

[TestClass]
public class MessageTest
{
    public static string? Token { get; set; }

    [TestMethod]
    public void GetMessageById()
    {
        var result = Get("https://localhost:7225/api/Messages?id=2");
        var message = JsonConvert.DeserializeObject<Message>(result);
        Assert.IsTrue(message != null);
    }

    [TestMethod]
    public void GetMessageByUserId()
    {
        var result = Get("https://localhost:7225/api/Messages/List");
        Assert.IsTrue(result != null);
    }

    //[TestMethod]
    //public void CreateMessage()
    //{
    //    var response = Get("");
    //    var messages = ;
    //}

    //[TestMethod]
    //public void DeleteMessage()
    //{

    //}

    /// <summary>
    /// Método base para chamadas <c>GET</c>.
    /// </summary>
    /// <param name="url"></param>
    /// <returns>Resposta da chamada ou <c>null</c>.</returns>
    public static string Get(string url)
    {
        GetToken();

        if (!string.IsNullOrWhiteSpace(Token))
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                var response = client.GetStringAsync(url);
                response.Wait();
                return response.Result;
            }

        return string.Empty;
    }
    /// <summary>
    /// Método base para chamadas <c>POST</c>.
    /// </summary>
    /// <param name="url"></param>
    /// <param name="data"></param>
    /// <returns>Resposta da chamada ou <c>null</c>.</returns>
    public static async Task<string?> Post(string url, object? data = null)
    {
        string json = data != null ? JsonConvert.SerializeObject(data) : "";
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        GetToken();
        if (!string.IsNullOrWhiteSpace(Token))
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                var response = client.PostAsync(url, content);
                response.Wait();
                if (response.Result.IsSuccessStatusCode)
                    return await response.Result.Content.ReadAsStringAsync();
            }
        }
        return null;
    }

    /// <summary>
    /// Obtém e define um token para os testes.
    /// </summary>
    public static void GetToken()
    {
        string getTokenUrl = "https://localhost:7225/api/Users/login";

        using (var client = new HttpClient())
        {
            string email = "pedro2@email.com";
            string password = "Abc@1234";
            var data = new { email, password };

            string jsonData = JsonConvert.SerializeObject(data);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = client.PostAsync(getTokenUrl, content);
            response.Wait();

            if (response.Result.IsSuccessStatusCode)
            {
                var tokenJson = response.Result.Content.ReadAsStringAsync();
                Token = tokenJson.Result.ToString();
            }
        }
    }
}