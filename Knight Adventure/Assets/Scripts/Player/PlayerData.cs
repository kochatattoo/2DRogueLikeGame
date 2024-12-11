using Assets.Scripts.Player;

[System.Serializable]
public class PlayerData
    {
        public static  PlayerData Instance {  get; private set; }

        public PlayerStats playerStats;
        public PlayerAchievements playerAchievements;
        public Inventory playerInventory;

        public string name = "";
        public int level = 1;
        public int coins = 10;

        public void LoadLastGame()
        {
            PlayerData data = SaveManager.Instance.LoadLastGame();

            name = data.playerStats.characterName;
            level = data.playerStats.level;
            coins = data.coins;
            playerStats = data.playerStats;
            playerAchievements = data.playerAchievements;

            //Загружаем состояние инвентаря
            playerInventory = data.playerInventory;
        }

        public void LoadUser(string fileName)
        {
            PlayerData data = SaveManager.Instance.LoadGame(fileName);

            name = data.name;
            level = data.level;
            coins = data.coins;

            playerStats = data.playerStats;
            playerAchievements = data.playerAchievements;
            playerInventory = data.playerInventory;
        }
    }

