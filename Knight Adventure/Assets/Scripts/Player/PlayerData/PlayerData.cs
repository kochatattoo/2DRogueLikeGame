using Assets.Scripts.Player;

[System.Serializable]
public class PlayerData
    {
        public PlayerStats playerStats;
        public PlayerAchievements playerAchievements;
        public Inventory playerInventory;

        public string name = "";
        public int level = 1;
        public int coins = 10;

    }

