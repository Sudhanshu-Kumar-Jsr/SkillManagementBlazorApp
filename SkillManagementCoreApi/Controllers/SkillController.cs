using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillManagementCoreApi.Data;
using SkillManagementCoreApi.Models;
using SkillMngmntClassLibrary;
using System.Security.Claims;
using static SkillMngmntClassLibrary.DTOs.Models;

namespace SkillManagementCoreApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SkillController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SkillController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddSkill([FromBody] SkillDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var skill = await _context.Skills.FirstOrDefaultAsync(s => s.Name == dto.Name);
            if (skill == null)
            {
                skill = new Skill { Name = dto.Name, ProficiencyLevel = dto.ProficiencyLevel };
                _context.Skills.Add(skill);
                await _context.SaveChangesAsync();
            }

            var userSkill = new UserSkill { UserId = userId, SkillId = skill.Id };
            _context.UserSkills.Add(userSkill);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Skill added successfully" });
        }


        [HttpGet("myskills")]
        public async Task<IActionResult> GetMySkills() 
        { 
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var skills = await _context.UserSkills.Include(us => us.Skill).Where(us => us.UserId == userId).Select(us => new
            {
                us.Skill.Id,
                us.Skill.Name,
                us.Skill.ProficiencyLevel
            }).ToListAsync(); 
            return Ok(skills);
        }


        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateSkill(int id, [FromBody] SkillDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var userSkill = await _context.UserSkills
                .Include(us => us.Skill)
                .FirstOrDefaultAsync(us => us.UserId == userId && us.Skill.Id == id);

            if (userSkill == null)
                return NotFound("Skill not found for this user");

            userSkill.Skill.Name = dto.Name;
            userSkill.Skill.ProficiencyLevel = dto.ProficiencyLevel;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Skill updated successfully" });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteSkill(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var userSkill = await _context.UserSkills
                .FirstOrDefaultAsync(us => us.UserId == userId && us.SkillId == id);

            if (userSkill == null)
                return NotFound("Skill not found for this user");

            _context.UserSkills.Remove(userSkill);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Skill deleted successfully" });
        }

    }
}

