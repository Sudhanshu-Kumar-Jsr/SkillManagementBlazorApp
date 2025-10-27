using SkillManagementCoreApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillMngmntClassLibrary
{
    public class UserSkill
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int SkillId { get; set; }
        public Skill Skill { get; set; }
    }
}

