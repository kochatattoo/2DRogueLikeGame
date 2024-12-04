-Скрипт отвечающий за данные пользователя
```
namespace Assets.Scripts
{
    [System.Serializable]
    public class User
    {
        public static User Instance {  get; set; }

        private string name="";
        private int level=1;
        private int coins=10;

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
      
        public void SaveUserSerialize()
        {
            SaveManager.SaveUser(this);
        }
        public void LoadUserSerialize()
        {
            User data = SaveManager.LoadUser();
            
            name = data.name;
            level = data.level;
            coins = data.coins;
        }
        public void ResetData()
        {
            User data = SaveManager.ResetData();

            name = data.name;
            level = data.level;
            coins = data.coins;
        }
    }
}

```
И считывания данных при сохранении и загрузки