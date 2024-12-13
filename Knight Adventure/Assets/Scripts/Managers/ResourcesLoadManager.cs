using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesLoadManager : MonoBehaviour
{
    // Загрузка окна по имени
    public GameObject LoadPlayerWindow(string windowName)
    {
        return LoadPrefab("Windows/Player_Windows_prefs/" + windowName);
    }
    // Загрузка стартового окна
    public GameObject LoadStartScreenWindow(string startScreenName)
    {
        return LoadPrefab("Windows/StartScreenWindow/"+startScreenName);
    }
    // Загрузка информационного окна по имени
    public GameObject LoadInformationWindow(string infoWindowName)
    {
        return LoadPrefab("Windows/Information_Windows_prefs/" + infoWindowName);
    }

    // Загрузка приоритетного (важного) окна по имени
    public GameObject LoadPriorityWindow(string priorityWindowName)
    {
        return LoadPrefab("Windows/Warning_Windows_prefs/" + priorityWindowName);
    }
    // Загрузка префаба окна сундука
    public GameObject LoadChestWindow(string chestWindowName)
    {
        return LoadPrefab("Windows/Chest_Windows_prefs/" + chestWindowName);
    }
    // Загрузка карты игры по имени
    public GameObject LoadMap(string mapName)
    {
        return LoadPrefab("Maps/" + mapName);
    }

    // Общий метод для загрузки префабов
    private GameObject LoadPrefab(string resourcePath)
    {
        GameObject prefab = Resources.Load<GameObject>(resourcePath);
        if (prefab == null)
        {
            Debug.LogError($"Префаб по пути '{resourcePath}' не найден.");
        }
        return prefab;
    }
}
