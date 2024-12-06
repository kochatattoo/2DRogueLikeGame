using Assets.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��������
//����� ������� �������� ������, ������� �� ����� ���������
[System.Serializable]
public class GameData : MonoBehaviour
{
    public string playerName;
    public int playerLevel;
    public static float playerHealth;
    public static Inventory inventory;
    public static PlayerStats stats;
    public static PlayerAchievements playerAchievements;

    // ������ ������ ����������� ������
}
