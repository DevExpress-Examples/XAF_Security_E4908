using System.Security.Claims;
using Blazor.WebAssembly.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace Blazor.WebAssembly.Services;
public class WebAPIAuthenticationStateProvider : AuthenticationStateProvider {
    private ClaimsPrincipal _claimsPrincipal = new(new ClaimsIdentity());
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
        => Task.FromResult<AuthenticationState>(new(_claimsPrincipal));

    public void ClearAuthInfo() {
        _claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public void SetAuthInfo(UserModel user) {
        _claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]{
            new Claim(nameof(UserModel.Email), user.Email ?? ""),
            new Claim(nameof(UserModel.XafUserId), user.XafUserId.ToString()),
            new Claim(nameof(UserModel.LoginProviderUserId), user.LoginProviderUserId.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim("ID", user.ToString()!) }, "AuthCookie"));
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}