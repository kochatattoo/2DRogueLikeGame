using UnityEngine;
using Assets.Scripts.Interfaces;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class MapManager : MonoBehaviour, IMapManager
{
    public static MapManager Instance {  get; private set; }

    public GameObject[] maps; // Список тайловых карт
    public GameObject currentMap; // Текущая карта
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
        //LoadMapFromResources();
        //LoadMap(currentMapIndex);
    }
    public void StartManager()
    {
        LoadMapFromResources();
        LoadMap(currentMapIndex);
    }
    // Метод для загрузки карты
    public void LoadMap(int index)
    {
        if (index < 0 || index >= maps.Length)
            return;

        // Удаление текущей карты, если она существует
        if (currentMap != null)
        {
            Destroy(currentMap); // Удаляем старую карту
        }

        // Загружаем новую карту
        currentMap = Instantiate(maps[index]); // Создаём новый экземпляр карты
        currentMap.transform.position = Player.Instance.transform.position; // Устанавливаем нужную позицию, если необходимо
    }

    // Метод для переключения на следующую карту
    public void SwitchToNextMap()
    {
        currentMapIndex = (currentMapIndex + 1) % maps.Length;
        LoadMap(currentMapIndex);
    }

    // Метод для переключения на предыдущую карту
    public void SwitchToPreviousMap()
    {
        currentMapIndex=(currentMapIndex - 1 + maps.Length) % maps.Length;
        LoadMap(currentMapIndex);
    }

    private void LoadMapFromResources()
    {
        ResourcesLoadManager resourcesLoadManager = gameObject.AddComponent<ResourcesLoadManager>();

        maps[0]=resourcesLoadManager.LoadMap("Taverna");
        maps[1] = resourcesLoadManager.LoadMap("Map_1");
    }

    //// Метод для сохранения состояния персонажа
    //private void SavePlayerState()
    //{
    //    //Player player = FindObjectOfType<Player>();

    //    Player player = Player.Instance;

    //    //Inventory playerInventory = FindObjectOfType<Inventory>();  
    //    Inventory playerInventory = Player.Instance.playerInventory;

    //    // Сохраняем здоровье
    //    GameData.playerHealth = player.GetCurrentHealth(); // Например, используйте свой класс GameData
    //    GameData.inventory = playerInventory; // Предполагается, что Inventory доступен
    //}

    //// Метод для восстановления состояния персонажа
    //private void RestorePlayerState()
    //{
    //   // Player player = FindObjectOfType<Player>();

    //    Player player = Player.Instance;

    //    //Inventory playerInventory = FindObjectOfType<Inventory>();
    //    Inventory playerInventory = Player.Instance.playerInventory;

    //    // Восстанавливаем здоровье
    //    player.SetCurrentHealth(GameData.playerHealth);
    //    // Здесь также можете восстановить инвентарь, если у вас есть такая логика
    //    if (GameData.inventory != null)
    //    {
    //        playerInventory = GameData.inventory; // Восстановите инвентарь
    //                                                 // Например, вызовите метод обновления UI инвентаря, если это необходимо
    //       // playerInventory.UpdateInventoryUI(); // Настройте этот метод в вашем классе Inventory
    //    }
    //}
}
