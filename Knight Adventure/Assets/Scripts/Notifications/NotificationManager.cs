using Assets.Scripts.Interfaces;
using UnityEngine;

 public class NotificationManager : MonoBehaviour, INotificationManager
 {
    [SerializeField] private GameObject _notificationDisplay;
    private ResourcesLoadManager _resourcesLoadManager;
    private GameObject[] uiPrefabsPriorityWindows;
    
    public void StartManager()
    {
        _resourcesLoadManager = GetComponent<ResourcesLoadManager>();
        LoadNotificationWindows();

    }
    private void LoadNotificationWindows()
    {
        uiPrefabsPriorityWindows = new GameObject[1];

        uiPrefabsPriorityWindows[0] = _resourcesLoadManager.LoadPriorityWindow("Window_Warning");
    }
    public void OpenNotificationWindow(string name)
    {
        switch (name)
        {
            case "Error":
                OpenPriorityWindow(0);
                break;

            case "Notifice":
                OpenPriorityWindow(1);
                break;

            default:
                break;

        }
    }
    public void OpenPriorityWindow(int windowIndex)
    {
        if (windowIndex >= 0 && windowIndex < uiPrefabsPriorityWindows.Length)
        {
            GameObject windowObject = Instantiate(uiPrefabsPriorityWindows[windowIndex], _notificationDisplay.transform);
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
}

