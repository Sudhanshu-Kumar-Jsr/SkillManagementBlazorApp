using SkillMngmntBlazorApp.Services;
using SkillMngmntBlazorApp.Services.SkillMngmntBlazorApp.Services;
using System.Net.Http.Json;
using static SkillMngmntClassLibrary.DTOs.Models;

namespace SkillMngmntBlazorApp.Services
{
    public class AuthService {
        private readonly HttpClient _httpClient; 
        private readonly JwtAuthStateProvider _authStateProvider;
        public AuthService(HttpClient httpClient, JwtAuthStateProvider authStateProvider)
        { 
            _httpClient = httpClient;
            _authStateProvider = authStateProvider;
        } 
        public async Task<string?> Register(RegisterDto dto)
        { 
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", dto); 
            return response.IsSuccessStatusCode ? "Success" : "Failed"; 
        } 
        public async Task<bool> Login(LoginDto dto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/login", dto);
                if (!response.IsSuccessStatusCode)
                    return false; var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if (result == null) return false;
                await _authStateProvider.MarkUserAsAuthenticated(result.Token);
                return true;
            }
            catch (Exception ex) {
                return false;
            }          
        } 
        public async Task Logout() => await _authStateProvider.MarkUserAsLoggedOut(); }


}


