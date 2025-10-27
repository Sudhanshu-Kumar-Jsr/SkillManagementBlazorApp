using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillMngmntClassLibrary.DTOs
{
    public class Models
    {
        public class RegisterDto
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        public class LoginDto
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        public class LoginResponse
        {
            public string Token { get; set; } = string.Empty;
            public DateTime Expiration { get; set; }
        }

        public class SkillDto
        {
            public string Name { get; set; } = string.Empty;
            public string ProficiencyLevel { get; set; } = string.Empty;
        }

        public class UserSkillView
        {
            public string UserEmail { get; set; } = string.Empty;
            public string SkillName { get; set; } = string.Empty;
            public string ProficiencyLevel { get; set; } = string.Empty;
        }

    }
}
