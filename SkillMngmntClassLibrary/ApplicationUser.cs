using Microsoft.AspNetCore.Identity;
using SkillMngmntClassLibrary;

namespace SkillManagementCoreApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public ICollection<UserSkill>? UserSkills { get; set; }
    }
}

