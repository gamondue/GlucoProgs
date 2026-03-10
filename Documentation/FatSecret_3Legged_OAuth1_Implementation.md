# FatSecret 3-Legged OAuth 1.0 Authentication - Implementazione

## Domanda

L'accesso a FatSecret attuale e complicato per il desktop e praticamente impossibile per lo smartphone. Fatsecret propone per gli smartphone, ed io la implementerei anche per il Desktop, la 3-Legged Authentication, il cui uso e spiegato in questa pagina:
https://platform.fatsecret.com/docs/guides/authentication/oauth1/three-legged.
Riusciamo a implementarla?

## Risposta

Si, l'implementazione e stata completata con successo. Ecco il riepilogo di cosa e stato fatto:

### Riepilogo delle modifiche

#### 1. FatSecretService.cs - OAuth 1.0 3-Legged Authentication

Aggiunto al service esistente (`SharedGlucoMan/Devices/FatSecretService.cs`):

- **Endpoint OAuth 1.0**: `request_token`, `authorize`, `access_token`
- **`OAuth1_GetRequestTokenAsync()`** - Step 1: ottiene il request token
- **`OAuth1_GetAuthorizationUrl()`** - Step 2: genera l'URL per l'autorizzazione utente
- **`OAuth1_ExchangeRequestTokenAsync(verifier)`** - Step 3: scambia il request token + verifier per l'access token
- **`MakeOAuth1ApiCallAsync()`** - chiama le API firmando con il token utente
- **`ClearUserToken()`** - logout (cancella i token salvati)
- **Helper**: `GenerateOAuth1Signature()` (HMAC-SHA1), `PercentEncode()`, `BuildOAuthHeaderString()`
- **Persistenza**: i token utente vengono salvati/ripristinati dal database via `BL_General`
- **Fallback**: i metodi `SearchFoodsAsync`, `GetFoodDetailsAsync`, `FindFoodByBarcodeAsync` usano OAuth 1.0 se il token utente e disponibile, altrimenti cadono sul vecchio OAuth 2.0

#### 2. FatSecretAuthPage.xaml + .cs - Nuova pagina

File: `GlucoMan.Maui/FatSecretAuthPage.xaml` e `GlucoMan.Maui/FatSecretAuthPage.xaml.cs`

- **WebView** che carica la pagina di autorizzazione FatSecret
- Intercetta il redirect alla callback URL (`https://localhost/fatsecret_callback`)
- Estrae automaticamente l'`oauth_verifier` e completa lo scambio token
- Pattern `PageClosedTask` coerente con le altre pagine del progetto

#### 3. FatSecretSearchPage.xaml + .cs - Aggiornata

File: `GlucoMan.Maui/FatSecretSearchPage.xaml` e `GlucoMan.Maui/FatSecretSearchPage.xaml.cs`

- Aggiunto pulsante **"Login with FatSecret"** / **"Logout"**
- Indicatore di stato autenticazione (verde = logged in, grigio = non configurato)
- Il pulsante si adatta: login apre la pagina WebView, logout cancella il token

### Flusso utente

1. L'utente inserisce Client ID e Client Secret (una volta sola, come prima)
2. Clicca **"Login with FatSecret"** -> si apre il WebView
3. Fa login col suo account FatSecret e autorizza l'app
4. Il token viene salvato permanentemente -> le ricerche usano OAuth 1.0
5. Funziona sia su desktop che su smartphone

### Contesto tecnico

- **Situazione precedente (OAuth 2.0 Client Credentials)**: ogni utente dell'app doveva registrarsi su FatSecret e inserire le proprie credenziali API. Complicato per desktop, praticamente impossibile per smartphone.
- **Nuova situazione (3-Legged OAuth 1.0)**: il developer ha un consumer key/secret, e gli utenti si autenticano semplicemente con il browser, facendo login su FatSecret. Molto piu semplice.

### Dettagli del flusso OAuth 1.0 a 3 gambe

Il flusso segue la specifica "OAuth Core 1.0 Revision A":

1. **Step 1 - Request Token**: L'app chiama `https://authentication.fatsecret.com/oauth/request_token` con firma HMAC-SHA1 per ottenere un token temporaneo.
2. **Step 2 - User Authorization**: L'utente viene reindirizzato (nel WebView) a `https://authentication.fatsecret.com/oauth/authorize` dove fa login e autorizza l'app.
3. **Step 3 - Access Token**: Dopo l'autorizzazione, FatSecret redirige alla callback URL con un `oauth_verifier`. L'app lo scambia per un access token permanente chiamando `https://authentication.fatsecret.com/oauth/access_token`.

Il token ottenuto viene salvato nel database SQLite dell'app e riutilizzato per tutte le chiamate API successive, firmando ogni richiesta con HMAC-SHA1.
