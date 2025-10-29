using SkillMngmntBlazorApp.Services;
using SkillMngmntBlazorApp.Services.SkillMngmntBlazorApp.Services;
using System.Net.Http.Json;
using System.Text.Json;
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

            if (response.IsSuccessStatusCode)
                return "Success";

            var content = await response.Content.ReadAsStringAsync();

            try
            {
                using var json = JsonDocument.Parse(content);
                var root = json.RootElement;

                // ✅ Case 1: top-level array (e.g. [ { "description": "..." } ])
                if (root.ValueKind == JsonValueKind.Array)
                {
                    var errors = root
                        .EnumerateArray()
                        .Select(e => e.TryGetProperty("description", out var desc)
                            ? desc.GetString()
                            : e.ToString())
                        .Where(msg => !string.IsNullOrEmpty(msg));

                    return string.Join("\n", errors);
                }

                // ✅ Case 2: object with an "errors" array
                if (root.ValueKind == JsonValueKind.Object &&
                    root.TryGetProperty("errors", out var errorsProp) &&
                    errorsProp.ValueKind == JsonValueKind.Array)
                {
                    var errors = errorsProp
                        .EnumerateArray()
                        .Select(e => e.TryGetProperty("description", out var desc)
                            ? desc.GetString()
                            : e.ToString())
                        .Where(msg => !string.IsNullOrEmpty(msg));

                    return string.Join("\n", errors);
                }

                // ✅ Case 3: object with a simple "message"
                if (root.ValueKind == JsonValueKind.Object &&
                    root.TryGetProperty("message", out var messageProp))
                {
                    return messageProp.GetString();
                }

                // ✅ Case 4: plain text or unknown JSON
                return root.ToString();
            }
            catch (Exception ex)
            {
                // Fallback for invalid or non-JSON responses
                return $"Registration failed. {ex.Message}";
            }
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


