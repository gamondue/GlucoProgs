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
    /// An error reported by FatSecret in the body of a response with HTTP status 200.
    /// </summary>
    public class FatSecretApiException : InvalidOperationException
    {
        public FatSecretApiException(string message) : base(message) { }
    }

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
        // Scopes granted to the current token: a key without "premier" uses the legacy search
        private static bool _tokenHasPremierScope;
        // Set after foods.search.v5 has been refused, to avoid asking again at every search
        private static bool _searchV5Refused;

        public const int MaxResultsPerPage = 50;

        // Tried in order: the application gets the first set enabled for its FatSecret plan
        private static readonly string[] SearchScopes =
        {
            "basic premier localization", "basic premier", "basic localization", "basic"
        };
        private static readonly string[] BarcodeScopes = { "basic barcode localization", "basic barcode" };

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

        // Locale of the phone, sent with every request. No region means FatSecret's default (US)
        private readonly (string Language, string Region) _locale = ParseLocale(GetDeviceCultureName());

        /// <summary>True when a user has completed 3-Legged OAuth 1.0 authorization.</summary>
        public bool HasUserToken => !string.IsNullOrEmpty(_oauth1AccessToken)
                                 && !string.IsNullOrEmpty(_oauth1AccessTokenSecret);

        public FatSecretService()
        {
            LoadCredentials();
        }

        private void LoadCredentials()
        {
            _clientId = "";
            _clientSecret = "";
            _consumerSecret = "";
            _oauth1AccessToken = "";
            _oauth1AccessTokenSecret = "";
#if !MATH_TESTS_ONLY
            try
            {
                var bl = new BL_General();
                _clientId = bl.RestoreParameter("FatSecret_ClientId") ?? "";
                _clientSecret = bl.RestoreParameter("FatSecret_ClientSecret") ?? "";
                _consumerSecret = bl.RestoreParameter("FatSecret_ConsumerSecret") ?? "";
                // Load persisted OAuth 1.0 user tokens
                _oauth1AccessToken = bl.RestoreParameter("FatSecret_OAuth1Token") ?? "";
                _oauth1AccessTokenSecret = bl.RestoreParameter("FatSecret_OAuth1TokenSecret") ?? "";
            }
            catch { }
#endif
        }

        public bool HasCredentials => !string.IsNullOrEmpty(_clientId) && !string.IsNullOrEmpty(_clientSecret);

        /// <summary>True when the OAuth 1.0 Shared Secret is configured (needed for 3-Legged auth).</summary>
        public bool HasConsumerSecret => !string.IsNullOrEmpty(_consumerSecret);

        public void SetCredentials(string clientId, string clientSecret)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
#if !MATH_TESTS_ONLY
            var bl = new BL_General();
            bl.SaveParameter("FatSecret_ClientId", clientId);
            bl.SaveParameter("FatSecret_ClientSecret", clientSecret);
