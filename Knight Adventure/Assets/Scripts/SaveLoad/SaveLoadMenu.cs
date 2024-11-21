using UnityEngine;
using System.IO;
using UnityEngine.UI;
using Assets.Scripts;
using TMPro;

public class SaveLoadMenu : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform contentPanel;
    private SaveManager _saveManager;

    private void Start()
    {
        _saveManager=FindObjectOfType<SaveManager>();
        if (_saveManager != null && !string.IsNullOrEmpty(_saveManager.saveDirectory))
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
        }
    }

    private void LoadGame(string fileName)
    {
        User loadUser = _saveManager.LoadGame(fileName);
        if (loadUser != null)
        {
            Debug.Log("Load player: " + loadUser.GetName());
        }
    }
}
