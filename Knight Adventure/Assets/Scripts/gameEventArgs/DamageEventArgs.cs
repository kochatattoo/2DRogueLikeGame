using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.gameEventArgs
{
    public class DamageEventArgs: EventArgs
    {
        public int Damage { get; set; }
        public float CurrentHealth { get; set; }
    }
}
