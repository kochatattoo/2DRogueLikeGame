using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesLoadManager : MonoBehaviour
{
    // �������� ���� �� �����
    public GameObject LoadPlayerWindow(string windowName)
    {
        return LoadPrefab("Windows/Player_Windows_prefs/" + windowName);
    }
    // �������� ���������� ����
    public GameObject LoadStartScreenWindow(string startScreenName)
    {
        return LoadPrefab("Windows/StartScreenWindow/"+startScreenName);
    }
    // �������� ��������������� ���� �� �����
    public GameObject LoadInformationWindow(string infoWindowName)
    {
        return LoadPrefab("Windows/Information_Windows_prefs/" + infoWindowName);
    }

    // �������� ������������� (�������) ���� �� �����
    public GameObject LoadPriorityWindow(string priorityWindowName)
    {
        return LoadPrefab("Windows/Warning_Windows_prefs/" + priorityWindowName);
    }
    // �������� ������� ���� �������
    public GameObject LoadChestWindow(string chestWindowName)
    {
        return LoadPrefab("Windows/Chest_Windows_prefs/" + chestWindowName);
    }
    // �������� ����� ���� �� �����
    public GameObject LoadMap(string mapName)
    {
        return LoadPrefab("Maps/" + mapName);
    }

    // ����� ����� ��� �������� ��������
    private GameObject LoadPrefab(string resourcePath)
    {
        GameObject prefab = Resources.Load<GameObject>(resourcePath);
        if (prefab == null)
        {
            Debug.LogError($"������ �� ���� '{resourcePath}' �� ������.");
        }
        return prefab;
    }
}
