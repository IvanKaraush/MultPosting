using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Util.Store;

namespace MultiPosting.Application;

public class CustomAuthorizationBroker : GoogleWebAuthorizationBroker
{
    public static string RedirectUri { get; set; } // Set this to your desired URI

    public new static async Task<UserCredential> AuthorizeAsync(
        ClientSecrets clientSecrets,
        IEnumerable<string> scopes,
        string user,
        CancellationToken taskCancellationToken,
        IDataStore dataStore = null)
    {
        var initializer = new GoogleAuthorizationCodeFlow.Initializer
        {
            ClientSecrets = clientSecrets,
        };
        return await AuthorizeAsyncCore(initializer, scopes, user, taskCancellationToken, dataStore).ConfigureAwait(false);
    }

    private static async Task<UserCredential> AuthorizeAsyncCore(
        GoogleAuthorizationCodeFlow.Initializer initializer,
        IEnumerable<string> scopes,
        string user,
        CancellationToken taskCancellationToken,
        IDataStore dataStore)
    {
        initializer.Scopes = scopes;
        initializer.DataStore = dataStore ?? new FileDataStore(Folder);
        var flow = new CustomAuthorizationCodeFlow(initializer);
        return await new AuthorizationCodeInstalledApp(flow, new LocalServerCodeReceiver())
            .AuthorizeAsync(user, taskCancellationToken).ConfigureAwait(false);
    }
}

public class CustomAuthorizationCodeFlow : GoogleAuthorizationCodeFlow
{
    public CustomAuthorizationCodeFlow(Initializer initializer) : base(initializer) { }

    public override AuthorizationCodeRequestUrl CreateAuthorizationCodeRequest(string redirectUri)
    {
        // Use the custom redirect URI instead of the dynamic one
        return base.CreateAuthorizationCodeRequest(CustomAuthorizationBroker.RedirectUri);
    }
}