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
    public GameObject LoadPauseMenuWindow(string windowName)
    {
        return LoadPrefab("Windows/PauseMenu" + windowName);
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

    public GameObject LoadManager(string managerName)
    {
        return LoadPrefab("Managers/"+managerName);
    }

    // ����� ����� ��� �������� ��������
    public GameObject LoadPrefab(string resourcePath)
    {
        GameObject prefab = Resources.Load<GameObject>(resourcePath);
        if (prefab == null)
        {
            Debug.LogError($"������ �� ���� '{resourcePath}' �� ������.");
        }
        return prefab;
    }
    public AudioClip LoadPlayerAudioClips(string audioClipsName)
    {
        return LoadAudioClip("Audio_Resources/Player_Audio_Clips/" + audioClipsName);
    }
    public AudioClip LoadEnemyAudioClips(string audioClipsName)
    {
        return LoadAudioClip("Audio_Resources/Enemies_Audio_Clips/" + audioClipsName);
    }
    public AudioClip LoadNotificationAudioClips(string audioClipsName)
    {
        return LoadAudioClip("Audio_Resources/Notification_Audio_Clips/" + audioClipsName);
    }
    public AudioClip LoadUIEffectClips(string audioClipsName)
    {
        return LoadAudioClip("Audio_Resources/UI_Effects_Audio_Clips/" + audioClipsName);
    }
    public AudioClip LoadAudioClip(string audioClipName)
    {
        AudioClip audioClip = Resources.Load<AudioClip>(audioClipName);
        if (audioClip == null)
        {
            Debug.LogError($"������ �� ���� '{audioClipName}' �� ������.");
        }
        return audioClip;
    }
}
