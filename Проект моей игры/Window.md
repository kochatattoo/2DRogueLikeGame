Класс - хранящий в себе весь необходимый функционал для отображения разных окон
Скрипты других информационных окон - будут наследоваться от данного класса и использоваться в нашем проекте

```
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Window : MonoBehaviour
{
    public AudioClip openSound; // Звук появления окна
    public AudioClip closeSound; // Звук закрытия окна
    public Button closeButton; // Кнопка закрытия окна
    public Button okButton; // Кнопка "ОК"

    private AudioSource audioSource; // Аудиомодуль
    private bool isPaused = false; // Флаг паузы
    public GameObject windowPrefab; // Префаб для создания окна
    private GameObject currentWindow; // Текущее созданное окно
    private static Window activeWindow; // Текущее активное окно

    private static Queue<Window> windowQueue = new Queue<Window>(); // Очередь окон

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        closeButton.onClick.AddListener(CloseWindow); // Подписка на событие кнопки закрытия
        okButton.onClick.AddListener(OnOkButtonClicked); // Подписка на событие кнопки "ОК"
    }

    public virtual void OpenWindow()
    {
        currentWindow = Instantiate(windowPrefab); // Создаем экземпляр окна из префаба
        currentWindow.transform.SetParent(transform, false); // Устанавливаем его как дочерний элемент
        PlaySound(openSound); // Воспроизведение звука открытия
        Time.timeScale = 0; // Пауза игры
        isPaused = true; // Установить флаг паузы
        currentWindow.SetActive(true); // Активировать окно
        activeWindow = this;
    }

    public virtual void CloseWindow()
    {
        if (currentWindow == this)
        {
            PlaySound(closeSound); // Воспроизведение звука закрытия
            Destroy(currentWindow); // Удаляем текущее окно
            currentWindow = null; // Сбрасываем ссылку
            Time.timeScale = 1; // Возобновление игры
            isPaused = false; // Сброс флага паузы
            activeWindow = null; // Убираем ссылку на активное окно
            ShowNextWindow(); // Показ следующего окна из очереди
        }
    }

    protected virtual void OnOkButtonClicked()
    {
        // Логика для выполнения при нажатии "ОК" (по умолчанию ничего не делает)
        CloseWindow(); // Закрыть окно при нажатии "ОК"
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
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

    public static void ShowNextWindow() // Метод для показа следующего окна
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

    public static void ShowPriorityWindow(Window window) // Метод для показа приоритетных окон
    {
        if (activeWindow != null)
        {
            activeWindow.CloseWindow(); // Закройте текущее активное окно
        }
        window.OpenWindow(); // Откроем приоритетное окно
    }
}
```

Наследуемые классы выглядят следующим образом
```
namespace Assets.Scripts.HUD.Windows
{
    public class InfoWindow : Window
    {
        protected override void OnOkButtonClicked()
        {
            // Логика закрытия окна
            CloseWindow(); // Закрыть окно при нажатии "ОК"
        }

        public override void OpenWindow()
        {
            base.OpenWindow(); // Вызов базового метода OpenWindow
        }
        public void Show()
        {
            QueueWindow(this);
        }
    }
}
```
```
using UnityEngine;

namespace Assets.Scripts.HUD.Windows
{
    public class WarningWindow : Window
    {
        // Вы можете дополнить функционал данного окна
        protected override void OnOkButtonClicked()
        {
            base.OnOkButtonClicked(); // Вызов родительской логики
                                      // Дополнительная логика для окна предупреждения
            Debug.Log("Warning acknowledged!"); // Пример логики
        }
        public static void ShowImportantWarning()
        {
            // Создание экземпляра WarningWindow
            WarningWindow warning = Instantiate(Resources.Load<WarningWindow>("WarningWindowPrefab"));
            ShowPriorityWindow(warning);
        }
    }
}
```

### Для создания и отображения очереди окон воспользуемся следующими действиями
Для создания системы очереди окон в вашем проекте Unity, которая будет управлять отображением различных окон в определенном порядке, вы можете воспользоваться ранее описанным классом `Window` и добавить несколько вспомогательных методов в вашем `GUIManager`. Вот пошаговое руководство по реализации этой функции:

### Шаг 1: Обновление класса Window

