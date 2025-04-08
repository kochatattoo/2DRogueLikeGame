using Assets.Scripts.Player;

[System.Serializable]
public class PlayerData
{
   public PlayerStats playerStats;
   public PlayerAchievements playerAchievements;
   public Inventory playerInventory;

   public string name;
   public int level;
   public int coins;

   public PlayerData CreatePlayer(string name)
    {
        var newPlayer = new PlayerData();
        newPlayer.name = name;
        newPlayer.level = 1;
        newPlayer.coins = 0;
        newPlayer.SetStartStats(name);
        newPlayer.SetStartAchievements();

        return newPlayer;
    }
    private void SetStartStats(string name)
    {
        playerStats.CreatePlayerCharacteristics(name);
    }
    private void SetStartAchievements()
    {
        playerAchievements = new PlayerAchievements();
    }
}

