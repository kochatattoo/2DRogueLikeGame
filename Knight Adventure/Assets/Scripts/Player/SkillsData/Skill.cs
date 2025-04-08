using UnityEngine;
using UnityEngine.UI;

// Структура для хранения информации о навыке
[System.Serializable]
public class Skill
{
    public string skillName;        // Название навыка
    public float cooldown;          // Время перезарядки
    public float duration;          // Длительность действия
    public GameObject skillEffect; // Эффект, который будет воспроизводиться при активации
    public Image icon; // Иконка скила

 }

public class AttackSkill : Skill
{
	public float mainDamage; // Основной урон
	public float addDamage; // Добавочный урон
	public float distance; // Дистанция атаки
	
	public void ScaleLevelState(int level) // Метод для изменения уровня атаки с повышением уровня героя
	{
		mainDamage = + 0.1f*level;
		addDamage = + 0.1f*level;
	}
}

public class HealSkill : Skill
{
	public float mainHeal; // Основное исцеление
	public float addHeal; // Добавочное исцеление
}

public class PassiveSkill : Skill
{
	
}