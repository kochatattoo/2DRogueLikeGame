
namespace Assets.Scripts.Player
{
    [System.Serializable ]
    public class PlayerStats
    {
        public string characterName;
        public int level;
        public float currentExperience;
        public float experienceToNextLevel;
        public int maxHealth;
        public int maxMana;
        public int attack;
        public int defense;
        public int strength;
        public int intelligence;
        public int agility;

        public void CreatePlayerCharacteristics(User user)
        {
            // Заглушка для создания характеристики персонажа
            characterName = user.name;
            level = user.GetLevel();
            currentExperience = 0f;
            experienceToNextLevel = 10f;
            maxHealth = 15;
            maxMana = 15;
            attack = 2;
            defense = 2;
            strength = 5;
            intelligence = 5;
            agility = 5;
        }
    }
}
