using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Тестовый
//Класс который содержит данные, которые мы хотим сохранить
[System.Serializable]
public class GameData : MonoBehaviour
{
    public string playerName;
    public int playerLevel;
    public static float playerHealth;
    public static Inventory inventory;


    // Добавь другие необходимые данные
}
