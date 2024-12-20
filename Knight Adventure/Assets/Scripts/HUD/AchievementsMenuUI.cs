using Assets.Scripts.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementsMenuUI : MonoBehaviour
{
    public PlayerAchievements playerAchievements; // ������ �� ������ ������
    public TextMeshProUGUI headerText; // ��������� ����
    public TextMeshProUGUI rewardsText; // ������ ������
    public TextMeshProUGUI achievementsText; // ������ ����������

    private void Start()
    {
        playerAchievements = Player.Instance.playerAchievements;
        UpdateUI();
    }

    public void UpdateUI()
    {
        rewardsText.text = "Rewards:\n" + string.Join("\n", playerAchievements.rewards);
        achievementsText.text = "Achievements:\n" + string.Join("\n", playerAchievements.achievements);
    }

    public static void OpenAchivements()
    {
        //OpenPlayerWindow(ACHIVMENT_WINDOW);
        GUIManager.Instance.OpenPlayerWindow(GameManager.Instance.resourcesLoadManager.LoadPlayerWindow("AchivmentsWindow")); // ����� ����� �� ����
        Debug.Log("Open Achivements Window");
    }
}
