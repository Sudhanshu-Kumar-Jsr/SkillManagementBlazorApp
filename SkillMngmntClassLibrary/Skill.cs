using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillMngmntClassLibrary
{
    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ProficiencyLevel { get; set; } = string.Empty;
        public ICollection<UserSkill>? UserSkills { get; set; }
    }
}
