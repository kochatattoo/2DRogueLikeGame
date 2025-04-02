using Assets.Scripts.Interfaces;
using UnityEngine;

 public class NotificationManager : MonoBehaviour, IManager, INotificationManager
 {
    [SerializeField] private GameObject _notificationCanvas;
    private ResourcesLoadManager _resourcesLoadManager;
    private GameObject[] uiPrefabsPriorityWindows;
    private AudioSource _audioSource;
    public AudioClip[] uiPrefabsSounds;
    
    public void StartManager()
    {
        _resourcesLoadManager = gameObject.AddComponent<ResourcesLoadManager>(); ;
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.GetComponent<AudioSource>();

        LoadNotificationWindows();
        LoadNotificationAudioClips();

    }
    private void LoadNotificationWindows()
    {
        uiPrefabsPriorityWindows = new GameObject[1];

        uiPrefabsPriorityWindows[0] = _resourcesLoadManager.LoadPriorityWindow("Window_Warning");
    }
    private void LoadNotificationAudioClips()
    {
        uiPrefabsSounds = new AudioClip[3];
        uiPrefabsSounds[0] = _resourcesLoadManager.LoadNotificationAudioClips("Lock");
        uiPrefabsSounds[1] = _resourcesLoadManager.LoadNotificationAudioClips("Notice");
        uiPrefabsSounds[2] = _resourcesLoadManager.LoadNotificationAudioClips("Error");
    }
    public void OpenNotificationWindow(string name)
    {
        switch (name)
        {
            case "Error":
                OpenPriorityWindow(0);
                break;

            case "Notice":
                OpenPriorityWindow(1);
                break;

            default:
                break;

        }
    }
    public void OpenNotificationWindow(string name, string txt)
    {
        switch (name)
        {
            case "Error":
                OpenPriorityWindow(0);
                SetWindowText(0, txt);
                break;

            case "Notice":
                OpenPriorityWindow(1);
                break;

            default:
                break;

        }
    }
    // Метод для срабатывания звука при невозможности использования, своего рода звуковое уведомелние без необходимости вызыва окна с ошибкой
    // На окно с ошибкой, поставлю отдельный звук для загрузки и вызова при появлении
    // Звуки кликов, вызова менюшек и прочие будут загружаться и вызываться из AudioManager
    public void PlayNotificationAudio(string name) // TODO: Возможно изменить на разные методы для частного вызова
    {
        switch (name)
        {
            case "Lock":
                PlayAudioCLips(0);
                break;

            case "Notice":
                PlayAudioCLips(1);
                break;

            case "Error":
                PlayAudioCLips(2);
                break;

            default:
                break;

        }
    }
    public void OpenPriorityWindow(int windowIndex)
    {
        if (windowIndex >= 0 && windowIndex < uiPrefabsPriorityWindows.Length)
        {
            GameObject windowObject = Instantiate(uiPrefabsPriorityWindows[windowIndex], _notificationCanvas.transform);
            // windowObject.transform.SetParent(GUIDisplay.transform.transform, false);

            Window window = windowObject.GetComponent<Window>();
            if (window != null)
            {
                Window.ShowPriorityWindow(window); // Открываем окно с высоким приоритетом
                PlayNotificationAudio("Error");
            }
        }
        else
        {
            Debug.LogWarning("Window index out of range: " + windowIndex);
            HandleError("Произошла ошибка: Не существует окна в указаном индексе.", 0);
            PlayNotificationAudio("Error");
        }
    }
    private void SetWindowText(int window_index, string txt)
    {
        GameObject window_prefab = uiPrefabsPriorityWindows[(int)window_index];
        var window = GetComponent<Window>();
        window.window_text.text = txt; // TODO: Вот тут ошибка Object Reference not set
        
    }
    public void HandleError(string errorMessage, int numberOfError)
    {
        Debug.LogError(errorMessage); // Записываем сообщение об ошибке в консоль

        // Можно создать и открыть окно с сообщением об ошибке
        OpenPriorityWindow(numberOfError); // Например, если у вас есть префаб окна с индексом 0 для ошибок
    }
    public void PlayAudioCLips(int audioIndex)
    {
        if (uiPrefabsSounds.Length > 1 && uiPrefabsSounds[audioIndex] != null)
        {
            _audioSource.PlayOneShot(uiPrefabsSounds[audioIndex]);
        }
    }
}

