using Assets.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Тестовый
//Класс который содержит данные, которые мы хотим сохранить
//Пока нигде не используется 

// Сделать как описание данных мира
[System.Serializable]
public class GameData : MonoBehaviour
{
    public List<ObjectData> objectsData; // Состояние всех объектов
    public PlayerData playerData; // Состояние игрока (например, здоровье, уровень и т.д.)
}

[System.Serializable]
public class ObjectData
{
    public string objectName; // Название объекта
    public Vector3 position; // Позиция объекта
    public Quaternion rotation; // Поворот объекта
    public bool isActive; // Активен ли объект
}
