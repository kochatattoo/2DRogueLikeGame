using UnityEngine;
using System.IO;
using UnityEngine.UI;
using Assets.Scripts;
using TMPro;

public class SaveLoadMenu : MonoBehaviour
{
    public GameObject buttonPrefab; // ������ ������ ��� ������
    public Transform contentPanel; // ������, ��� ����� ������������ ������
    private SaveManager _saveManager; // ������ �� ������ SaveManager
    private string selectedFileName; // ���������� ��� �������� ��������� ����� �����

    private void Start() //����� ���������� ��� ������� ������� � ����� 
    {
        _saveManager=SaveManager.Instance;
        if (_saveManager != null && !string.IsNullOrEmpty(_saveManager.saveDirectory)) //��������� �� ����� �� savemanager ������ � �� ���� �� ��
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
            // ��������� ��������� ��� �������� ����������
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
        selectedFileName = fileName; // ��������� ��� ���������� �����
        Debug.Log("Selected file for deletion: " + selectedFileName);
    }

    // ����� ��� �������� ����������
    public void DeleteSelectedSave()
    {
        if (!string.IsNullOrEmpty(selectedFileName))
        {
            _saveManager.DeleteSaveFile(selectedFileName); // ������� ���� ����� SaveManager
            Debug.Log("File deleted: " + selectedFileName);
            PopulateSaveLoadMenu(); // ��������� ���� ����� ��������
            selectedFileName = null; // ���������� ��� �����
        }
        else
        {
            Debug.LogError("No file selected for deletion.");
        }
    }
}
