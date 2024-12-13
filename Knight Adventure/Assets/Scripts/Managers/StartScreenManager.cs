using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class StartScreenManager : MonoBehaviour
{
    [Header("UI Elements")]
    private GameObject startScreenPrefab; // ������ ���������� ������
    private Button continueButton; // ������ ��� �����������

    private GameObject startScreen; // ��������� ���������� ������

    private void Start()
    {
        startScreenPrefab = GameManager.Instance.resourcesLoadManager.LoadStartScreenWindow("Star_Screen_Window");
       // startScreenPrefab = Resources.Load<GameObject>("Windows/StartScreenWindow/Star_Screen_Window");
        InitializeStartScreen(); // ������������� ���������� ������
    }

    private void InitializeStartScreen()
    {
        GameInput.Instance.DisableMovement();
        // �������� ���������� ���������� ������
        startScreen = Instantiate(startScreenPrefab);
        startScreen.transform.SetParent(GameObject.Find("GUI_Display").transform, false); // ����������� � Canvas

        continueButton = FindObjectOfType<Button>();
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
            GUIManager.Instance.ShowWindowQueue(); // ��������� ������ �������������� ���� ��� ������
        }
    }
}
