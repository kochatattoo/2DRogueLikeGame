using Assets.Scripts;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public string saveDirectory;
    public static SaveManager Instance;

    private void Start()
    {
        Instance = this;
        saveDirectory=Application.persistentDataPath+"/saves/";

        if(!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
        }
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

    //Надо почистить метод сохранения
    public static void SaveUser(User user)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/user.data";
        FileStream stream = new FileStream(path, FileMode.Create);
        
        formatter.Serialize(stream, GameManager.Instance.user);
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
}
