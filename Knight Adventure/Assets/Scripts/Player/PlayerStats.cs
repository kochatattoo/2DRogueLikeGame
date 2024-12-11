
namespace Assets.Scripts.Player
{
    [System.Serializable ]
    public class PlayerStats
    {
        public string characterName;
        public int level;
        public float currentExperience;
        public float experienceToNextLevel;
        public float speed;
        public float maxHealth;
        public int maxMana;
        public int attack;
        public int defense;
        public int strength;
        public int intelligence;
        public int agility;

        public void CreatePlayerCharacteristics(PlayerData playerData)
        {
            // Заглушка для создания характеристики персонажа
            characterName =playerData.name;
            level = 1;
            currentExperience = 0f;
            experienceToNextLevel = 10f;
            speed =  15.0f;
            maxHealth = 15.0f;
            maxMana = 15;
            attack = 2;
            defense = 2;
            strength = 5;
            intelligence = 5;
            agility = 5;
        }
    }
}
