using gamon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GlucoMan
{
    /// <summary>
    /// Service for interacting with the FatSecret API.
    /// Supports both OAuth 2.0 Client Credentials (2-legged) and
    /// OAuth 1.0a 3-Legged Authentication flows.
    /// </summary>
    public class FatSecretService
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private static string _accessToken;
        private static DateTime _tokenExpiry = DateTime.MinValue;
        private static string _barcodeAccessToken;
        private static DateTime _barcodeTokenExpiry = DateTime.MinValue;

        // FatSecret API endpoints
        private const string TokenEndpoint = "https://oauth.fatsecret.com/connect/token";
        private const string ApiBaseUrl = "https://platform.fatsecret.com/rest/server.api";

        // OAuth 1.0 endpoints for 3-Legged Authentication
        private const string OAuth1RequestTokenUrl = "https://authentication.fatsecret.com/oauth/request_token";
        private const string OAuth1AuthorizeUrl = "https://authentication.fatsecret.com/oauth/authorize";
        private const string OAuth1AccessTokenUrl = "https://authentication.fatsecret.com/oauth/access_token";
        private const string OAuth1ApiBaseUrl = "https://platform.fatsecret.com/rest/server.api";

        // Callback URL intercepted by the WebView during authorization
        public const string OAuth1CallbackUrl = "https://localhost/fatsecret_callback";

        // API credentials - these should be stored securely
        // Get your own credentials at https://platform.fatsecret.com/
        private string _clientId;
        private string _clientSecret;

        // OAuth 1.0 Consumer Secret (Shared Secret) — may differ from _clientSecret
        private string _consumerSecret;

        // OAuth 1.0 user tokens (3-Legged)
        private string _oauth1AccessToken;
        private string _oauth1AccessTokenSecret;

        // Temporary request token used during authorization flow
        private string _oauth1RequestToken;
        private string _oauth1RequestTokenSecret;

        // Locale settings — default to Italian
        private string _language;
        private string _region;

        public string Language => _language;
        public string Region => _region;

        /// <summary>True when a user has completed 3-Legged OAuth 1.0 authorization.</summary>
        public bool HasUserToken => !string.IsNullOrEmpty(_oauth1AccessToken)
                                 && !string.IsNullOrEmpty(_oauth1AccessTokenSecret);

        public FatSecretService()
        {
            LoadCredentials();
        }

        private void LoadCredentials()
        {
            try
            {
                var bl = new BL_General();
                _clientId = bl.RestoreParameter("FatSecret_ClientId") ?? "";
                _clientSecret = bl.RestoreParameter("FatSecret_ClientSecret") ?? "";
                _consumerSecret = bl.RestoreParameter("FatSecret_ConsumerSecret") ?? "";
                _language = bl.RestoreParameter("FatSecret_Language") ?? "it";
                _region = bl.RestoreParameter("FatSecret_Region") ?? "IT";
                // Load persisted OAuth 1.0 user tokens
                _oauth1AccessToken = bl.RestoreParameter("FatSecret_OAuth1Token") ?? "";
                _oauth1AccessTokenSecret = bl.RestoreParameter("FatSecret_OAuth1TokenSecret") ?? "";
            }
            catch
            {
                _clientId = "";
                _clientSecret = "";
                _consumerSecret = "";
                _language = "it";
                _region = "IT";
                _oauth1AccessToken = "";
                _oauth1AccessTokenSecret = "";
            }
        }

        public bool HasCredentials => !string.IsNullOrEmpty(_clientId) && !string.IsNullOrEmpty(_clientSecret);

        /// <summary>True when the OAuth 1.0 Shared Secret is configured (needed for 3-Legged auth).</summary>
        public bool HasConsumerSecret => !string.IsNullOrEmpty(_consumerSecret);

        public void SetCredentials(string clientId, string clientSecret)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            var bl = new BL_General();
            bl.SaveParameter("FatSecret_ClientId", clientId);
            bl.SaveParameter("FatSecret_ClientSecret", clientSecret);
        }

        public void SetConsumerSecret(string consumerSecret)
        {
            _consumerSecret = consumerSecret;
            var bl = new BL_General();
            bl.SaveParameter("FatSecret_ConsumerSecret", consumerSecret);
        }

        /// <summary>Sets and saves the locale used for all FatSecret API requests.</summary>
        public void SetLocale(string language, string region)
        {
            _language = language;
            _region = region;
            var bl = new BL_General();
            bl.SaveParameter("FatSecret_Language", language);
            bl.SaveParameter("FatSecret_Region", region);
            // Invalidate cached tokens so next call re-authenticates cleanly
            _accessToken = null;
            _barcodeAccessToken = null;
        }

        /// <summary>Removes the stored OAuth 1.0 user tokens (logout).</summary>
        public void ClearUserToken()
        {
            _oauth1AccessToken = "";
            _oauth1AccessTokenSecret = "";
            var bl = new BL_General();
            bl.SaveParameter("FatSecret_OAuth1Token", "");
            bl.SaveParameter("FatSecret_OAuth1TokenSecret", "");
        }

        #region OAuth 1.0 Three-Legged Authentication

        /// <summary>
        /// Step 1 – Obtain a Request Token from FatSecret.
        /// </summary>
        public async Task<string> OAuth1_GetRequestTokenAsync()
        {
            var oauthParams = BuildOAuth1BaseParams();
            oauthParams["oauth_callback"] = OAuth1CallbackUrl;

            var signature = GenerateOAuth1Signature("POST", OAuth1RequestTokenUrl,
                oauthParams, _consumerSecret, "");
            oauthParams["oauth_signature"] = signature;

            // Send OAuth params as POST body (form-encoded)
            var request = new HttpRequestMessage(HttpMethod.Post, OAuth1RequestTokenUrl);
            request.Content = new FormUrlEncodedContent(oauthParams);

            var response = await _httpClient.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();

            General.LogOfProgram?.Debug($"FatSecret OAuth1 RequestToken HTTP {(int)response.StatusCode}: {body}");

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(
                    $"OAuth1 request token failed: {response.StatusCode} - {body}");

            var parsed = ParseFormUrlEncoded(body);
            _oauth1RequestToken = parsed["oauth_token"];
            _oauth1RequestTokenSecret = parsed["oauth_token_secret"];

            return _oauth1RequestToken;
        }

        /// <summary>
        /// Step 2 – Build the authorization URL that the user should visit.
        /// Call <see cref="OAuth1_GetRequestTokenAsync"/> first.
        /// </summary>
        public string OAuth1_GetAuthorizationUrl()
        {
            if (string.IsNullOrEmpty(_oauth1RequestToken))
                throw new InvalidOperationException(
                    "Call OAuth1_GetRequestTokenAsync before getting the authorization URL.");

            return $"{OAuth1AuthorizeUrl}?oauth_token={Uri.EscapeDataString(_oauth1RequestToken)}";
        }

        /// <summary>
        /// Step 3 – Exchange the authorized request token + verifier for an Access Token.
        /// </summary>
        public async Task OAuth1_ExchangeRequestTokenAsync(string oauthVerifier)
        {
            var oauthParams = BuildOAuth1BaseParams();
            oauthParams["oauth_token"] = _oauth1RequestToken;
            oauthParams["oauth_verifier"] = oauthVerifier;

            var signature = GenerateOAuth1Signature("POST", OAuth1AccessTokenUrl,
                oauthParams, _consumerSecret, _oauth1RequestTokenSecret);
            oauthParams["oauth_signature"] = signature;

            // Send OAuth params as POST body (form-encoded)
            var request = new HttpRequestMessage(HttpMethod.Post, OAuth1AccessTokenUrl);
            request.Content = new FormUrlEncodedContent(oauthParams);

            var response = await _httpClient.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();

            General.LogOfProgram?.Debug($"FatSecret OAuth1 AccessToken HTTP {(int)response.StatusCode}: {body}");

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(
                    $"OAuth1 access token exchange failed: {response.StatusCode} - {body}");

            var parsed = ParseFormUrlEncoded(body);
            _oauth1AccessToken = parsed["oauth_token"];
            _oauth1AccessTokenSecret = parsed["oauth_token_secret"];

            // Persist tokens
            var bl = new BL_General();
            bl.SaveParameter("FatSecret_OAuth1Token", _oauth1AccessToken);
            bl.SaveParameter("FatSecret_OAuth1TokenSecret", _oauth1AccessTokenSecret);
        }

        /// <summary>
        /// Makes an API call signed with the user's OAuth 1.0 access token.
        /// </summary>
        private async Task<string> MakeOAuth1ApiCallAsync(Dictionary<string, string> apiParams)
        {
            var oauthParams = BuildOAuth1BaseParams();
            oauthParams["oauth_token"] = _oauth1AccessToken;

            // Merge all params for signature computation
            var allParams = new Dictionary<string, string>(oauthParams);
            foreach (var kv in apiParams)
                allParams[kv.Key] = kv.Value;

            var signature = GenerateOAuth1Signature("POST", OAuth1ApiBaseUrl,
                allParams, _consumerSecret, _oauth1AccessTokenSecret);
            allParams["oauth_signature"] = signature;

            // Send all params (OAuth + API) as POST body
            var request = new HttpRequestMessage(HttpMethod.Post, OAuth1ApiBaseUrl);
            request.Content = new FormUrlEncodedContent(allParams);

            var response = await _httpClient.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(
                    $"FatSecret API call failed: {response.StatusCode} - {body}");

            return body;
        }

        // ---- OAuth 1.0 helpers ----

        private Dictionary<string, string> BuildOAuth1BaseParams()
        {
            return new Dictionary<string, string>
            {
                ["oauth_consumer_key"] = _clientId,
                ["oauth_signature_method"] = "HMAC-SHA1",
                ["oauth_timestamp"] = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                ["oauth_nonce"] = Guid.NewGuid().ToString("N"),
                ["oauth_version"] = "1.0"
            };
        }

        private static string GenerateOAuth1Signature(string httpMethod, string url,
            Dictionary<string, string> parameters, string consumerSecret, string tokenSecret)
        {
            // 1. Sort parameters alphabetically by key, then by value
            var sorted = parameters
                .OrderBy(p => p.Key)
                .ThenBy(p => p.Value);

            // 2. Build parameter string
            var paramString = string.Join("&",
                sorted.Select(p => $"{PercentEncode(p.Key)}={PercentEncode(p.Value)}"));

            // 3. Build Signature Base String
            var signatureBase = $"{httpMethod.ToUpperInvariant()}&{PercentEncode(url)}&{PercentEncode(paramString)}";

            // 4. Build signing key
            var signingKey = $"{PercentEncode(consumerSecret)}&{PercentEncode(tokenSecret)}";

            // 5. HMAC-SHA1
            using var hmac = new HMACSHA1(Encoding.ASCII.GetBytes(signingKey));
            var hash = hmac.ComputeHash(Encoding.ASCII.GetBytes(signatureBase));
            return Convert.ToBase64String(hash);
        }

        private static string PercentEncode(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            // RFC 5849 percent-encoding: encode everything except unreserved characters
            var encoded = new StringBuilder();
            foreach (var c in Encoding.UTF8.GetBytes(value))
            {
                if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') ||
                    (c >= '0' && c <= '9') || c == '-' || c == '.' || c == '_' || c == '~')
                {
                    encoded.Append((char)c);
                }
                else
                {
                    encoded.Append($"%{c:X2}");
                }
            }
            return encoded.ToString();
        }

        private static string BuildOAuthHeaderString(Dictionary<string, string> oauthParams)
        {
            return string.Join(", ", oauthParams
                .OrderBy(p => p.Key)
                .Select(p => $"{PercentEncode(p.Key)}=\"{PercentEncode(p.Value)}\""));
        }

        private static Dictionary<string, string> ParseFormUrlEncoded(string body)
        {
            var result = new Dictionary<string, string>();
            foreach (var pair in body.Split('&'))
            {
                var parts = pair.Split('=', 2);
                if (parts.Length == 2)
                    result[Uri.UnescapeDataString(parts[0])] = Uri.UnescapeDataString(parts[1]);
            }
            return result;
        }

        #endregion

        /// <summary>
        /// Get OAuth 2.0 access token using Client Credentials flow
        /// </summary>
        private async Task<string> GetAccessTokenAsync()
        {
            // Return cached token if still valid
            if (!string.IsNullOrEmpty(_accessToken) && DateTime.UtcNow < _tokenExpiry)
            {
                return _accessToken;
            }

            if (!HasCredentials)
            {
                throw new InvalidOperationException("FatSecret API credentials not configured. Please set Client ID and Client Secret in settings.");
            }

            var request = new HttpRequestMessage(HttpMethod.Post, TokenEndpoint);

            // Add Basic Authentication header
            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_clientId}:{_clientSecret}"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            // Request body
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("scope", "basic")
            });
            request.Content = content;

            var response = await _httpClient.SendAsync(request);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to get access token: {response.StatusCode} - {errorContent}");
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(responseJson);
            var root = doc.RootElement;

            _accessToken = root.GetProperty("access_token").GetString();
            var expiresIn = root.GetProperty("expires_in").GetInt32();
            _tokenExpiry = DateTime.UtcNow.AddSeconds(expiresIn - 60); // Refresh 1 minute early

            return _accessToken;
        }

        /// <summary>
        /// Get OAuth 2.0 access token with 'basic barcode' scope.
        /// Requires the barcode scope to be enabled on the FatSecret application
        /// at platform.fatsecret.com → My App → Barcode.
        /// </summary>
        private async Task<string> GetBarcodeAccessTokenAsync()
        {
            if (!string.IsNullOrEmpty(_barcodeAccessToken) && DateTime.UtcNow < _barcodeTokenExpiry)
                return _barcodeAccessToken;

            if (!HasCredentials)
                throw new InvalidOperationException("FatSecret API credentials not configured. Please set Client ID and Client Secret in settings.");

            var request = new HttpRequestMessage(HttpMethod.Post, TokenEndpoint);
            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_clientId}:{_clientSecret}"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);
            request.Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("scope", "basic barcode")
            });

            var response = await _httpClient.SendAsync(request);
            var responseJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                if (responseJson.Contains("invalid_scope"))
                    throw new InvalidOperationException(
                        "The 'barcode' scope is not enabled for your FatSecret application. " +
                        "Enable it at platform.fatsecret.com → My App → Edit → Barcode.");
                throw new HttpRequestException($"Failed to get barcode access token: {response.StatusCode} - {responseJson}");
            }

            using var doc = JsonDocument.Parse(responseJson);
            var root = doc.RootElement;
            _barcodeAccessToken = root.GetProperty("access_token").GetString();
            var expiresIn = root.GetProperty("expires_in").GetInt32();
            _barcodeTokenExpiry = DateTime.UtcNow.AddSeconds(expiresIn - 60);

            return _barcodeAccessToken;
        }

        /// <summary>
        /// Searches FatSecret for a food by barcode (EAN-13 / UPC).
        /// Calls food.find_id_for_barcode, then food.get.v4 for full details.
        /// Returns null if the barcode is not found.
        /// </summary>
        public async Task<FatSecretFood> FindFoodByBarcodeAsync(string barcode)
        {
            var apiParams = new Dictionary<string, string>
            {
                { "method", "food.find_id_for_barcode" },
                { "barcode", barcode },
                { "format", "json" },
                { "language", _language },
                { "region", _region }
            };

            string responseJson;
            if (HasUserToken)
            {
                responseJson = await MakeOAuth1ApiCallAsync(apiParams);
            }
            else
            {
                var token = await GetBarcodeAccessTokenAsync();
                var request = new HttpRequestMessage(HttpMethod.Post, ApiBaseUrl);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                request.Content = new FormUrlEncodedContent(apiParams);
                var response = await _httpClient.SendAsync(request);
                responseJson = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"Barcode lookup failed: {response.StatusCode} - {responseJson}");
            }

            General.LogOfProgram?.Debug($"FatSecret FindByBarcode: {responseJson}");
            CheckForApiError(responseJson);

            // Response: { "food_id": { "value": "33691" } }  or value "0" when not found
            using var doc = JsonDocument.Parse(responseJson);
            if (!doc.RootElement.TryGetProperty("food_id", out var foodIdNode))
                return null;

            var valueStr = foodIdNode.TryGetProperty("value", out var valueProp)
                ? valueProp.GetString()
                : foodIdNode.ValueKind == JsonValueKind.String ? foodIdNode.GetString() : null;

            if (!long.TryParse(valueStr, out var foodId) || foodId == 0)
                return null;

            // Fetch full nutritional details with the resolved food_id
            return await GetFoodDetailsAsync(foodId);
        }

        /// <summary>
        /// Search for foods in the FatSecret database
        /// </summary>
        public async Task<List<FatSecretFood>> SearchFoodsAsync(string searchExpression, int maxResults = 20)
        {
            var apiParams = new Dictionary<string, string>
            {
                { "method", "foods.search" },
                { "search_expression", searchExpression },
                { "format", "json" },
                { "max_results", maxResults.ToString() },
                { "page_number", "0" },
                { "language", _language },
                { "region", _region }
            };

            string responseJson;
            if (HasUserToken)
            {
                responseJson = await MakeOAuth1ApiCallAsync(apiParams);
            }
            else
            {
                var token = await GetAccessTokenAsync();
                var request = new HttpRequestMessage(HttpMethod.Post, ApiBaseUrl);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                request.Content = new FormUrlEncodedContent(apiParams);
                var response = await _httpClient.SendAsync(request);
                responseJson = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"Food search failed: {response.StatusCode} - {responseJson}");
            }

            General.LogOfProgram?.Debug($"FatSecret SearchFoods: {responseJson}");

            // FatSecret returns HTTP 200 even for API-level errors; check the JSON body
            CheckForApiError(responseJson);

            return ParseSearchResults(responseJson);
        }

        /// <summary>
        /// Get detailed information about a specific food
        /// </summary>
        public async Task<FatSecretFood> GetFoodDetailsAsync(long foodId)
        {
            var apiParams = new Dictionary<string, string>
            {
                { "method", "food.get.v4" },
                { "food_id", foodId.ToString() },
                { "format", "json" },
                { "language", _language },
                { "region", _region }
            };

            string responseJson;
            if (HasUserToken)
            {
                responseJson = await MakeOAuth1ApiCallAsync(apiParams);
            }
            else
            {
                var token = await GetAccessTokenAsync();
                var request = new HttpRequestMessage(HttpMethod.Post, ApiBaseUrl);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                request.Content = new FormUrlEncodedContent(apiParams);
                var response = await _httpClient.SendAsync(request);
                responseJson = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"Get food details failed: {response.StatusCode} - {responseJson}");
            }

            return ParseFoodDetails(responseJson);
        }

        private List<FatSecretFood> ParseSearchResults(string json)
        {
            var foods = new List<FatSecretFood>();

            try
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (!root.TryGetProperty("foods", out var foodsElement))
                {
                    General.LogOfProgram?.Debug("FatSecret ParseSearchResults: 'foods' property not found in response");
                    return foods;
                }

                if (foodsElement.TryGetProperty("total_results", out var totalProp))
                    General.LogOfProgram?.Debug($"FatSecret total_results: {totalProp.GetString()}");

                if (!foodsElement.TryGetProperty("food", out var foodArray))
                {
                    General.LogOfProgram?.Debug("FatSecret ParseSearchResults: 'food' property absent — API returned 0 results");
                    return foods;
                }

                // Handle both single food object and array
                if (foodArray.ValueKind == JsonValueKind.Array)
                {
                    foreach (var foodElement in foodArray.EnumerateArray())
                    {
                        var food = ParseFoodFromElement(foodElement);
                        if (food != null)
                            foods.Add(food);
                    }
                }
                else if (foodArray.ValueKind == JsonValueKind.Object)
                {
                    var food = ParseFoodFromElement(foodArray);
                    if (food != null)
                        foods.Add(food);
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("FatSecretService | ParseSearchResults", ex);
            }

            return foods;
        }

        /// <summary>
        /// Throws an exception if the JSON body contains a FatSecret API-level error.
        /// FatSecret returns HTTP 200 even for errors like invalid scope or unknown method.
        /// </summary>
        private static void CheckForApiError(string json)
        {
            try
            {
                using var doc = JsonDocument.Parse(json);
                if (doc.RootElement.TryGetProperty("error", out var errorProp))
                {
                    var code = errorProp.TryGetProperty("code", out var codeProp) ? codeProp.GetInt32() : -1;
                    var message = errorProp.TryGetProperty("message", out var msgProp) ? msgProp.GetString() : "Unknown API error";

                    var hint = code switch
                    {
                        21 => "\nFix: log in to platform.fatsecret.com → your app → Edit → 'Allowed IP Addresses': add your IP or use '*' to allow all.",
                        10 => "\nFix: check your Client ID and Client Secret in Settings.",
                        8  => "\nFix: the foods.search method is not available on your API plan.",
                        _  => string.Empty
                    };

                    throw new InvalidOperationException($"FatSecret API error {code}: {message}{hint}");
                }
            }
            catch (JsonException)
            {
                // Not valid JSON — the HTTP error check above will have caught real HTTP errors
            }
        }

        private FatSecretFood ParseFoodFromElement(JsonElement element)
        {
            try
            {
                var food = new FatSecretFood
                {
                    FoodId = element.TryGetProperty("food_id", out var idProp) ? 
                        long.Parse(idProp.GetString() ?? "0") : 0,
                    Name = element.TryGetProperty("food_name", out var nameProp) ? 
                        nameProp.GetString() : "",
                    Description = element.TryGetProperty("food_description", out var descProp) ? 
                        descProp.GetString() : "",
                    BrandName = element.TryGetProperty("brand_name", out var brandProp) ? 
                        brandProp.GetString() : "",
                    FoodType = element.TryGetProperty("food_type", out var typeProp) ? 
                        typeProp.GetString() : "",
                    FoodUrl = element.TryGetProperty("food_url", out var urlProp) ? 
                        urlProp.GetString() : ""
                };

                // Parse nutritional info from description if available
                // FatSecret search results include a description like:
                // "Per 100g - Calories: 89kcal | Fat: 0.33g | Carbs: 22.84g | Protein: 1.09g"
                ParseNutritionFromDescription(food, food.Description);

                return food;
            }
            catch
            {
                return null;
            }
        }

        private void ParseNutritionFromDescription(FatSecretFood food, string description)
        {
            if (string.IsNullOrEmpty(description))
                return;

            try
            {
                // FatSecret description format varies by locale:
                // EN: "Per 100g - Calories: 89kcal | Fat: 0.33g | Carbs: 22.84g | Protein: 1.09g"
                // IT: "Per 100g - Calorie: 89kcal | Grassi: 0,33g | Carboidrati: 22,84g | Proteine: 1,09g"
                // DE: "Per 100g - Kalorien: 89kcal | Fett: 0,33g | Kohlenhydrate: 22,84g | Protein: 1,09g"
                // FR: "Per 100g - Calories: 89kcal | Lipides: 0,33g | Glucides: 22,84g | Protéines: 1,09g"
                var parts = description.Split('|');
                foreach (var part in parts)
                {
                    var trimmed = part.Trim();

                    if (MatchesAnyLabel(trimmed, out string label,
                        "Calories:", "Calorie:", "Kalorien:", "Calorías:"))
                    {
                        food.Calories = ExtractNumber(trimmed, label);
                    }
                    else if (MatchesAnyLabel(trimmed, out label,
                        "Fat:", "Grassi:", "Fett:", "Lipides:", "Grasas:"))
                    {
                        food.TotalFatsPercent = ExtractNumber(trimmed, label);
                    }
                    else if (MatchesAnyLabel(trimmed, out label,
                        "Carbs:", "Carboidrati:", "Kohlenhydrate:", "Glucides:", "Carbohidratos:"))
                    {
                        food.CarbohydratesPercent = ExtractNumber(trimmed, label);
                    }
                    else if (MatchesAnyLabel(trimmed, out label,
                        "Protein:", "Proteine:", "Protein:", "Protéines:", "Proteínas:"))
                    {
                        food.ProteinsPercent = ExtractNumber(trimmed, label);
                    }
                    else if (MatchesAnyLabel(trimmed, out label,
                        "Fiber:", "Fibre:", "Ballaststoffe:", "Fibres:", "Fibra:"))
                    {
                        food.FibersPercent = ExtractNumber(trimmed, label);
                    }
                    else if (MatchesAnyLabel(trimmed, out label,
                        "Sugar:", "Zuccheri:", "Zucker:", "Sucres:", "Azúcares:"))
                    {
                        food.SugarPercent = ExtractNumber(trimmed, label);
                    }
                    else if (MatchesAnyLabel(trimmed, out label,
                        "Sodium:", "Sodio:", "Natrium:", "Sodium:", "Sodio:"))
                    {
                        food.SodiumPercent = ExtractNumber(trimmed, label);
                    }
                }
            }
            catch
            {
                // Ignore parsing errors
            }
        }

        /// <summary>
        /// Checks if the text contains any of the given labels and returns the matched one.
        /// </summary>
        private static bool MatchesAnyLabel(string text, out string matchedLabel, params string[] labels)
        {
            foreach (var label in labels)
            {
                if (text.Contains(label))
                {
                    matchedLabel = label;
                    return true;
                }
            }
            matchedLabel = null;
            return false;
        }

        private double? ExtractNumber(string text, string prefix)
        {
            try
            {
                var startIndex = text.IndexOf(prefix) + prefix.Length;
                var valueStr = text.Substring(startIndex).Trim();
                
                // Remove units (g, kcal, mg, etc.)
                var numericPart = new StringBuilder();
                foreach (var c in valueStr)
                {
                    if (char.IsDigit(c) || c == '.' || c == ',')
                        numericPart.Append(c);
                    else if (numericPart.Length > 0)
                        break;
                }

                if (numericPart.Length > 0)
                {
                    var normalized = numericPart.ToString().Replace(',', '.');
                    if (double.TryParse(normalized, System.Globalization.NumberStyles.Any, 
                        System.Globalization.CultureInfo.InvariantCulture, out var result))
                    {
                        return result;
                    }
                }
            }
            catch
            {
                // Ignore
            }
            return null;
        }

        private FatSecretFood ParseFoodDetails(string json)
        {
            try
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (!root.TryGetProperty("food", out var foodElement))
                    return null;

                var food = new FatSecretFood
                {
                    FoodId = foodElement.TryGetProperty("food_id", out var idProp) ? 
                        long.Parse(idProp.GetString() ?? "0") : 0,
                    Name = foodElement.TryGetProperty("food_name", out var nameProp) ? 
                        nameProp.GetString() : "",
                    BrandName = foodElement.TryGetProperty("brand_name", out var brandProp) ? 
                        brandProp.GetString() : "",
                    FoodType = foodElement.TryGetProperty("food_type", out var typeProp) ? 
                        typeProp.GetString() : "",
                    FoodUrl = foodElement.TryGetProperty("food_url", out var urlProp) ? 
                        urlProp.GetString() : "",
                    Category = ParseFoodSubCategory(foodElement)
                };

                // Parse servings for nutritional data
                if (foodElement.TryGetProperty("servings", out var servingsElement) &&
                    servingsElement.TryGetProperty("serving", out var servingArray))
                {
                    JsonElement serving;
                    if (servingArray.ValueKind == JsonValueKind.Array)
                    {
                        // Find 100g serving or use first
                        serving = servingArray[0];
                        foreach (var s in servingArray.EnumerateArray())
                        {
                            if (s.TryGetProperty("serving_description", out var descProp) &&
                                descProp.GetString()?.Contains("100") == true)
                            {
                                serving = s;
                                break;
                            }
                        }
                    }
                    else
                    {
                        serving = servingArray;
                    }

                    ParseServingNutrition(food, serving);
                }

                return food;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("FatSecretService | ParseFoodDetails", ex);
                return null;
            }
        }

        /// <summary>
        /// Extracts the first food sub-category string from the food element.
        /// FatSecret returns: "food_sub_categories": { "food_sub_category": "Dairy" }
        /// or an array: { "food_sub_category": ["Dairy","Cheese"] }
        /// </summary>
        private static string ParseFoodSubCategory(JsonElement foodElement)
        {
            try
            {
                if (!foodElement.TryGetProperty("food_sub_categories", out var subCats))
                    return null;
                if (!subCats.TryGetProperty("food_sub_category", out var subCat))
                    return null;

                if (subCat.ValueKind == JsonValueKind.String)
                    return subCat.GetString();

                if (subCat.ValueKind == JsonValueKind.Array)
                {
                    var first = subCat.EnumerateArray().FirstOrDefault();
                    return first.ValueKind == JsonValueKind.String ? first.GetString() : null;
                }
            }
            catch { }
            return null;
        }

        private void ParseServingNutrition(FatSecretFood food, JsonElement serving)
        {
            food.Calories = GetDoubleProperty(serving, "calories");
            food.CarbohydratesPercent = GetDoubleProperty(serving, "carbohydrate");
            food.ProteinsPercent = GetDoubleProperty(serving, "protein");
            food.TotalFatsPercent = GetDoubleProperty(serving, "fat");
            food.SaturatedFatsPercent = GetDoubleProperty(serving, "saturated_fat");
            food.FibersPercent = GetDoubleProperty(serving, "fiber");
            food.SugarPercent = GetDoubleProperty(serving, "sugar");
            food.SodiumPercent = GetDoubleProperty(serving, "sodium");

            if (serving.TryGetProperty("serving_description", out var descProp))
                food.ServingDescription = descProp.GetString();
            
            if (serving.TryGetProperty("metric_serving_amount", out var amountProp) &&
                double.TryParse(amountProp.GetString(), out var amount))
                food.ServingSize = amount;
            
            if (serving.TryGetProperty("metric_serving_unit", out var unitProp))
                food.ServingUnit = unitProp.GetString();
        }

        private double? GetDoubleProperty(JsonElement element, string propertyName)
        {
            if (element.TryGetProperty(propertyName, out var prop))
            {
                var str = prop.GetString();
                if (!string.IsNullOrEmpty(str) && 
                    double.TryParse(str, System.Globalization.NumberStyles.Any,
                        System.Globalization.CultureInfo.InvariantCulture, out var result))
                {
                    return result;
                }
            }
            return null;
        }
    }
}
