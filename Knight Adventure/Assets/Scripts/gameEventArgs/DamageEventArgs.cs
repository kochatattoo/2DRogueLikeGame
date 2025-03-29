using System;

namespace Assets.Scripts.gameEventArgs
{
    public class DamageEventArgs: EventArgs
    {
        public int Damage { get; set; }
        public float CurrentHealth { get; set; }
    }
}
