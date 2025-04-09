using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using Assets.ServiceLocator;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

//������������� ��������� ����������� ���������
[RequireComponent (typeof(SceneManager))]

//����� ���������� �� ���������� HUD ����
  public class GUIManager : MonoBehaviour, IManager, IGUIManager
    {
    //��������� ���������� ��������� �����
    [SerializeField] TextMeshProUGUI _name;
    [SerializeField] TextMeshProUGUI _coins;
    [SerializeField] TextMeshProUGUI _level;

    private GameObject[] uiPrefabsInformationWindows; // ������ �������� ��� ���� � �����������

    private GameObject _currentWindow; // ������� ����
    [SerializeField] private GameObject GUIDisplay;

    private static readonly Queue<Window> windowQueue = new Queue<Window>(); // ������� ����
    private static readonly Window activeWindow; // ������� �������� ����

    private ResourcesLoadManager resourcesLoadManager;
    private PlayerData _playerData;


    public void StartManager()
    {
        gameObject.SetActive (true);

        resourcesLoadManager = gameObject.AddComponent<ResourcesLoadManager>();
        var autarizationManager = ServiceLocator.GetService<IAutarizationManager>();
        _playerData = autarizationManager.GetPlayerData();

        SetTextAreas();
        IniitializeUIPrefabsInformationWindows();
        CloseCurrentWindow();
    }
    private void IniitializeUIPrefabsInformationWindows()
    {
        uiPrefabsInformationWindows = new GameObject[3];

        uiPrefabsInformationWindows[0] = resourcesLoadManager.LoadInformationWindow("Window_Info");
        uiPrefabsInformationWindows[1] = resourcesLoadManager.LoadInformationWindow("Window_Shares");
        uiPrefabsInformationWindows[2] = resourcesLoadManager.LoadInformationWindow("Window_Entry");
    }

    public void AddQueueWindows()
    {

    }
    public void SetTextAreas()
    {
        //����������� ������� ���������� �� �������� ����� USER
        _name.text = _playerData.name;
        _coins.text = _playerData.coins.ToString();
        _level.text = _playerData.level.ToString();
    }

    // ������� ���������� ��� ������ OpenPlayerWindow
    public void OpenPlayerWindow(GameObject name)
    {
        // ���������� ������� ����, ���� ��� ����������
        if (_currentWindow != null)
        {
            Destroy(_currentWindow);
        }

         // �������� ������ ����
         _currentWindow = Instantiate(name, GUIDisplay.transform); // ���������� �� ���� ��������� ��������� ������ ����������
        
    }
    public void OpenPlayerWindow(string name)
    {
        var prefab= resourcesLoadManager.LoadPrefab(name);
        // ���������� ������� ����, ���� ��� ����������
        if (_currentWindow != null)
        {
            Destroy(_currentWindow);
        }

        // �������� ������ ����
        _currentWindow = Instantiate(prefab, GUIDisplay.transform);
    }

    /////////////////////////////////////////////////////////////////////////////////////
    //�������� ����� ��� �������� �������������� ����, ������� ����� ����������� � �������
    public void OpenInformationWindow(int windowIndex)
    {
        if (windowIndex >= 0 && windowIndex < uiPrefabsInformationWindows.Length)
        {
            GameObject windowObject = Instantiate(uiPrefabsInformationWindows[windowIndex], GUIDisplay.transform);
            Window window = windowObject.GetComponent<Window>();

            if (window != null)
            {
                Window.QueueWindow(window); // ��������� ���� � �������
            }
        }
        else
        {
            Debug.LogWarning("Window index out of range: " + windowIndex);
            var notificationManager = ServiceLocator.GetService<INotificationManager>();
            notificationManager.HandleError("��������� ������: �� ���������� ���� � �������� �������.", 0);
        }
    }
    public static void QueueWindow(Window window) // ����� ��� ���������� � �������
    {
        windowQueue.Enqueue(window);
        if (activeWindow == null)
        {
            ShowNextWindow(); // �������� ��������� ���� � �������
        }
    }
    public static bool IsQueueEmpty()
    {
        return windowQueue.Count == 0;
    }
    public static void ShowNextWindow()
    {
        if (windowQueue.Count > 0)
        {
            Window nextWindow = windowQueue.Dequeue(); // �������� ��������� ���� �� �������
            if (nextWindow != null)
            {
                nextWindow.OpenWindow(); // ��������� ��������� ����
            }
        }
        else
        {
            Time.timeScale = 1; // ���������� ������� �������� � �����, ����� ��� �������� ����
        }
    }
    public void ShowWindowQueue()
    {
        // ��������� ������� ������ ���������� (������)
        for (int i = 0; i < uiPrefabsInformationWindows.Length; i++)
        {
            OpenInformationWindow(i); // ��������� ���� � �������
        }
    }
  
    // ����� ��� �������� �������� ����
    public void CloseCurrentWindow()
    {
        if (_currentWindow != null)
        {
            Destroy(_currentWindow);
            _currentWindow = null;
        }
    }

    // ������ ��� �������� ����, ������� �� � ���� �� ���������(�� ��� ���� �� ���������� ���������) 
    public void OpenOption()
    {
        OpenPlayerWindow(resourcesLoadManager.LoadPlayerWindow("OptionWindow")); // ����� ����� �� ����
        Debug.Log("Open Option Window");
    }

    public void OpenStorageChestInventory(Inventory chestInventory)
    {
        OpenPlayerWindow( resourcesLoadManager.LoadChestWindow("Storage_Chest_Window")); // ����� ����� �� ����

        // �������� ������ � ���������� ��������� (InventoryUI)
        InventoryUI inventoryUI = _currentWindow.GetComponent<InventoryUI>();
        if (inventoryUI != null)
        {
            // ��������� ��������� ������� � ��������������� UI
            inventoryUI.inventory = chestInventory;
            //inventoryUI.UpdateInventoryUI(); // ��������� UI ��� ����������� ����������� �������
        }
    }
}
