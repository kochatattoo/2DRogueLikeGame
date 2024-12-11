using Assets.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��������
//����� ������� �������� ������, ������� �� ����� ���������
//���� ����� �� ������������ 

// ������� ��� �������� ������ ����
[System.Serializable]
public class GameData : MonoBehaviour
{
    public List<ObjectData> objectsData; // ��������� ���� ��������
    public PlayerData playerData; // ��������� ������ (��������, ��������, ������� � �.�.)
}

[System.Serializable]
public class ObjectData
{
    public string objectName; // �������� �������
    public Vector3 position; // ������� �������
    public Quaternion rotation; // ������� �������
    public bool isActive; // ������� �� ������
}
