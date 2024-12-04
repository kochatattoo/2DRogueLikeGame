
namespace Assets.Scripts.Player
{
    [System.Serializable]
    public class PlayerAchievements
    {
        public string[] rewards;
        public string[] achievements;

        public PlayerAchievements()
        {
            rewards = new string[1];
            rewards[0] = "No Rewards";
            achievements = new string[1];
            achievements[0] = "No Achievements";
        }

    }
}
