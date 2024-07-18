
using Attendiia.Authentication;

namespace Attendiia.DelegatingHandlers;

public sealed class _401MessageDelegatingHandler : DelegatingHandler
{
    private readonly FirebaseAuthenticationStateProvider _firebaseAuthenticationStateProvider;
    public _401MessageDelegatingHandler(FirebaseAuthenticationStateProvider firebaseAuthenticationStateProvider)
    {
        _firebaseAuthenticationStateProvider = firebaseAuthenticationStateProvider;
    }
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        HttpResponseMessage httpResponseMessage = await base.SendAsync(request, cancellationToken);
        if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            await _firebaseAuthenticationStateProvider.NotifySignOut();
        }
        return httpResponseMessage;
    }
}
