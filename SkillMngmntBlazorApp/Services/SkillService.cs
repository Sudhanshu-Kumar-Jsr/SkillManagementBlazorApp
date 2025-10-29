using static SkillMngmntClassLibrary.DTOs.Models;
using System.Net.Http.Json;

namespace SkillMngmntBlazorApp.Services
{
    public class SkillService 
    { 
        private readonly HttpClient _httpClient;
        public SkillService(HttpClient httpClient) => _httpClient = httpClient;
        public async Task<bool> AddSkill(SkillDto dto) 
        {
            var response = await _httpClient.PostAsJsonAsync("api/skill/add", dto);
            return response.IsSuccessStatusCode; 
        } 
        public async Task<List<SkillDto>> GetMySkills()
        { 
            var response = await _httpClient.GetFromJsonAsync<List<SkillDto>>("api/skill/myskills");
            return response ?? new List<SkillDto>(); 
        }

        public async Task<bool> UpdateSkill(SkillDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/skill/update/{dto.Id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteSkill(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/skill/delete/{id}");
            return response.IsSuccessStatusCode;
        }


    }
}


