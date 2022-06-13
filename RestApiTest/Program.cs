using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

class Program
{
    private static readonly HttpClient client = new HttpClient();
    static async Task Main(string[] args)
    {
        Console.WriteLine("Test FatSecret API");
        WebToken responseObject = await RequestFatSecretWebToken();
        Console.WriteLine(responseObject.AccessToken);
        Console.WriteLine(responseObject.TokenType);
        Console.WriteLine(responseObject.ExpiresIn);     
        
        Console.WriteLine("Test GitHub API");
        var repositories = await ProcessRepositories();

        foreach (var repo in repositories)
        {
            Console.WriteLine(repo.Name);
            Console.WriteLine(repo.Description);
            Console.WriteLine(repo.GitHubHomeUrl);
            Console.WriteLine(repo.Homepage);
            Console.WriteLine(repo.Watchers);
            Console.WriteLine(repo.LastPush);
            Console.WriteLine();
        }
    }
    private static async Task<WebToken> RequestFatSecretWebToken()
    {
        var byteArray = Encoding.ASCII.GetBytes("3c8adbf007b744f0a1e57826d2b21027:1d7a9908768f455d843d44757fce4def");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        var values = new Dictionary<string, string>
        {
           { "scope", "basic" },
           { "grant_type", "client_credentials" }
        };
        var content = new FormUrlEncodedContent(values);
        //////////System.IO.Stream response = await client.PostAsync("https://oauth.fatsecret.com/connect/token", content);

        //var responseString = await response.Content.ReadAsStringAsync();
        //////////var responseObject = await JsonSerializer.Deserialize() .DeserializeAsync<WebToken>(response);

        ////////////return responseObject;
        return null; 
    }
    static async Task<List<Repository>> ProcessRepositories()
    {
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
        client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

        //var stringTask = client.GetStringAsync("https://api.github.com/orgs/dotnet/repos");
        //var msg = await stringTask;
        var streamTask = client.GetStreamAsync("https://api.github.com/orgs/dotnet/repos");
        var repositories = await JsonSerializer.DeserializeAsync<List<Repository>>(await streamTask);
        //var msg = await stringTask;
        //Console.Write(msg);
        //foreach (var repo in repositories)
        //    Console.WriteLine(repo.Name);
        return repositories;
    }
}
public class WebToken
{
    [JsonPropertyName("access_token")]
    public string AccessToken;
    [JsonPropertyName("token_type")]
    public string TokenType;
    [JsonPropertyName("expires_in")]
    public DateTime ExpiresIn;
}
public class Repository
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("description")]
    public string Description { get; set; }
    [JsonPropertyName("html_url")]
    public Uri GitHubHomeUrl { get; set; }
    [JsonPropertyName("homepage")]
    public Uri Homepage { get; set; }
    [JsonPropertyName("watchers")]
    public int Watchers { get; set; }
    [JsonPropertyName("pushed_at")]
    public DateTime LastPushUtc { get; set; }
    public DateTime LastPush => LastPushUtc.ToLocalTime();
}
