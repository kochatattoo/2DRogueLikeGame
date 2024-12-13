using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyManager : MonoBehaviour
{
    public enum Difficulty { Easy, Medium, Hard }
    public Difficulty currentDifficulty;

    public TextMeshProUGUI difficultyText; // Текст для отображения текущего уровня сложности
    public Button increaseDifficultyButton; // Кнопка для увеличения сложности
    public Button decreaseDifficultyButton; // Кнопка для уменьшения сложности

    private void Start()
    {
        // Устанавливаем начальное значение сложности
        currentDifficulty = Difficulty.Medium; // Установите значение по умолчанию
        UpdateDifficultyText();

        // Подписка на события кнопок
        increaseDifficultyButton.onClick.AddListener(IncreaseDifficulty);
        decreaseDifficultyButton.onClick.AddListener(DecreaseDifficulty);

    }

    private void UpdateDifficultyText()
    {
        difficultyText.text = "Difficulty: " + currentDifficulty.ToString(); // Обновляем текст
    }

    public void IncreaseDifficulty()
    {
        if (currentDifficulty < Difficulty.Hard)
        {
            currentDifficulty++; // Увеличиваем сложность
            UpdateDifficultyText(); // Обновляем текст
        }
    }

    public void DecreaseDifficulty()
    {
        if (currentDifficulty > Difficulty.Easy)
        {
            currentDifficulty--; // Уменьшаем сложность
            UpdateDifficultyText(); // Обновляем текст
        }
    }
}
