using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using Assets.ServiceLocator;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

//Автоматически добавляем необходимые компонент
[RequireComponent (typeof(SceneManager))]

//Класс отвечающий за реализацию HUD меню
  public class GUIManager : MonoBehaviour, IManager, IGUIManager
    {

        //Объявляем переменные текстовых полей
    [SerializeField] TextMeshProUGUI _name;
    [SerializeField] TextMeshProUGUI _coins;
    [SerializeField] TextMeshProUGUI _level;

    private GameObject[] uiPrefabsInformationWindows; // Массив префабов для окон с информацией
   // private GameObject[] uiPrefabsPriorityWindows; // Массив префабов для окон с ошибками 

    private GameObject _currentWindow; // Текущее окно

    [SerializeField] GameObject GUIDisplay;

    private static readonly Queue<Window> windowQueue = new Queue<Window>(); // Очередь окон
    private static readonly Window activeWindow; // Текущее активное окно

    private ResourcesLoadManager resourcesLoadManager;

    private PlayerData _playerData;

    public void StartManager()
    {
       resourcesLoadManager = gameObject.AddComponent<ResourcesLoadManager>();

        var autarizationManager = ServiceLocator.GetService<IAutarizationManager>();
        _playerData = autarizationManager.GetPlayerData();

        SetTextAreas();

        IniitializeUIPrefabsInformationWindows();
        //IniitializeUIPrefabsWarningWindows();

        CloseCurrentWindow();

    }
    private void IniitializeUIPrefabsInformationWindows()
    {
        uiPrefabsInformationWindows = new GameObject[3];

        uiPrefabsInformationWindows[0] = resourcesLoadManager.LoadInformationWindow("Window_Info");
        uiPrefabsInformationWindows[1] = resourcesLoadManager.LoadInformationWindow("Window_Shares");
        uiPrefabsInformationWindows[2] = resourcesLoadManager.LoadInformationWindow("Window_Entry");
    }
    /*private void IniitializeUIPrefabsWarningWindows()
    {
        uiPrefabsPriorityWindows = new GameObject[1];

        uiPrefabsPriorityWindows[0] = resourcesLoadManager.LoadPriorityWindow("Window_Warning");
    }*/
    public void AddQueueWindows()
    {

    }
    public void SetTextAreas()
    {
        //Присваиваем значеие переменных из значения полей USER
        _name.text = _playerData.name;
        _coins.text = _playerData.coins.ToString();
        _level.text = _playerData.level.ToString();
    }

    // Сделаем перегрузку для метода OpenPlayerWindow
    public void OpenPlayerWindow(GameObject name)
    {
        // Закрывайте текущее окно, если оно существует
        if (_currentWindow != null)
        {
            Destroy(_currentWindow);
        }

         // Создание нового окна
         _currentWindow = Instantiate(name, GUIDisplay.transform); // переписать ко всем интсниэйт родителей вторым аргументом
        
    }
    public void OpenPlayerWindow(string name)
    {
        var prefab= resourcesLoadManager.LoadPrefab(name);
        // Закрывайте текущее окно, если оно существует
        if (_currentWindow != null)
        {
            Destroy(_currentWindow);
        }

        // Создание нового окна
        _currentWindow = Instantiate(prefab, GUIDisplay.transform);
    }

    /////////////////////////////////////////////////////////////////////////////////////
    //Добавляю метод для создания информационных окон, который будут загружаться в очередь
    public void OpenInformationWindow(int windowIndex)
    {
        if (windowIndex >= 0 && windowIndex < uiPrefabsInformationWindows.Length)
        {
            GameObject windowObject = Instantiate(uiPrefabsInformationWindows[windowIndex], GUIDisplay.transform);

           // windowObject.transform.SetParent(GUIDisplay.transform, false) ;

            Window window = windowObject.GetComponent<Window>();
            if (window != null)
            {
                Window.QueueWindow(window); // Добавляем окно в очередь
                //window.OpenWindow();
            }
        }
        else
        {
            /* Debug.LogWarning("Window index out of range: " + windowIndex);
             HandleError("Произошла ошибка: Не существует окна в указаном индексе.", 0);*/

            Debug.LogWarning("Window index out of range: " + windowIndex);
            var notificationManager = ServiceLocator.GetService<INotificationManager>();
            notificationManager.HandleError("Произошла ошибка: Не существует окна в указаном индексе.", 0);
        }
    }
    public static void QueueWindow(Window window) // Метод для добавления в очередь
    {
        windowQueue.Enqueue(window);
        if (activeWindow == null)
        {
            ShowNextWindow(); // Показать следующее окно в очереди
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
            Window nextWindow = windowQueue.Dequeue(); // Получаем следующее окно из очереди
            if (nextWindow != null)
            {
                nextWindow.OpenWindow(); // Открываем следующее окно
            }
        }
        else
        {
            Time.timeScale = 1; // Возвращаем игровую скорость к норме, когда нет активных окон
        }
    }
    public void ShowWindowQueue()
    {
        // Наполняем очередь окнами информации (пример)
        for (int i = 0; i < uiPrefabsInformationWindows.Length; i++)
        {
            OpenInformationWindow(i); // Добавляем окна в очередь
        }
    }

    //Добавляю метод для создания  окон который отображают ОШИБКИ
    /*
    public void OpenPriorityWindow(int windowIndex)
    {
        if (windowIndex >= 0 && windowIndex < uiPrefabsPriorityWindows.Length)
        {
            GameObject windowObject = Instantiate(uiPrefabsPriorityWindows[windowIndex], GUIDisplay.transform);
           // windowObject.transform.SetParent(GUIDisplay.transform.transform, false);

            Window window = windowObject.GetComponent<Window>();
            if (window != null)
            {
                Window.ShowPriorityWindow(window); // Открываем окно с высоким приоритетом
                //Window.QueueWindow(window);
            }
        }
        else
        {
            Debug.LogWarning("Window index out of range: " + windowIndex);
            HandleError("Произошла ошибка: Не существует окна в указаном индексе.", 0);
        }
    }
    public void HandleError(string errorMessage, int numberOfError)
    {
        Debug.LogError(errorMessage); // Записываем сообщение об ошибке в консоль

        // Можно создать и открыть окно с сообщением об ошибке
        OpenPriorityWindow(numberOfError); // Например, если у вас есть префаб окна с индексом 0 для ошибок
    }
    *////////////////////////////////////////////////////////////////////////////////////////////////////
    
    // Метод для закрытия текущего окна
    public void CloseCurrentWindow()
    {
        if (_currentWindow != null)
        {
            Destroy(_currentWindow);
            _currentWindow = null;
        }
    }

    // Методы для открытия окон, которые ни к чему не привязаны(ну или пока не получилось привязать) 
    public void OpenOption()
    {
        OpenPlayerWindow(resourcesLoadManager.LoadPlayerWindow("OptionWindow")); // Новый метод по пути
        Debug.Log("Open Option Window");
    }

    public void OpenStorageChestInventory(Inventory chestInventory)
    {
        OpenPlayerWindow( resourcesLoadManager.LoadChestWindow("Storage_Chest_Window")); // Новый метод по пути

        // Получаем доступ к компоненту инвентаря (InventoryUI)
        InventoryUI inventoryUI = _currentWindow.GetComponent<InventoryUI>();
        if (inventoryUI != null)
        {
            // Связываем инвентарь сундука с соответствующим UI
            inventoryUI.inventory = chestInventory;
            //inventoryUI.UpdateInventoryUI(); // Обновляем UI для отображения содержимого сундука
        }
    }
}
