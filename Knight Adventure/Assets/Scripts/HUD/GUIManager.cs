using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts;
   
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

    public GameObject[] uiPrefabs; // ������ �������� ��� UI ����

    private GameObject _currentWindow; // ������� ����

    public const int PAUSE_WINDOW = 0;
    public const int INVENTORY_WINDOW = 1;
    public const int CHARACTERISTIC_WINDOW = 2;
    public const int OPTION_WINDOW = 3;
    public const int ACHIVMENT_WINDOW = 4;
    public const int STORAGE_CHEST_WINDOW = 5;

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
        User.Instance = SaveManager.Instance.LoadLastGame();
        FirstTextAwake();
        CloseCurrentWindow();
    }

    public void SetTextAreas()
    {
        //����������� ������� ���������� �� �������� ����� USER
        _name.text = User.Instance.GetName();
        _coins.text = User.Instance.GetCoins().ToString();
        _level.text = User.Instance.GetLevel().ToString();
    }

    public void OpenWindow(int windowIndex)
    {
        // ���������� ������� ����, ���� ��� ����������
        if (_currentWindow != null)
        {
            Destroy(_currentWindow);
        }

        // �������� �� ���������� �������
        if (windowIndex >= 0 && windowIndex < uiPrefabs.Length)
        {
            // �������� ������ ����
            _currentWindow = Instantiate(uiPrefabs[windowIndex]);
            // ���������, ��� ����� ���� ����������� � Canvas
            _currentWindow.transform.SetParent(GameObject.Find("GUI_Display").transform, false);
        }
        else
        {
            Debug.LogWarning("Window index out of range: " + windowIndex);
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

    public void OpenInventory()
    {
        OpenWindow(INVENTORY_WINDOW);
        Debug.Log("Open Inventory");
    }

    public void OpenCharacteristic()
    {
        OpenWindow(CHARACTERISTIC_WINDOW);
        Debug.Log("Open Characteristic Window");
    }

    public void OpenOption()
    {
        OpenWindow(OPTION_WINDOW);
        Debug.Log("Open Option Window");
    }

    public void OpenAchivements()
    {
        OpenWindow(ACHIVMENT_WINDOW);
        Debug.Log("Open Achivements Window");
    }

    public void OpenStorageChestInventory(Inventory chestInventory)
    {
        OpenWindow(STORAGE_CHEST_WINDOW);

        // �������� ������ � ���������� ��������� (InventoryUI)
        InventoryUI inventoryUI = _currentWindow.GetComponent<InventoryUI>();
        if (inventoryUI != null)
        {
            // ��������� ��������� ������� � ��������������� UI
            inventoryUI.inventory = chestInventory;
            inventoryUI.UpdateInventoryUI(); // ��������� UI ��� ����������� ����������� �������
        }
    }
    private void FirstTextAwake()
    {
        if (User.Instance == null)
            User.Instance = SaveManager.Instance.LoadLastGame();

        //����������� ������� ���������� �� �������� ����� USER
        _name.text = User.Instance.GetName();
        _coins.text = User.Instance.GetCoins().ToString();
        _level.text = User.Instance.GetLevel().ToString();
    }
}
