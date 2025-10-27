using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillManagementCoreApi.Data;

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
            var data = await _context.UserSkills.Include(us => us.User).Include(us => us.Skill).Select(us => new
            {
                UserEmail = us.User.Email,
                SkillName = us.Skill.Name,
                us.Skill.ProficiencyLevel
            }).ToListAsync();
            return Ok(data);
        }
    }

}