#endif
            // Tokens and the refused search belong to the previous key
            _accessToken = null;
            _barcodeAccessToken = null;
            _tokenHasPremierScope = false;
            _searchV5Refused = false;
        }

        public void SetConsumerSecret(string consumerSecret)
        {
            _consumerSecret = consumerSecret;
#if !MATH_TESTS_ONLY
            var bl = new BL_General();
            bl.SaveParameter("FatSecret_ConsumerSecret", consumerSecret);
#endif
        }

        /// <summary>
        /// Name of the culture of the phone. CultureInfo.CurrentCulture can't be used, because
        /// LocalizationService replaces it with the language of the application.
        /// </summary>
        private static string GetDeviceCultureName()
        {
#if ANDROID
            return Java.Util.Locale.Default.ToLanguageTag();
#else
            return System.Globalization.CultureInfo.InstalledUICulture.Name;
#endif
        }

        /// <summary>
        /// Splits a culture name like "it-IT", "en_US" or "zh-Hant-TW" in language and region.
        /// </summary>
        internal static (string Language, string Region) ParseLocale(string cultureName)
        {
            var parts = (cultureName ?? "").Split('-', '_');
            var region = parts.Skip(1).FirstOrDefault(p => p.Length == 2);
            return (parts[0].ToLowerInvariant(), region?.ToUpperInvariant());
        }

        /// <summary>
        /// Adds language and region. FatSecret ignores the language when there is no region.
        /// </summary>
        private void AddLocale(Dictionary<string, string> apiParams)
        {
            if (string.IsNullOrEmpty(_locale.Region))
                return;
            apiParams["language"] = _locale.Language;
            apiParams["region"] = _locale.Region;
        }

        /// <summary>Removes the stored OAuth 1.0 user tokens (logout).</summary>
        public void ClearUserToken()
        {
            _oauth1AccessToken = "";
            _oauth1AccessTokenSecret = "";
#if !MATH_TESTS_ONLY
            var bl = new BL_General();
            bl.SaveParameter("FatSecret_OAuth1Token", "");
            bl.SaveParameter("FatSecret_OAuth1TokenSecret", "");
#endif
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
#if !MATH_TESTS_ONLY
            var bl = new BL_General();
            bl.SaveParameter("FatSecret_OAuth1Token", _oauth1AccessToken);
            bl.SaveParameter("FatSecret_OAuth1TokenSecret", _oauth1AccessTokenSecret);
#endif
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

            var (token, expiry, scope) = await RequestTokenAsync(SearchScopes)
                ?? throw new InvalidOperationException("FatSecret refused the 'basic' scope for this application.");

            _accessToken = token;
            _tokenExpiry = expiry;
            _tokenHasPremierScope = scope.Contains("premier");

            return _accessToken;
        }

        /// <summary>
        /// Get OAuth 2.0 access token with the 'barcode' scope.
        /// Requires the barcode scope to be enabled on the FatSecret application
        /// at platform.fatsecret.com → My App → Barcode.
        /// </summary>
        private async Task<string> GetBarcodeAccessTokenAsync()
        {
            if (!string.IsNullOrEmpty(_barcodeAccessToken) && DateTime.UtcNow < _barcodeTokenExpiry)
                return _barcodeAccessToken;

            var (token, expiry, _) = await RequestTokenAsync(BarcodeScopes)
                ?? throw new InvalidOperationException(
                    "The 'barcode' scope is not enabled for your FatSecret application. " +
                    "Enable it at platform.fatsecret.com → My App → Edit → Barcode.");

            _barcodeAccessToken = token;
            _barcodeTokenExpiry = expiry;

            return _barcodeAccessToken;
        }

        /// <summary>
        /// Requests a token with the first of the scope sets that FatSecret accepts.
        /// Returns null when all of them are refused.
        /// </summary>
        private async Task<(string Token, DateTime Expiry, string Scope)?> RequestTokenAsync(string[] scopeSets)
        {
            if (!HasCredentials)
                throw new InvalidOperationException("FatSecret API credentials not configured. Please set Client ID and Client Secret in settings.");

            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_clientId}:{_clientSecret}"));

            foreach (var scope in scopeSets)
            {
                var request = new HttpRequestMessage(HttpMethod.Post, TokenEndpoint);
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                request.Content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials"),
                    new KeyValuePair<string, string>("scope", scope)
                });

                var response = await _httpClient.SendAsync(request);
                var responseJson = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    if (responseJson.Contains("invalid_scope"))
                        continue;
                    throw new HttpRequestException($"Failed to get access token: {response.StatusCode} - {responseJson}");
                }

                using var doc = JsonDocument.Parse(responseJson);
                var root = doc.RootElement;
                var expiresIn = root.GetProperty("expires_in").GetInt32();
                // Refresh 1 minute early
                return (root.GetProperty("access_token").GetString(), DateTime.UtcNow.AddSeconds(expiresIn - 60), scope);
            }

            return null;
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
                { "format", "json" }
            };
            AddLocale(apiParams);

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
        /// Search for foods in the FatSecret database. Returns one page of results:
        /// the first page has number 0.
        /// Uses foods.search.v5 where the plan of the key allows it, otherwise foods.search.
        /// </summary>
        public async Task<FatSecretSearchResults> SearchFoodsAsync(string searchExpression,
            int pageNumber = 0, int pageSize = MaxResultsPerPage)
        {
            var apiParams = new Dictionary<string, string>
            {
                { "search_expression", searchExpression },
                { "format", "json" },
                { "max_results", pageSize.ToString() },
                { "page_number", pageNumber.ToString() }
            };
            AddLocale(apiParams);

            // The token must be read first: it tells if the key has the "premier" scope
            var token = HasUserToken ? null : await GetAccessTokenAsync();

            string responseJson = null;
            if (!_searchV5Refused && (HasUserToken || _tokenHasPremierScope))
            {
                try
                {
                    responseJson = await CallSearchAsync("foods.search.v5", apiParams, token);
                }
                catch (Exception ex) when (ex is FatSecretApiException or HttpRequestException)
                {
                    // FatSecret doesn't document the error for a method outside of the plan.
                    // A real failure will come out again from the legacy search.
                    General.LogOfProgram?.Debug($"FatSecret foods.search.v5 refused: {ex.Message}");
                    _searchV5Refused = true;
                }
            }
            responseJson ??= await CallSearchAsync("foods.search", apiParams, token);

            return ParseSearchPage(responseJson, pageNumber, pageSize);
        }

        private async Task<string> CallSearchAsync(string method, Dictionary<string, string> searchParams, string token)
        {
            var apiParams = new Dictionary<string, string>(searchParams) { ["method"] = method };

            string responseJson;
            if (HasUserToken)
            {
                responseJson = await MakeOAuth1ApiCallAsync(apiParams);
            }
            else
            {
                var request = new HttpRequestMessage(HttpMethod.Post, ApiBaseUrl);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                request.Content = new FormUrlEncodedContent(apiParams);
                var response = await _httpClient.SendAsync(request);
                responseJson = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"Food search failed: {response.StatusCode} - {responseJson}");
            }

            General.LogOfProgram?.Debug($"FatSecret {method}: {responseJson}");

            // FatSecret returns HTTP 200 even for API-level errors; check the JSON body
            CheckForApiError(responseJson);

            return responseJson;
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
                { "format", "json" }
            };
            AddLocale(apiParams);

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

        /// <summary>
        /// Reads a page of results of foods.search ("foods") or foods.search.v5 ("foods_search").
        /// </summary>
        internal FatSecretSearchResults ParseSearchPage(string json, int pageNumber, int pageSize)
        {
            var page = new FatSecretSearchResults { PageNumber = pageNumber, PageSize = pageSize };

            try
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                JsonElement container;
                JsonElement foodsHolder;
                if (root.TryGetProperty("foods_search", out container))
                {
                    if (!container.TryGetProperty("results", out foodsHolder))
                        foodsHolder = container;
                }
                else if (root.TryGetProperty("foods", out container))
                {
                    foodsHolder = container;
                }
                else
                {
                    General.LogOfProgram?.Debug("FatSecret ParseSearchPage: no results in response");
                    return page;
                }

                if (container.TryGetProperty("total_results", out var totalProp)
                    && int.TryParse(totalProp.ToString(), out var total))
                    page.TotalResults = total;

                if (!foodsHolder.TryGetProperty("food", out var foodArray))
                {
                    General.LogOfProgram?.Debug("FatSecret ParseSearchPage: 'food' property absent — API returned 0 results");
                    return page;
                }

                // Handle both single food object and array
                var elements = foodArray.ValueKind == JsonValueKind.Array
                    ? foodArray.EnumerateArray().ToList()
                    : new List<JsonElement> { foodArray };
                page.Foods.AddRange(elements.Select(ParseFoodFromElement).Where(food => food != null));
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("FatSecretService | ParseSearchPage", ex);
            }

            return page;
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
                        21 => "\nFix: add this IP address at platform.fatsecret.com/my-account/ip-restrictions (it can take up to 24 hours).",
                        10 => "\nFix: check your Client ID and Client Secret in Settings.",
                        8  => "\nFix: the foods.search method is not available on your API plan.",
                        _  => string.Empty
                    };

                    throw new FatSecretApiException($"FatSecret API error {code}: {message}{hint}");
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

                // foods.search.v5 has no description, but the list of servings
                if (string.IsNullOrEmpty(food.Description)
                    && TryGetReferenceServing(element, out var serving))
                {
                    ParseServingNutrition(food, serving);
                    food.Description = FormattableString.Invariant(
                        $"Per {food.ServingDescription} - Calories: {food.Calories}kcal | Fat: {food.TotalFatsPercent}g | Carbs: {food.CarbohydratesPercent}g | Protein: {food.ProteinsPercent}g");
                }

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
                if (TryGetReferenceServing(foodElement, out var serving))
                    ParseServingNutrition(food, serving);

                return food;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("FatSecretService | ParseFoodDetails", ex);
                return null;
            }
        }

        /// <summary>
        /// Finds the 100g serving of a food, or the first one if it has no such serving.
        /// </summary>
        private static bool TryGetReferenceServing(JsonElement foodElement, out JsonElement serving)
        {
            serving = default;
            if (!foodElement.TryGetProperty("servings", out var servingsElement) ||
                !servingsElement.TryGetProperty("serving", out var servingArray))
                return false;

            if (servingArray.ValueKind != JsonValueKind.Array)
            {
                serving = servingArray;
                return true;
            }
            if (servingArray.GetArrayLength() == 0)
                return false;

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
            return true;
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
