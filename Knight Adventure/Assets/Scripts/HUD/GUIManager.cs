using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts;
   
//Автоматически добавляем необходимые компонент
[RequireComponent (typeof(SceneManager))]
//Класс отвечающий за реализацию HUD меню
  public class GUIManager : MonoBehaviour
    {
       public static GUIManager Instance {  get; private set; }
        //Объявляем переменные текстовых полей
        [SerializeField] TextMeshProUGUI _name;
        [SerializeField] TextMeshProUGUI _coins;
        [SerializeField] TextMeshProUGUI _level;

    public GameObject[] uiPrefabs; // Массив префабов для UI окон

    private GameObject _currentWindow; // Текущее окно

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
        //Присваиваем значеие переменных из значения полей USER
        _name.text = User.Instance.GetName();
        _coins.text = User.Instance.GetCoins().ToString();
        _level.text = User.Instance.GetLevel().ToString();
    }

    public void OpenWindow(int windowIndex)
    {
        // Закрывайте текущее окно, если оно существует
        if (_currentWindow != null)
        {
            Destroy(_currentWindow);
        }

        // Проверка на валидность индекса
        if (windowIndex >= 0 && windowIndex < uiPrefabs.Length)
        {
            // Создание нового окна
            _currentWindow = Instantiate(uiPrefabs[windowIndex]);
            // Убедитесь, что новое окно прикреплено к Canvas
            _currentWindow.transform.SetParent(GameObject.Find("GUI_Display").transform, false);
        }
        else
        {
            Debug.LogWarning("Window index out of range: " + windowIndex);
        }
    }

    // Метод для закрытия текущего окна
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

        // Получаем доступ к компоненту инвентаря (InventoryUI)
        InventoryUI inventoryUI = _currentWindow.GetComponent<InventoryUI>();
        if (inventoryUI != null)
        {
            // Связываем инвентарь сундука с соответствующим UI
            inventoryUI.inventory = chestInventory;
            inventoryUI.UpdateInventoryUI(); // Обновляем UI для отображения содержимого сундука
        }
    }
    private void FirstTextAwake()
    {
        if (User.Instance == null)
            User.Instance = SaveManager.Instance.LoadLastGame();

        //Присваиваем значеие переменных из значения полей USER
        _name.text = User.Instance.GetName();
        _coins.text = User.Instance.GetCoins().ToString();
        _level.text = User.Instance.GetLevel().ToString();
    }
}
