
namespace Assets.Scripts
{
    [System.Serializable]
    public class User
    {
        public static User Instance {  get; set; }

        public string name="";
        private int level=1;
        private int coins=10;

        public User()
        { 
                name = "нет игрока";
                level = 1;
                coins = 10;         
        }
        public User(string name, int level, int coins)
        {
            this.name = name;
            this.level = level;
            this.coins = coins;
        }
        public User(User other)
        {
            this.name=other.name;
            this.level = other.level;
            this.coins = other.coins;
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
        public int GetLevel()=> level;
        public int GetCoins()=>coins;
        public string GetName() => name;
      
        public void LoadLastGame()
        {
            User data = SaveManager.Instance.LoadLastGame();

            name = data.name;
            level = data.level;
            coins = data.coins;
        }

        public void LoadUser(string fileName)
        {
            User data = SaveManager.Instance.LoadGame(fileName);
            
            name = data.name;
            level = data.level;
            coins = data.coins;
        }
        //public void ResetData()
        //{
        //    User data = SaveManager.ResetData();

        //    name = data.name;
        //    level = data.level;
        //    coins = data.coins;
        //}
    }
}
