using Assets.Scripts;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public string saveDirectory;
    public static SaveManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // ��������� ������������� ����������
            DontDestroyOnLoad(gameObject); // �����������: ������������� ����������� ��� �������� ����� �����
        }
        else
        {
            Destroy(gameObject); // ������� �������������� ����������
        }

        saveDirectory =Application.persistentDataPath+"/saves/";

        if(!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
        }

        User.Instance=LoadLastGame();
    }

    public void SaveGame(User user, string fileName)
    {
        string json =JsonUtility.ToJson(user);
        File.WriteAllText(saveDirectory+fileName+".json", json);
        Debug.Log("Game saved to "+saveDirectory+fileName+".json");
    }

    public User LoadGame(string fileName)
    {
        string path = saveDirectory + fileName + ".json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            User user = JsonUtility.FromJson<User>(json);
            Debug.Log("Game loaded from " + path);
            return user;
        }
        else
        {
            Debug.LogError("Save file not found at " + path);
            return null;
        }
    }
    
    public User LoadLastGame()
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


    //���� ��������� ����� ����������
    //�� ����
    public static void SaveUser(User user)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/user.data";
        FileStream stream = new FileStream(path, FileMode.Create);
        
        formatter.Serialize(stream, User.Instance);
        stream.Close();
        Debug.Log("Save Complete");
    }
    public static User LoadUser()
    {
        string path = Application.persistentDataPath + "/user.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            User data = formatter.Deserialize(stream) as User;
            stream.Close();
            Debug.Log("Save has loaded");

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in" + path);
            return null;
        }
    }
    public static User ResetData()
    {
        User user = new User();
        string path = Application.persistentDataPath + "/user.data";

        if (File.Exists(path))
        {
           File.Delete(path);
            Debug.Log("Data reset complete");
            return user;
        }
        else
        {
            Debug.LogError("Save file not found in" + path);
            return null;
        }
    }
    //�������� �� ���� �������


    // ����� ����� ��� ��������� ������ ������ ����������
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

