using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SkillMngmntBlazorApp.Services
{
    using Blazored.LocalStorage;
    using Microsoft.AspNetCore.Components.Authorization;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;

    namespace SkillMngmntBlazorApp.Services
    {
        public class JwtAuthStateProvider : AuthenticationStateProvider
        {
            private readonly ILocalStorageService _localStorage;

            public JwtAuthStateProvider(ILocalStorageService localStorage)
            {
                _localStorage = localStorage;
            }

            public override async Task<AuthenticationState> GetAuthenticationStateAsync()
            {
                var token = await _localStorage.GetItemAsync<string>("authToken");
                var identity = new ClaimsIdentity();

                if (!string.IsNullOrWhiteSpace(token))
                {
                    try
                    {
                        var handler = new JwtSecurityTokenHandler();
                        var jwt = handler.ReadJwtToken(token);

                        if (jwt.ValidTo > DateTime.UtcNow)
                        {
                            identity = new ClaimsIdentity(jwt.Claims, "jwt");
                        }
                        else
                        {
                            await _localStorage.RemoveItemAsync("authToken");
                        }
                    }
                    catch
                    {
                        await _localStorage.RemoveItemAsync("authToken");
                    }
                }

                var user = new ClaimsPrincipal(identity);
                return new AuthenticationState(user);
            }

            public async Task MarkUserAsAuthenticated(string token)
            {
                await _localStorage.SetItemAsync("authToken", token);

                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);
                var identity = new ClaimsIdentity(jwt.Claims, "jwt");
                var user = new ClaimsPrincipal(identity);

                // Notify components immediately
                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
            }

            public async Task MarkUserAsLoggedOut()
            {
                await _localStorage.RemoveItemAsync("authToken");
                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal())));
            }
        }
    }

}

