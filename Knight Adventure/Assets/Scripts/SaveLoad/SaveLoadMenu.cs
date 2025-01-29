using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;
using System;
using Assets.ServiceLocator;
using Assets.Scripts.Interfaces;

public class SaveLoadMenu : MonoBehaviour
{
    public GameObject buttonPrefab; // ������ ������ ��� ������
    public Transform contentPanel; // ������, ��� ����� ������������ ������
    public event EventHandler _LoadGame; // ������� ���������� 

    private ISaveManager _saveManager; // ������ �� ������ SaveManager
    private string selectedFileName; // ���������� ��� �������� ��������� ����� �����
    private TMP_Text selectedText; // ������ �� ����� ���������� �����

    public void StartScript() //����� ���������� ��� ������� ������� � ����� 
    {
        _saveManager=ServiceLocator.GetService<ISaveManager>();
        if (_saveManager != null && !string.IsNullOrEmpty(_saveManager.GetSaveDirectory())) //��������� �� ����� �� savemanager ������ � �� ���� �� ��
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
            // ��������� ��������� ��� �������� ����������
            newButton.GetComponent<Button>().onClick.AddListener(()=>LoadGame(fileName));
            // ��������� ��������� ��� �������� ����������
            newButton.GetComponent<Button>().onClick.AddListener(() => SelectFileForDeletion(fileName));
            // ��������� ��������� ��� ������ ��������
            newButton.GetComponent<Button>().onClick.AddListener(() => SelectFileForLoading(fileName, buttonText));
        }
    }


    private void SelectFileForLoading(string fileName, TMP_Text buttonText)
    {
        DeselectCurrentFile(); // ������� ��������� � �������� �����
        selectedFileName = fileName; // ��������� ��� ���������� �����
        selectedText = buttonText; // ��������� ������ �� �����

        HighlightSelectedFile(selectedText); // ������������ ��������� ����
        Debug.Log("Selected file for loading: " + selectedFileName); // �������� ��������� ����
    }
    private void HighlightSelectedFile(TMP_Text buttonText)
    {
        // ����� ����� �������� ���� ������ ������� ������
        buttonText.fontSize *= 1.1f; // ��������� ������ ������, ����� ������� ����� ����� ��������
        buttonText.color = Color.yellow; // �������� ���� ������ �� ������ ��� �������� �� ���� �����
        // ����� ������������ Material ��� Shader ��� �������� ������� �������
    }

    // ����� ��� ������ ��������� � �������� �����
    private void DeselectCurrentFile()
    {
        if (selectedText != null)
        {
            // ���������� ����� ������� � �������� �����
            selectedText.fontSize /= 1.1f; // ���������� ������� ������ ������
            selectedText.color = Color.black; // ������ ���� ������ �������
            // ����� ����� �������� �������, ���� ����������� ��� ����� Material ��� ����������� ������
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
