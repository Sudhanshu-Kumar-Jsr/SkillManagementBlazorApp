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

        public async Task<bool> UpdateSkill(SkillDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/admin/update/{dto.Id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteSkill(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/admin/delete/{id}");
            return response.IsSuccessStatusCode;
        }

        // ✅ New method for dashboard
        public async Task<DashboardStatsDto> GetDashboardStats()
        {
            var response = await _httpClient.GetFromJsonAsync<DashboardStatsDto>("api/admin/dashboard");
            return response ?? new DashboardStatsDto();
        }
    }
}
