using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//НИГДЕ НЕ ИСПОЛЬЗУЕТСЯ

[System.Serializable]
public class CharacterState
{
    public string characterType; // Тип персонажа (Player или Enemy)
    public Vector3 position; // Позиция на карте
}

[System.Serializable]
public class MapState
{
    public List<CharacterState> characterStates = new List<CharacterState>(); // Позиции всех персонажей на карте
}

public class SaveMapState : MonoBehaviour
{
    public void saveMapState()
    {

        MapState state = new MapState();

        //Пример: получаем все персонажи на текущей карте
        foreach (var player in FindObjectsOfType<Player>())
        {
            if (player.gameObject.activeInHierarchy) // Проверяем что объект активен
            {
                CharacterState characterState = new CharacterState
                {
                    characterType = "Player", // Указываем тип
                    position = player.transform.position // Сохраняем позицию
                };
                state.characterStates.Add(characterState);
            }
        }

        // Сохраняем позиции всех врагов на карте
        foreach (var enemy in FindObjectsOfType<EnemyEntity>())
        {
            if (enemy.gameObject.activeInHierarchy) // Проверяем, что объект активен
            {
                CharacterState characterState = new CharacterState
                {
                    characterType = "Enemy", // Указываем тип
                    position = enemy.transform.position // Сохраняем позицию
                };
                state.characterStates.Add(characterState);
            }
        }
        // Сохраняем состояние как JSON в PlayerPrefs
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

            // Очистка текущих объектов на карте перед загрузкой нового состояния
            foreach (var player in FindObjectsOfType<Player>())
            {
                Destroy(player.gameObject);
            }

            foreach (var enemy in FindObjectsOfType<EnemyEntity>())
            {
                Destroy(enemy.gameObject);
            }

            // Загружаем позиции для каждого персонажа
            foreach (var characterState in state.characterStates)
            {
                if (characterState.characterType == "Player")
                {
                    GameObject playerPrefab = Resources.Load<GameObject>("PlayerPrefab"); // Замените на путь к вашему префабу
                    if (playerPrefab != null)
                    {
                        Player player = Instantiate(playerPrefab, characterState.position, Quaternion.identity).GetComponent<Player>();
                        // Здесь можно присвоить дополнительные параметры, если это необходимо
                    }
                }
                else if (characterState.characterType == "Enemy")
                {
                    GameObject enemyPrefab = Resources.Load<GameObject>("EnemyPrefab"); // Замените на путь к вашему префабу
                    if (enemyPrefab != null)
                    {
                        EnemyEntity enemy = Instantiate(enemyPrefab, characterState.position, Quaternion.identity).GetComponent<EnemyEntity>();
                        // Здесь можно присвоить дополнительные параметры, если это необходимо
                    }
                }
            }
        }
    }
}
