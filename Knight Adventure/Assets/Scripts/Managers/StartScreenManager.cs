using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartScreenManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject startScreenPrefab; // ������ ���������� ������
    [SerializeField] private TextMeshProUGUI welcomeText; // �������������� ���������
    [SerializeField] private Button continueButton; // ������ ��� �����������

    private GameObject startScreen; // ��������� ���������� ������

    private void Start()
    {
        InitializeStartScreen(); // ������������� ���������� ������
    }

    private void InitializeStartScreen()
    {
        // �������� ���������� ���������� ������
        startScreen = Instantiate(startScreenPrefab);
        startScreen.transform.SetParent(GameObject.Find("GUI_Display").transform, false); // ����������� � Canvas

        // ������������� ����� �����������
        welcomeText = startScreen.GetComponentInChildren<TextMeshProUGUI>();
        if (welcomeText != null)
        {
            welcomeText.text = "����� ���������� � ���� ����!"; // ���������� ����� �����������
        }

        // ����������� ������ ��� �����������
        continueButton = startScreen.GetComponentInChildren<Button>();
        if (continueButton != null)
        {
            continueButton.onClick.AddListener(OnContinueButtonClicked); // �������� �� ������� ������
        }
    }

    private void OnContinueButtonClicked()
    {
        // �������� ��������� �����
        Destroy(startScreen);

        // ����� �� ��������� ������ ���� ���������� �� �������, ���� ��� ����
        if (GUIManager.Instance != null) // ���������, ��� GUIManager ����������
        {
            GUIManager.Instance.OpenInformationWindow(0); // ��������� ������ �������������� ���� ��� ������
        }
    }
}
