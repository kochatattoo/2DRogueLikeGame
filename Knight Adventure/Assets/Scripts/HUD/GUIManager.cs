using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public GameObject[] uiPrefabsPlayerWindows; // Массив префабов для UI окон
    public GameObject[] uiPrefabsInformationWindows; // Массив префабов для окон с информацией
    public GameObject[] uiPrefabsPriorityWindows; // Массив префабов для окон с ошибками 

    private GameObject _currentWindow; // Текущее окно

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
        OpenInformationWindow(0);
        // Не понял почему при отображении 2х окон, не откликается окно и отображаются не в том порядке (как работать с очередью?)
    }

    public void SetTextAreas()
    {
        //Присваиваем значеие переменных из значения полей USER
        _name.text = GameManager.Instance.playerData.name;
        _coins.text = GameManager.Instance.playerData.coins.ToString();
        _level.text = GameManager.Instance.playerData.level.ToString();
    }

    public void OpenPlayerWindow(int windowIndex)
    {
        // Закрывайте текущее окно, если оно существует
        if (_currentWindow != null)
        {
            Destroy(_currentWindow);
        }

        // Проверка на валидность индекса
        if (windowIndex >= 0 && windowIndex < uiPrefabsPlayerWindows.Length)
        {
            // Создание нового окна
            _currentWindow = Instantiate(uiPrefabsPlayerWindows[windowIndex]);
            // Убедитесь, что новое окно прикреплено к Canvas
            _currentWindow.transform.SetParent(GameObject.Find("GUI_Display").transform, false);
        }
        else
        {
            Debug.LogWarning("Window index out of range: " + windowIndex);
        }
    }

    //Добавляю метод для создания информационных окон, коорый буду тзагружаться в очередь
    public void OpenInformationWindow(int windowIndex)
    {
        if (windowIndex >= 0 && windowIndex < uiPrefabsInformationWindows.Length)
        {
            GameObject windowObject = Instantiate(uiPrefabsInformationWindows[windowIndex]);
            windowObject.transform.SetParent(GameObject.Find("GUI_Display").transform, false) ;
            Window window = windowObject.GetComponent<Window>();
            if (window != null)
            {
                Window.QueueWindow(window); // Добавляем окно в очередь
                //window.OpenWindow();
            }
        }
        else
        {
            Debug.LogWarning("Window index out of range: " + windowIndex);
        }
    }
    //Добавляю метод для создания  окон который отображают ОШИБКИ
    public void OpenPriorityWindow(int windowIndex)
    {
        if (windowIndex >= 0 && windowIndex < uiPrefabsPriorityWindows.Length)
        {
            GameObject windowObject = Instantiate(uiPrefabsPriorityWindows[windowIndex]);
            windowObject.transform.SetParent(GameObject.Find("GUI_Display").transform, false);
            Window window = windowObject.GetComponent<Window>();
            if (window != null)
            {
                Window.ShowPriorityWindow(window); // Открываем окно с высоким приоритетом
            }
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
        OpenPlayerWindow(INVENTORY_WINDOW);
        Debug.Log("Open Inventory");
    }

    public void OpenCharacteristic()
    {
        OpenPlayerWindow(CHARACTERISTIC_WINDOW);
        Debug.Log("Open Characteristic Window");
    }

    public void OpenOption()
    {
        OpenPlayerWindow(OPTION_WINDOW);
        Debug.Log("Open Option Window");
    }

    public void OpenAchivements()
    {
        OpenPlayerWindow(ACHIVMENT_WINDOW);
        Debug.Log("Open Achivements Window");
    }

    public void OpenStorageChestInventory(Inventory chestInventory)
    {
        OpenPlayerWindow(STORAGE_CHEST_WINDOW);

        // Получаем доступ к компоненту инвентаря (InventoryUI)
        InventoryUI inventoryUI = _currentWindow.GetComponent<InventoryUI>();
        if (inventoryUI != null)
        {
            // Связываем инвентарь сундука с соответствующим UI
            inventoryUI.inventory = chestInventory;
            //inventoryUI.UpdateInventoryUI(); // Обновляем UI для отображения содержимого сундука
        }
    }
    private void FirstTextAwake()
    {
        if (Player.Instance == null)
            GameManager.Instance.playerData = SaveManager.Instance.LoadLastGame();

        //Присваиваем значеие переменных из значения полей USER
        _name.text = GameManager.Instance.playerData.name;
        _coins.text = GameManager.Instance.playerData.coins.ToString();
        _level.text = GameManager.Instance.playerData.level.ToString();
    }
}
