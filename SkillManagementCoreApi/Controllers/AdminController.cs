using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillManagementCoreApi.Data;
using static SkillMngmntClassLibrary.DTOs.Models;

namespace SkillManagementCoreApi.Controllers
{

    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public AdminController(ApplicationDbContext context) => _context = context;

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var data = await _context.UserSkills
                .Include(us => us.User)
                .Include(us => us.Skill)
                .Select(us => new
                {
                    Name = us.User.Name,
                    us.Skill.Id,
                    UserEmail = us.User.Email,
                    SkillName = us.Skill.Name,
                    us.Skill.ProficiencyLevel
                })
                .ToListAsync();

            return Ok(data);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateSkill(int id, [FromBody] SkillDto dto)
        {
            var skill = await _context.Skills.FindAsync(id);
            if (skill == null)
                return NotFound("Skill not found");

            skill.Name = dto.Name;
            skill.ProficiencyLevel = dto.ProficiencyLevel;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Skill updated successfully" });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteSkill(int id)
        {
            var skill = await _context.Skills.FindAsync(id);
            if (skill == null)
                return NotFound("Skill not found");

            _context.Skills.Remove(skill);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Skill deleted successfully" });
        }
    
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboardStats()
        {
            var totalUsers = await _context.Users.CountAsync();
            var totalSkills = await _context.Skills.CountAsync();
            var totalUserSkills = await _context.UserSkills.CountAsync();

            var stats = new DashboardStatsDto
            {
                TotalUsers = totalUsers,
                TotalSkills = totalSkills,
                TotalUserSkills = totalUserSkills
            };

            return Ok(stats);
        }
    }
}



