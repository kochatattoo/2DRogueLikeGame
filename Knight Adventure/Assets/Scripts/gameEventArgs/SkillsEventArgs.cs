using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.gameEventArgs
{
    public class SkillsEventArgs: EventArgs
    {
        public int Damage { get; set; }
        public float CurrentHealth { get; set; }
        public bool IsDead { get; set; }
        public bool CanUseSkills { get; set; }
    }
}
