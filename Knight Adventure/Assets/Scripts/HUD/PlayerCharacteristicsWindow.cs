using Assets.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacteristicsWindow : MonoBehaviour
{
    public PlayerStats playerStats; // ������ �� ������ ������
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
        levelSlider.interactable = false; // ��������� ��� ��������������
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

        // ������������� �������� �������� ��� ������
        float experiencePercentage = (float)playerStats.currentExperience / playerStats.experienceToNextLevel;
        levelSlider.value = experiencePercentage;

        // ���������, ������ �� ����� ���������� ������
        if (playerStats.currentExperience >= playerStats.experienceToNextLevel)
        {
            LevelUp();
        }
    }
    private void LevelUp()
    {
        playerStats.currentExperience -= playerStats.experienceToNextLevel; // �������� ���� ��� ���������� ������
        playerStats.level++; // ����������� �������
        playerStats.experienceToNextLevel = CalculateExperienceToNextLevel(playerStats.level); // ������������ ����� ���� ��� ���������� ������
        UpdateUI(); // ��������� ���������
    }
    private float CalculateExperienceToNextLevel(int level)
    {
        // ������� ������� ��� ������� ����� ��� ���������� ������
        return level * 100; // ��������, 100 ����� �� �������
    }
}