Убедитесь, что в классе `Window` уже есть поддержка очереди и методов для управления приоритетами окон. Вот общий вид класса `Window`, который включает очередность и приоритет:
```
using UnityEngine;
using UnityEngine.UI;

public class Window : MonoBehaviour
{
    public AudioClip openSound;  // Звук появления окна
    public AudioClip closeSound; // Звук закрытия окна
    public Button closeButton;    // Кнопка закрытия окна
    public Button okButton;       // Кнопка "ОК"

    private AudioSource audioSource;  // Аудиомодуль
    private static Window activeWindow; // Текущее активное окно
    private static Queue<Window> windowQueue = new Queue<Window>(); // Очередь окон
    private static bool isPaused = false; // Флаг паузы

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // Добавляем компонент AudioSource
        closeButton.onClick.AddListener(CloseWindow); // Подписка на событие кнопки закрытия
        okButton.onClick.AddListener(OnOkButtonClicked); // Подписка на событие кнопки "ОК"
    }

    public virtual void OpenWindow()
    {
        PlaySound(openSound); // Воспроизведение звука открытия
        gameObject.SetActive(true); // Активируем окно
        Time.timeScale = 0; // Ставим игру на паузу
        isPaused = true; // Устанавливаем флаг паузы
        activeWindow = this; // Устанавливаем активное окно
    }

    public virtual void CloseWindow()
    {
        if (activeWindow == this) // Проверяем, является ли текущее активное окно
        {
            PlaySound(closeSound); // Воспроизведение звука закрытия
            gameObject.SetActive(false); // Деактивируем окно
            Time.timeScale = 1; // Возвращаем игровую скорость к норме
            isPaused = false; // Сбрасываем флаг паузы
            activeWindow = null; // Убираем ссылку на активное окно
            ShowNextWindow(); // Показ следующего окна из очереди
        }
    }

    protected virtual void OnOkButtonClicked()
    {
        CloseWindow(); // Закрываем окно при нажатии "ОК"
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public static void QueueWindow(Window window) // Метод для добавления в очередь
    {
        windowQueue.Enqueue(window);
        if (activeWindow == null)
        {
            ShowNextWindow(); // Показываем следующее окно в очереди
        }
    }

    public static void ShowNextWindow() // Метод для показа следующего окна
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

    public static void ShowPriorityWindow(Window window) // Метод для показа приоритетных окон
    {
        if (activeWindow != null)
        {
            activeWindow.CloseWindow(); // Закрываем текущее активное окно
        }
        window.OpenWindow(); // Открываем приоритетное окно
    }
}
```
### Шаг 2: Интеграция в GUIManager

В вашем классе `GUIManager` добавьте методы, которые будут управлять созданием окон и добавлять их в очередь:
```
public class GUIManager : MonoBehaviour
{
    public static GUIManager Instance { get; private set; }
    public GameObject[] uiPrefabs; // Массив префабов для UI окон

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OpenPlayerWindow(int windowIndex)
    {
        if (windowIndex >= 0 && windowIndex < uiPrefabs.Length)
        {
            GameObject windowObject = Instantiate(uiPrefabs[windowIndex]);
            Window window = windowObject.GetComponent<Window>();
            if (window != null)
            {
                Window.QueueWindow(window); // Добавляем окно в очередь
            }
        }
        else
        {
            Debug.LogWarning("Window index out of range: " + windowIndex);
        }
    }

    // Метод для открытия приоритетного окна
    public void OpenPriorityWindow(int windowIndex)
    {
        if (windowIndex >= 0 && windowIndex < uiPrefabs.Length)
        {
            GameObject windowObject = Instantiate(uiPrefabs[windowIndex]);
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
}
```
### Шаг 3: Использование очереди окон

Теперь вы можете использовать очередь окон в своем проекте. Примеры использования:

1. **Открыть обычное окно**:
    - Чтобы открыть окно, используйте метод `OpenPlayerWindow(int windowIndex)` из вашего `GUIManager`, передав индекс окна, которое нужно открыть.

```
void Update()
{
    if (Input.GetKeyDown(KeyCode.I)) // Открыть окно инвентаря при нажатии 'I'
    {
        GUIManager.Instance.OpenPlayerWindow(1); // Предположим, это индекс инвентаря
    }
}
```
2. **Открыть приоритетное окно**:
    - Чтобы открыть приоритетное окно, используйте метод `OpenPriorityWindow(int windowIndex)` аналогичным образом.

```
if (Input.GetKeyDown(KeyCode.P)) // Открыть окно предупреждения при нажатии 'P'
{
    GUIManager.Instance.OpenPriorityWindow(0); // Предположим, это индекс окна предупреждения
}
```

### Шаг 4: Существует ли очередь окон?

Система очереди уже интегрирована в вашем классе `Window`. Вся логика, описанная выше, позволяет добавлять окна в очередь, открывать их в порядке очередности, а также открывать окна с высоким приоритетом.


