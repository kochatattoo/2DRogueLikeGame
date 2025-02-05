using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using Assets.Scripts.Interfaces;
using Assets.ServiceLocator;

public class StartScreenManager : MonoBehaviour, IStartScreenManager
{
    [Header("UI Elements")]
    private GameObject startScreenPrefab; // ������ ���������� ������
    private Button continueButton; // ������ ��� �����������

    private GameObject startScreen; // ��������� ���������� ������
    private IGameInput gameInput;
    private IGUIManager guiManager;

    private void Start()
    {
       // startScreenPrefab = GameManager.Instance.resourcesLoadManager.LoadStartScreenWindow("Star_Screen_Window");
       //// startScreenPrefab = Resources.Load<GameObject>("Windows/StartScreenWindow/Star_Screen_Window");
       // InitializeStartScreen(); // ������������� ���������� ������
    }
    public void StartManager()
    {
        ResourcesLoadManager resourcesLoadManager  = gameObject.AddComponent<ResourcesLoadManager>(); 
        startScreenPrefab = resourcesLoadManager.LoadStartScreenWindow("Star_Screen_Window");
        // startScreenPrefab = Resources.Load<GameObject>("Windows/StartScreenWindow/Star_Screen_Window");
        gameInput = ServiceLocator.GetService<IGameInput>();
        guiManager = ServiceLocator.GetService<IGUIManager>();
        InitializeStartScreen(); // ������������� ���������� ������
    }
    private void InitializeStartScreen()
    {
        gameInput.DisableMovement();

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
        if (guiManager != null) // ���������, ��� GUIManager ����������
        {
            guiManager.ShowWindowQueue(); // ��������� ������ �������������� ���� ��� ������
        }
    }
}
