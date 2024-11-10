using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class User
    {
        private string name;
        private int level;
        private int coins;

        public User()
        {
            name = "";
            level = 1;
            coins = 0;
        }
        public User(string name, int level, int coins)
        {
            this.name = name;
            this.level = level;
            this.coins = coins;
        }

        public void SetName(string Name)
        {
           name = Name;
        }
        public void SetLevel(int Level)
        {
           level = Level;
        }
        public void SetCoins(int Coins)
        {
           coins = Coins;
        }
        public int GetLevel()
        {
            return level;
        }
        public int GetCoins()
        {
            return coins;
        }
        public string GetName()
        {
            return name;
        }
    }
}
