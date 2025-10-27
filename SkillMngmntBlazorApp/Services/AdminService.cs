using static SkillMngmntClassLibrary.DTOs.Models;
using System.Net.Http.Json;

namespace SkillMngmntBlazorApp.Services
{
    public class AdminService 
    { 
        private readonly HttpClient _httpClient;
        public AdminService(HttpClient httpClient) => _httpClient = httpClient;
        public async Task<List<UserSkillView>> GetAllUsersWithSkills() 
        { 
            var response = await _httpClient.GetFromJsonAsync<List<UserSkillView>>("api/admin/users");
            return response ?? new List<UserSkillView>();
        }
    }
}
 