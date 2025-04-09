using UnityEngine;
using Assets.Scripts.Interfaces;

public class MapManager : MonoBehaviour, IManager, IMapManager
{
 
    public GameObject[] maps; // ������ �������� ����
    public GameObject currentMap; // ������� �����
    private int currentMapIndex = 0; // ������ ������� �����

    public void StartManager()
    {
        gameObject.SetActive(true);
        LoadMapFromResources();
        LoadMap(currentMapIndex);
    }
    // ����� ��� �������� �����
    public void LoadMap(int index)
    {
        if (index < 0 || index >= maps.Length)
            return;

        // �������� ������� �����, ���� ��� ����������
        if (currentMap != null)
        {
            Destroy(currentMap); // ������� ������ �����
        }

        // ��������� ����� �����
        currentMap = Instantiate(maps[index]); // ������ ����� ��������� �����
       // currentMap.transform.position = Player.Instance.transform.position; // ������������� ������ �������, ���� ����������
    }

    // ����� ��� ������������ �� ��������� �����
    public void SwitchToNextMap()
    {
        currentMapIndex = (currentMapIndex + 1) % maps.Length;
        LoadMap(currentMapIndex);
    }

    // ����� ��� ������������ �� ���������� �����
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

    //// ����� ��� ���������� ��������� ���������
    //private void SavePlayerState()
    //{
    //    //Player player = FindObjectOfType<Player>();

    //    Player player = Player.Instance;

    //    //Inventory playerInventory = FindObjectOfType<Inventory>();  
    //    Inventory playerInventory = Player.Instance.playerInventory;

    //    // ��������� ��������
    //    GameData.playerHealth = player.GetCurrentHealth(); // ��������, ����������� ���� ����� GameData
    //    GameData.inventory = playerInventory; // ��������������, ��� Inventory ��������
    //}

    //// ����� ��� �������������� ��������� ���������
    //private void RestorePlayerState()
    //{
    //   // Player player = FindObjectOfType<Player>();

    //    Player player = Player.Instance;

    //    //Inventory playerInventory = FindObjectOfType<Inventory>();
    //    Inventory playerInventory = Player.Instance.playerInventory;

    //    // ��������������� ��������
    //    player.SetCurrentHealth(GameData.playerHealth);
    //    // ����� ����� ������ ������������ ���������, ���� � ��� ���� ����� ������
    //    if (GameData.inventory != null)
    //    {
    //        playerInventory = GameData.inventory; // ������������ ���������
    //                                                 // ��������, �������� ����� ���������� UI ���������, ���� ��� ����������
    //       // playerInventory.UpdateInventoryUI(); // ��������� ���� ����� � ����� ������ Inventory
    //    }
    //}
}
