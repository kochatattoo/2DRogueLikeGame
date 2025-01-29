using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;
using System;
using Assets.ServiceLocator;
using Assets.Scripts.Interfaces;

public class SaveLoadMenu : MonoBehaviour
{
    public GameObject buttonPrefab; // Префаб кнопки для файлов
    public Transform contentPanel; // Панель, где будут отображаться кнопки
    public event EventHandler _LoadGame; // Событие обновления 

    private ISaveManager _saveManager; // Ссылка на объект SaveManager
    private string selectedFileName; // Переменная для хранения выбраного имени файла
    private TMP_Text selectedText; // Ссылка на текст выбранного файла

    public void StartScript() //Метод вызываемый при запуске скрипта в сцене 
    {
        _saveManager=ServiceLocator.GetService<ISaveManager>();
        if (_saveManager != null && !string.IsNullOrEmpty(_saveManager.GetSaveDirectory())) //проверяем не равен ли savemanager ничему и не пуст ли он
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

        if (!Directory.Exists(_saveManager.GetSaveDirectory()))
        {
            Debug.LogError("Save directory does not exist.");
            return;
        }

        string[] files = Directory.GetFiles(_saveManager.GetSaveDirectory(), "*.json");
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
            // Добавляем слушателя для загрузки сохранения
            newButton.GetComponent<Button>().onClick.AddListener(()=>LoadGame(fileName));
            // Добавляем слушатель для удаления сохранения
            newButton.GetComponent<Button>().onClick.AddListener(() => SelectFileForDeletion(fileName));
            // Добавляем слушателя для выбора загрузки
            newButton.GetComponent<Button>().onClick.AddListener(() => SelectFileForLoading(fileName, buttonText));
        }
    }


    private void SelectFileForLoading(string fileName, TMP_Text buttonText)
    {
        DeselectCurrentFile(); // Снимаем выделение с текущего файла
        selectedFileName = fileName; // Сохраняем имя выбранного файла
        selectedText = buttonText; // Сохраняем ссылку на текст

        HighlightSelectedFile(selectedText); // Подсвечиваем выбранный файл
        Debug.Log("Selected file for loading: " + selectedFileName); // Логируем выбранный файл
    }
    private void HighlightSelectedFile(TMP_Text buttonText)
    {
        // Здесь можно добавить вашу логику обводки текста
        buttonText.fontSize *= 1.1f; // Увеличьте размер шрифта, чтобы сделать текст более заметным
        buttonText.color = Color.yellow; // Изменяем цвет текста на желтый или измените на свои цвета
        // Можно использовать Material или Shader для создания эффекта обводки
    }

    // Метод для снятия выделения с текущего файла
    private void DeselectCurrentFile()
    {
        if (selectedText != null)
        {
            // Возвращаем текст обратно к обычному стилю
            selectedText.fontSize /= 1.1f; // Возвращаем прежний размер шрифта
            selectedText.color = Color.black; // Меняем цвет текста обратно
            // Также можно отменить обводку, если используете для этого Material или специальный эффект
        }
    }


    private void LoadGame(string fileName)
    {
        GameManager.Instance.playerData = _saveManager.LoadGame(fileName);
        if (GameManager.Instance.playerData != null)
        {
            Debug.Log("Load player: " + GameManager.Instance.playerData.name);
        }

        _LoadGame?.Invoke(this, EventArgs.Empty);
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
