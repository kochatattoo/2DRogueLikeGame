using Assets.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacteristicsWindow : MonoBehaviour
{
    public PlayerStats playerStats; // Ссылка на данные игрока
    public TextMeshProUGUI characterNameText;
    public Slider levelSlider;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI manaText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI defenseText;
    public TextMeshProUGUI strengthText;
    public TextMeshProUGUI intelligenceText;
    public TextMeshProUGUI agilityText;

    private void Start()
    {
        playerStats = Player.Instance.playerStats;
        levelSlider.interactable = false; // Закрываем для взаимодействия
        UpdateUI();
        
    }

    public void UpdateUI()
    {
        characterNameText.text = playerStats.characterName;
        healthText.text = "Health: " + playerStats.maxHealth;
        manaText.text = "Mana: " + playerStats.maxMana;
        attackText.text = "Attack: " + playerStats.attack;
        defenseText.text = "Defense: " + playerStats.defense;
        strengthText.text = "Strength: " + playerStats.strength;
        intelligenceText.text = "Intelligence: " + playerStats.intelligence;
        agilityText.text = "Agility: " + playerStats.agility;

        // Устанавливаем значение слайдера для уровня
        float experiencePercentage = (float)playerStats.currentExperience / playerStats.experienceToNextLevel;
        levelSlider.value = experiencePercentage;

        // Проверяем, достиг ли игрок следующего уровня
        if (playerStats.currentExperience >= playerStats.experienceToNextLevel)
        {
            LevelUp();
        }
    }
    private void LevelUp()
    {
        playerStats.currentExperience -= playerStats.experienceToNextLevel; // Вычитаем опыт для следующего уровня
        playerStats.level++; // Увеличиваем уровень
        playerStats.experienceToNextLevel = CalculateExperienceToNextLevel(playerStats.level); // Рассчитываем новый опыт для следующего уровня
        UpdateUI(); // Обновляем интерфейс
    }
    private float CalculateExperienceToNextLevel(int level)
    {
        // Простая формула для расчета опыта для следующего уровня
        return level * 100; // Например, 100 опыта на уровень
    }
}
