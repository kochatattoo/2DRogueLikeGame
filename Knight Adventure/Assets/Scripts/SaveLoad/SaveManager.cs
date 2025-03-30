using Assets.Scripts.Interfaces;
using Assets.ServiceLocator;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour, IManager, ISaveManager
{
    public string saveDirectory {  get; private set; }
    public string GetSaveDirectory() => saveDirectory;


    public void StartManager()
    {
        saveDirectory = Application.persistentDataPath + "/saves/";

        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
        }
    }
    public void SaveGame(PlayerData playerData, string fileName)
    {
        string json =JsonUtility.ToJson(playerData);
        File.WriteAllText(saveDirectory+fileName+".json", json);
        Debug.Log("Game saved to "+saveDirectory+fileName+".json");
    }

    public void QuickSaveGame(PlayerData playerData)
    {
        string fileName;
        if(GetLastSaveFileName()==null)
        {
            Debug.Log("Save fille no exist!");
            fileName = "QuickSaved";
        }
        else
        {
            fileName=GetLastSaveFileName();
            DeleteSaveFile(GetLastSaveFileName());
            SaveGame(playerData, fileName);
        }
    }

    public PlayerData LoadGame(string fileName)
    {
        string path = saveDirectory + fileName + ".json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerData playerData = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log("Game loaded from " + path);
            return playerData;
        }
        else
        {
            var notificationManager = ServiceLocator.GetService<INotificationManager>();
            notificationManager.OpenNotificationWindow("Error");

            Debug.LogError("Save file not found at " + path);
            return null;
        }
    }
    
    public PlayerData LoadLastGame()
    {
        string lastFileName = GetLastSaveFileName(); // �������� ��� ���������� ������������ �����
        if (lastFileName != null)
        {
            Debug.Log("Load complete");
            return LoadGame(lastFileName); // ��������� ���� �� ����� �����
        }
        Debug.LogError("No save files found."); // ���� ��� ������, ������� ������
        return null; // ���������� null, ���� �� ������� ���������
    }
    // ����� ��� ��������� ����� ���������� ������������ �����
    private string GetLastSaveFileName()
    {
        if (!Directory.Exists(saveDirectory))
        {
            return null; // ���� ���������� �� ����������, ���������� null
        }

        // �������� ��� ����� .json � ���������� � ��������� �� �� ������� ��������
        var files = Directory.GetFiles(saveDirectory, "*.json")
                             .Select(f => new FileInfo(f))
                             .OrderByDescending(fi => fi.LastWriteTime)
                             .ToList();

        return files.Count > 0 ? Path.GetFileNameWithoutExtension(files[0].FullName) : null; // ���������� ��� ���������� ����� ��� ����������
    }

    public List<string> GetSaveFileNames()
    {
        List<string> saveFileNames = new List<string>();
        string[] files = Directory.GetFiles(saveDirectory, "*.json");

        foreach (string file in files)
        {
            saveFileNames.Add(Path.GetFileNameWithoutExtension(file)); // ��������� ��� ����� ��� ����������
        }

        return saveFileNames;
    }

    // ����� ����� ��� �������� ����� ���������� �� �����
    public void DeleteSaveFile(string fileName)
    {
        string filePath = Path.Combine(saveDirectory, fileName + ".json");

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log($"{fileName}.json ��� ������!");
        }
        else
        {
            Debug.LogError($"���� ���������� {fileName}.json �� ������.");
        }
    }
}