## Как реализовать это в Юнити
Чтобы использовать данные скрипты в Unity, следуйте следующей пошаговой инструкции. Здесь описаны все этапы, начиная с создания проекта до настройки префабов и использования классов.

### Шаг 1: Создание проекта Unity

1. **Запустите Unity Hub** и создайте новый проект, выбрав 2D или 3D в зависимости от ваших потребностей. Дайте проекту имя и нажмите "Create".

### Шаг 2: Создание скриптов в Unity

1. **Создайте папку для скриптов**:
    
    - В `Project` окне щелкните правой кнопкой мыши и выберите `Create > Folder`. Назовите папку `Scripts`.
2. **Создайте скрипты**:
    
    - Щелкните правой кнопкой на папке `Scripts` и выберите `Create > C# Script`. Создайте следующие скрипты:
        - `Window.cs`
        - `GUIManager.cs`
        - `InventoryWindow.cs` (опционально, если вы хотите создать наследующий класс для инвентаря).
    - Дважды щелкните на каждом из них, чтобы открыть их в редакторе кода (например, Visual Studio).
3. **Скопируйте и вставьте код**:
    
    - Скопируйте код из предыдущих сообщений для каждого скрипта и вставьте его в соответствующий .cs файл. Не забудьте сохранить изменения после вставки.

### Шаг 3: Создание пользовательского интерфейса (UI)

1. **Создание Canvas**:
    
    - Щелкните правой кнопкой в `Hierarchy` и выберите `UI > Canvas`. Это создаст новый Canvas, который будет служить основой для ожидания окон.
2. **Создание панели GUI**:
    
    - Щелкните правой кнопкой на `Canvas` и выберите `UI > Panel`. Это будет ваш GUI-отображатель. Назовите его `GUI_Display`.
3. **Создание префабов окон**:
    
    - Создайте отдельные `UI > Panel` для каждого окна, которое вы хотите создать (например, `InventoryWindow`, `PauseWindow` и т.д.).
    - Настройте содержимое каждого окна, добавив текстовые поля, кнопки (Close и OK).
    - Перетащите созданное окно в папку `Assets` в `Project` окне, чтобы создать префаб (это можно сделать, вытянув панель на папку).
4. **Создание массива префабов**:
    
    - Создайте новый `GameObject` в `Hierarchy` (можно назвать его `GUIManagerObject`).
    - Перетащите скрипт `GUIManager.cs` на этот объект, чтобы добавить его как компонент.
    - В `Inspector` у вас будет компонент `GUIManager`. Настройте массив `uiPrefabs` и добавьте в него созданные префабы.

### Шаг 4: Настройка префабов и кнопок

1. **Настройка кнопок**:
    
    - Убедитесь, что кнопки "Закрыть" и "ОК" в каждом префабе окна имеют правильные ссылки на функции в классе `Window`.
    - Например, выберите кнопку "Закрыть" и в `Inspector` укажите OnClick событие, используя `Window:CloseWindow()`.
2. **Добавление аудиофайлов** (если используется):
    
    - Если у вас есть звуки для открытия и закрытия окон, добавьте аудиофайлы в проект.
    - Создайте `AudioSource` в вашем классе `Window`, если это ещё не сделано, и привяжите файлы, как было показано в предыдущем коде.

### Шаг 5: Использование

1. **Запуск игры**:
    
    - Теперь, когда всё сконфигурировано, вы можете запустить игру. В `GUIManager` вы можете открывать окна, вызывая метод `OpenPlayerWindow(int windowIndex);`, передавая нужный индекс префаба.
2. **Пример использования**:
    
    - Подготовьте вызов метода для открытия окна в вашем игровом процессе, например, вы можете сделать это при нажатии кнопки или по какому-то событию в игре.
    - В `Start()` или в методе `Update()` можно добавить:

```
void Update()
{
    if (Input.GetKeyDown(KeyCode.I)) // Открыть инвентарь при нажатии 'I'
    {
        OpenPlayerWindow(INVENTORY_WINDOW);
    }

    if (Input.GetKeyDown(KeyCode.Escape)) // Закрыть текущее окно при нажатии 'Esc'
    {
        CloseCurrentWindow();
    }
}
```
### Шаг 6: Тестирование

1. **Запустите игру** из Unity, нажимая на кнопку Play.
2. Нажмите на соответствующие кнопки (если настроены) или используйте клавиши (например, 'I', 'Escape' и т.д.) для открытия/закрытия окон.