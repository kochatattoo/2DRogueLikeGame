```
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using Assets.Scripts;
using TMPro;

public class SaveLoadMenu : MonoBehaviour
{
    public GameObject buttonPrefab; // Префаб кнопки для файлов
    public Transform contentPanel; // Панель, где будут отображаться кнопки
    private SaveManager _saveManager; // Ссылка на объект SaveManager
    private string selectedFileName; // Переменная для хранения выбранного имени файла

    private void Start()
    {
        _saveManager = FindObjectOfType<SaveManager>();
        if (_saveManager != null && !string.IsNullOrEmpty(_saveManager.saveDirectory))
        {
            PopulateSaveLoadMenu();
        }
        else
        {
            Debug.LogError("SaveLoadManager not found or saveDirectory is empty.");
        }
    }

    // Заполняем меню сохранений
    private void PopulateSaveLoadMenu()
    {
        // Очищаем старые кнопки
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        // Проверяем существует ли директория сохранений
        if (!Directory.Exists(_saveManager.saveDirectory))
        {
            Debug.LogError("Save directory does not exist.");
            return;
        }

        // Получаем все файлы сохранений
        string[] files = Directory.GetFiles(_saveManager.saveDirectory, "*.json");
        foreach (string file in files)
        {
            string fileName = Path.GetFileNameWithoutExtension(file); // Извлекаем имя файла без расширения
            GameObject newButton = Instantiate(buttonPrefab, contentPanel); // Создаем новую кнопку
            TMP_Text buttonText = newButton.GetComponent<TMP_Text>();

            if (buttonText != null)
            {
                buttonText.text = fileName; // Устанавливаем текст кнопки
            }
            else
            {
                Debug.LogError("TMP_Text component not found on button prefab");
            }

            // Устанавливаем слушатель на кнопку для загрузки сохранения
            newButton.GetComponent<Button>().onClick.AddListener(() => LoadGame(fileName));

            // Добавляем слушатель для удаления сохранения
            newButton.GetComponent<Button>().onClick.AddListener(() => SelectFileForDeletion(fileName));
        }
    }

    // Метод для загрузки игры
    private void LoadGame(string fileName)
    {
        User loadUser = _saveManager.LoadGame(fileName);
        if (loadUser != null)
        {
            Debug.Log("Load player: " + loadUser.GetName());
        }
    }

    // Метод для выбора файла для удаления
    private void SelectFileForDeletion(string fileName)
    {
        selectedFileName = fileName; // Сохраняем имя выбранного файла
        Debug.Log("Selected file for deletion: " + selectedFileName);
    }

    // Метод для удаления сохранения
    public void DeleteSelectedSave()
    {
        if (!string.IsNullOrEmpty(selectedFileName))
        {
            _saveManager.DeleteSaveFile(selectedFileName); // Удаляем файл через SaveManager
            Debug.Log("File deleted: " + selectedFileName);
            PopulateSaveLoadMenu(); // Обновляем меню после удаления
            selectedFileName = null; // Сбрасываем имя файла
        }
        else
        {
            Debug.LogError("No file selected for deletion.");
        }
    }
}
```

### Объяснение степеней реализации:

1. **Добавлены переменные**:
    
    - `private string selectedFileName;`: Для хранения имени файла, который пользователь выбрал для удаления.
2. **Метод `SelectFileForDeletion`**:
    
    - Этот метод получает имя файла, который пользователь выбрал, и сохраняет его в `selectedFileName`. Вызывается при нажатии на кнопку загрузки.
3. **Метод `DeleteSelectedSave()`**:
    
    - Удаляет сохранение, используя метод `DeleteSaveFile` из класса `SaveManager`.
    - После удаления обновляется меню вызовом `PopulateSaveLoadMenu()`, чтобы отобразить актуальный список файлов.

### Настройка в Unity:

1. Убедитесь, что у кнопки удаления на сцене есть ссылка на метод `DeleteSelectedSave()`.
2. Теперь после того как пользователь нажмет на кнопку загрузки, он сможет нажать кнопку для удаления, и сохранение будет удалено.

### Как это сделать `buttonPrefab` для удаления:

В UI вам может понадобиться добавить отдельную кнопку для удаления сохранения, чтобы пользователь мог подтвердить, что хочет удалить выбранное сохранение. Например, создайте новую кнопку с текстом "Удалить", и при нажатии на нее будет вызываться метод `DeleteSelectedSave`.

