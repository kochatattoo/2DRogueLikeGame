using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����� �� ������������

[System.Serializable]
public class CharacterState
{
    public string characterType; // ��� ��������� (Player ��� Enemy)
    public Vector3 position; // ������� �� �����
}

[System.Serializable]
public class MapState
{
    public List<CharacterState> characterStates = new List<CharacterState>(); // ������� ���� ���������� �� �����
}

public class SaveMapState : MonoBehaviour
{
    public void saveMapState()
    {

        MapState state = new MapState();

        //������: �������� ��� ��������� �� ������� �����
        foreach (var player in FindObjectsOfType<Player>())
        {
            if (player.gameObject.activeInHierarchy) // ��������� ��� ������ �������
            {
                CharacterState characterState = new CharacterState
                {
                    characterType = "Player", // ��������� ���
                    position = player.transform.position // ��������� �������
                };
                state.characterStates.Add(characterState);
            }
        }

        // ��������� ������� ���� ������ �� �����
        foreach (var enemy in FindObjectsOfType<EnemyEntity>())
        {
            if (enemy.gameObject.activeInHierarchy) // ���������, ��� ������ �������
            {
                CharacterState characterState = new CharacterState
                {
                    characterType = "Enemy", // ��������� ���
                    position = enemy.transform.position // ��������� �������
                };
                state.characterStates.Add(characterState);
            }
        }
        // ��������� ��������� ��� JSON � PlayerPrefs
        string json = JsonUtility.ToJson(state);
        PlayerPrefs.SetString("MapState", json);
        PlayerPrefs.Save();
    }
    public void LoadMapState()
    {
        string json = PlayerPrefs.GetString("MapState", "");
        if (!string.IsNullOrEmpty(json))
        {
            MapState state = JsonUtility.FromJson<MapState>(json);

            // ������� ������� �������� �� ����� ����� ��������� ������ ���������
            foreach (var player in FindObjectsOfType<Player>())
            {
                Destroy(player.gameObject);
            }

            foreach (var enemy in FindObjectsOfType<EnemyEntity>())
            {
                Destroy(enemy.gameObject);
            }

            // ��������� ������� ��� ������� ���������
            foreach (var characterState in state.characterStates)
            {
                if (characterState.characterType == "Player")
                {
                    GameObject playerPrefab = Resources.Load<GameObject>("PlayerPrefab"); // �������� �� ���� � ������ �������
                    if (playerPrefab != null)
                    {
                        Player player = Instantiate(playerPrefab, characterState.position, Quaternion.identity).GetComponent<Player>();
                        // ����� ����� ��������� �������������� ���������, ���� ��� ����������
                    }
                }
                else if (characterState.characterType == "Enemy")
                {
                    GameObject enemyPrefab = Resources.Load<GameObject>("EnemyPrefab"); // �������� �� ���� � ������ �������
                    if (enemyPrefab != null)
                    {
                        EnemyEntity enemy = Instantiate(enemyPrefab, characterState.position, Quaternion.identity).GetComponent<EnemyEntity>();
                        // ����� ����� ��������� �������������� ���������, ���� ��� ����������
                    }
                }
            }
        }
    }
}
