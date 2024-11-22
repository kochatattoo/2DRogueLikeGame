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
    private string selectedFileName; // Переменная для хранения выбраного имени файла

    private void Start() //Метод вызываемый при запуске скрипта в сцене 
    {
        _saveManager=SaveManager.Instance;
        if (_saveManager != null && !string.IsNullOrEmpty(_saveManager.saveDirectory)) //проверяем не равен ли savemanager ничему и не пуст ли он
        {
            PopulateSaveLoadMenu();
        }
        else
        {
            Debug.LogError("SaveLoadManager not found or saveDirectory is empty.");
        }
    }

    private void PopulateSaveLoadMenu()
    {
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        if (!Directory.Exists(_saveManager.saveDirectory))
        {
            Debug.LogError("Save directory does not exist.");
            return;
        }

        string[] files = Directory.GetFiles(_saveManager.saveDirectory, "*.json");
        foreach (string file in files)
        {
            string fileName = Path.GetFileNameWithoutExtension(file);
            GameObject newButton = Instantiate(buttonPrefab, contentPanel);
            TMP_Text buttonText = newButton.GetComponent<TMP_Text>();

            if (buttonText != null)
            {
                buttonText.text = fileName;
            }
            else
            {
                Debug.LogError("TMP_Text component not found on button prefab");
            }

            newButton.GetComponent<Button>().onClick.AddListener(()=>LoadGame(fileName));
            // Добавляем слушатель для удаления сохранения
            newButton.GetComponent<Button>().onClick.AddListener(() => SelectFileForDeletion(fileName));
        }
    }

    private void LoadGame(string fileName)
    {
        User.Instance = _saveManager.LoadGame(fileName);
        if (User.Instance != null)
        {
            Debug.Log("Load player: " + User.Instance.GetName());
        }
    }

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
