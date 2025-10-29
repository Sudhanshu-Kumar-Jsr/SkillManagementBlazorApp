using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillMngmntClassLibrary.DTOs
{
    public class Models
    {
        public class RegisterDto
        {
            [Required(ErrorMessage = "Name is required")]
            public string Name { get; set; } = string.Empty;
            [Required(ErrorMessage = "Email is required")]
            [EmailAddress(ErrorMessage = "Invalid email format")]
            public string Email { get; set; } = string.Empty;           
            [Required(ErrorMessage = "Password is required")]
            [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password must be at least 8 characters long, with at least one uppercase letter, one lowercase letter, one number, and one special character."
    )]
            public string Password { get; set; } = string.Empty;
        }

        public class LoginDto
        {
            [Required(ErrorMessage = "Email or User Name is required")]
            public string Email { get; set; } = string.Empty;
            [Required(ErrorMessage = "Password is required")]
            public string Password { get; set; } = string.Empty;
        }

        public class LoginResponse
        {
            public string Token { get; set; } = string.Empty;
            public DateTime Expiration { get; set; }
        }

        public class SkillDto
        {
            public int? Id { get; set; }
            [Required(ErrorMessage = "Skill Name is required")]
            public string Name { get; set; } = string.Empty;
            [Required(ErrorMessage = "Proficiency Level is required")]
            public string ProficiencyLevel { get; set; } = string.Empty;
        }

        public class UserSkillView
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string UserEmail { get; set; } = string.Empty;
            public string SkillName { get; set; } = string.Empty;
            public string ProficiencyLevel { get; set; } = string.Empty;
        }

        public class DashboardStatsDto
        {
            public int TotalUsers { get; set; }
            public int TotalSkills { get; set; }
            public int TotalUserSkills { get; set; }
        }
        public class IdentityError
        {
            public string Code { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
        }
    }
}
