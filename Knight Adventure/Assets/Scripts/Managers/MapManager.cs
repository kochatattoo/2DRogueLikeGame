using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance {  get; private set; }

    public List<GameObject> maps; // Список тайловых карт
    private int currentMapIndex = 0; // Индекс текущей карты

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Опционально: предотвратить уничтожение при загрузке новой сцены
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadMap(currentMapIndex);
    }

    // Метод для загрузки карты
    public void LoadMap(int index)
    {
        if (index < 0 || index >= maps.Count)
            return;

        // Скрыть все карты
        foreach (var map in maps)
        {
            map.gameObject.SetActive(false);
        }

        // Отображаем выбранную карту
        maps[index].gameObject.SetActive(true);
        currentMapIndex = index;
    }

    // Метод для переключения на следующую карту
    public void SwitchToNextMap()
    {
        LoadMap((currentMapIndex + 1) % maps.Count);
    }

    // Метод для переключения на предыдущую карту
    public void SwitchToPreviousMap()
    {
        LoadMap((currentMapIndex - 1 + maps.Count) % maps.Count);
    }
}
