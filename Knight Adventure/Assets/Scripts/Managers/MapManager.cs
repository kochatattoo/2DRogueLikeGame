using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance {  get; private set; }

    public List<GameObject> maps; // ������ �������� ����
    private int currentMapIndex = 0; // ������ ������� �����

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �����������: ������������� ����������� ��� �������� ����� �����
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

    // ����� ��� �������� �����
    public void LoadMap(int index)
    {
        if (index < 0 || index >= maps.Count)
            return;

        // ������ ��� �����
        foreach (var map in maps)
        {
            map.gameObject.SetActive(false);
        }

        // ���������� ��������� �����
        maps[index].gameObject.SetActive(true);
        currentMapIndex = index;
    }

    // ����� ��� ������������ �� ��������� �����
    public void SwitchToNextMap()
    {
        LoadMap((currentMapIndex + 1) % maps.Count);
    }

    // ����� ��� ������������ �� ���������� �����
    public void SwitchToPreviousMap()
    {
        LoadMap((currentMapIndex - 1 + maps.Count) % maps.Count);
    }
}
