using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyManager : MonoBehaviour
{
    public enum Difficulty { Easy, Medium, Hard }
    public Difficulty currentDifficulty;

    public TextMeshProUGUI difficultyText; // ����� ��� ����������� �������� ������ ���������
    public Button increaseDifficultyButton; // ������ ��� ���������� ���������
    public Button decreaseDifficultyButton; // ������ ��� ���������� ���������

    private void Start()
    {
        // ������������� ��������� �������� ���������
        currentDifficulty = Difficulty.Medium; // ���������� �������� �� ���������
        UpdateDifficultyText();

        // �������� �� ������� ������
        increaseDifficultyButton.onClick.AddListener(IncreaseDifficulty);
        decreaseDifficultyButton.onClick.AddListener(DecreaseDifficulty);

    }

    private void UpdateDifficultyText()
    {
        difficultyText.text = "Difficulty: " + currentDifficulty.ToString(); // ��������� �����
    }

    public void IncreaseDifficulty()
    {
        if (currentDifficulty < Difficulty.Hard)
        {
            currentDifficulty++; // ����������� ���������
            UpdateDifficultyText(); // ��������� �����
        }
    }

    public void DecreaseDifficulty()
    {
        if (currentDifficulty > Difficulty.Easy)
        {
            currentDifficulty--; // ��������� ���������
            UpdateDifficultyText(); // ��������� �����
        }
    }
}
