using UnityEngine;

// Структура для хранения информации о навыке
[System.Serializable]
public class Skill
{
        public string skillName;        // Название навыка
        public float cooldown;          // Время перезарядки
        public float duration;          // Длительность действия
        public GameObject skillEffect; // Эффект, который будет воспроизводиться при активации
 }

