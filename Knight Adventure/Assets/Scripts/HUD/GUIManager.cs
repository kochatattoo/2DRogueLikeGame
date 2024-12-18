using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

//������������� ��������� ����������� ���������
[RequireComponent (typeof(SceneManager))]

//����� ���������� �� ���������� HUD ����
  public class GUIManager : MonoBehaviour
    {
       public static GUIManager Instance {  get; private set; }
        //��������� ���������� ��������� �����
        [SerializeField] TextMeshProUGUI _name;
        [SerializeField] TextMeshProUGUI _coins;
        [SerializeField] TextMeshProUGUI _level;

    public GameObject[] uiPrefabsPlayerWindows; // ������ �������� ��� UI ����
    public GameObject[] uiPrefabsInformationWindows; // ������ �������� ��� ���� � �����������
    public GameObject[] uiPrefabsPriorityWindows; // ������ �������� ��� ���� � �������� 

    private GameObject _currentWindow; // ������� ����

    [SerializeField] GameObject GUIDisplay;

    private static Queue<Window> windowQueue = new Queue<Window>(); // ������� ����
    private static Window activeWindow; // ������� �������� ����

    public const int PAUSE_WINDOW = 0; // Pause_Menu_Display
    public const int INVENTORY_WINDOW = 1; // InventoryUI
    public const int CHARACTERISTIC_WINDOW = 2; // CharacteristicWindow
    public const int OPTION_WINDOW = 3; // OptionWindow
    public const int ACHIVMENT_WINDOW = 4; // AchivementsWindow
    public const int STORAGE_CHEST_WINDOW = 5; // Storage_CHest_Window

    //private User user;

       private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else 
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
            //Debug.Log(GameManager.Instance.user.GetName());

       }

    private void Start()
    {
        GameManager.Instance.playerData = SaveManager.Instance.LoadLastGame();
        FirstTextAwake();
        CloseCurrentWindow();

        // ��������� ������� ������ ���������� (������)
        //for (int i = 0; i < uiPrefabsInformationWindows.Length; i++)
        //{
        //    OpenInformationWindow(i); // ��������� ���� � �������
        //}

        //ShowWindowQueue();

    }
    public void AddQueueWindows()
    {

    }
    public void SetTextAreas()
    {
        //����������� ������� ���������� �� �������� ����� USER
        _name.text = GameManager.Instance.playerData.name;
        _coins.text = GameManager.Instance.playerData.coins.ToString();
        _level.text = GameManager.Instance.playerData.level.ToString();
    }

    public void OpenPlayerWindow(int windowIndex)
    {
        // ���������� ������� ����, ���� ��� ����������
        if (_currentWindow != null)
        {
            Destroy(_currentWindow);
        }

        // �������� �� ���������� �������
        if (windowIndex >= 0 && windowIndex < uiPrefabsPlayerWindows.Length)
        {
            // �������� ������ ����
            _currentWindow = Instantiate(uiPrefabsPlayerWindows[windowIndex]);
            // ���������, ��� ����� ���� ����������� � Canvas
            _currentWindow.transform.SetParent(GUIDisplay.transform, false);
        }
        else
        {
            Debug.LogWarning("Window index out of range: " + windowIndex);
            HandleError("��������� ������: �� ���������� ���� � �������� �������.", 0);
        }
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
    /////////////////////////////////////////////////////////////////////////////////////
    //�������� ����� ��� �������� �������������� ����, ������� ����� ����������� � �������
    public void OpenInformationWindow(int windowIndex)
    {
        if (windowIndex >= 0 && windowIndex < uiPrefabsInformationWindows.Length)
        {
            GameObject windowObject = Instantiate(uiPrefabsInformationWindows[windowIndex]);
            windowObject.transform.SetParent(GUIDisplay.transform, false) ;

            Window window = windowObject.GetComponent<Window>();
            if (window != null)
            {
                Window.QueueWindow(window); // ��������� ���� � �������
                //window.OpenWindow();
            }
        }
        else
        {
            Debug.LogWarning("Window index out of range: " + windowIndex);
            HandleError("��������� ������: �� ���������� ���� � �������� �������.", 0);
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

    //�������� ����� ��� ��������  ���� ������� ���������� ������
    public void OpenPriorityWindow(int windowIndex)
    {
        if (windowIndex >= 0 && windowIndex < uiPrefabsPriorityWindows.Length)
        {
            GameObject windowObject = Instantiate(uiPrefabsPriorityWindows[windowIndex]);
            windowObject.transform.SetParent(GUIDisplay.transform.transform, false);

            Window window = windowObject.GetComponent<Window>();
            if (window != null)
            {
                Window.ShowPriorityWindow(window); // ��������� ���� � ������� �����������
                //Window.QueueWindow(window);
            }
        }
        else
        {
            Debug.LogWarning("Window index out of range: " + windowIndex);
            HandleError("��������� ������: �� ���������� ���� � �������� �������.", 0);
        }
    }
    public void HandleError(string errorMessage, int numberOfError)
    {
        Debug.LogError(errorMessage); // ���������� ��������� �� ������ � �������

        // ����� ������� � ������� ���� � ���������� �� ������
        OpenPriorityWindow(numberOfError); // ��������, ���� � ��� ���� ������ ���� � �������� 0 ��� ������
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // ����� ��� �������� �������� ����
    public void CloseCurrentWindow()
    {
        if (_currentWindow != null)
        {
            Destroy(_currentWindow);
            _currentWindow = null;
        }
    }

    public void OpenInventory()
    {
        //OpenPlayerWindow(INVENTORY_WINDOW);
        OpenPlayerWindow(GameManager.Instance.resourcesLoadManager.LoadPlayerWindow("InventoryWindow")); // ����� ����� �� ����
        Debug.Log("Open Inventory"); 
    }

    public void OpenCharacteristic()
    {
        //OpenPlayerWindow(CHARACTERISTIC_WINDOW);
        OpenPlayerWindow(GameManager.Instance.resourcesLoadManager.LoadPlayerWindow("CharactristicWindow")); // ����� ����� �� ����
        Debug.Log("Open Characteristic Window");
    }

    public void OpenOption()
    {
        //OpenPlayerWindow(OPTION_WINDOW);
        OpenPlayerWindow(GameManager.Instance.resourcesLoadManager.LoadPlayerWindow("OptionWindow")); // ����� ����� �� ����
        Debug.Log("Open Option Window");
    }

    public void OpenAchivements()
    {
        //OpenPlayerWindow(ACHIVMENT_WINDOW);
        OpenPlayerWindow(GameManager.Instance.resourcesLoadManager.LoadPlayerWindow("AchivmentsWindow")); // ����� ����� �� ����
        Debug.Log("Open Achivements Window");
    }

    public void OpenStorageChestInventory(Inventory chestInventory)
    {
        //OpenPlayerWindow(STORAGE_CHEST_WINDOW); - ������ ����� �� �������
        OpenPlayerWindow( GameManager.Instance.resourcesLoadManager.LoadChestWindow("Storage_Chest_Window")); // ����� ����� �� ����

        // �������� ������ � ���������� ��������� (InventoryUI)
        InventoryUI inventoryUI = _currentWindow.GetComponent<InventoryUI>();
        if (inventoryUI != null)
        {
            // ��������� ��������� ������� � ��������������� UI
            inventoryUI.inventory = chestInventory;
            //inventoryUI.UpdateInventoryUI(); // ��������� UI ��� ����������� ����������� �������
        }
    }
    private void FirstTextAwake()
    {
        if (Player.Instance == null)
            GameManager.Instance.playerData = SaveManager.Instance.LoadLastGame();

        //����������� ������� ���������� �� �������� ����� USER
        _name.text = GameManager.Instance.playerData.name;
        _coins.text = GameManager.Instance.playerData.coins.ToString();
        _level.text = GameManager.Instance.playerData.level.ToString();
    }
}
