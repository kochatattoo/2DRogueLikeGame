using Assets.Scripts;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.U2D;

[System.Serializable]
public class Item
{
    public string itemName; // Название предмета
    public string itemDescription; // Описание предмета
    public int itemID; // Уникальный идентификатор предмета
    public int quantity; // Количество предметов (если можно иметь несколько одинаковых)
    public Sprite itemSprite; // Изображение предмета для UI
    public int width; // Ширина предмета в сетке
    public int height; // Высота предмета в сетке

    public Item(string name, string description ,
        int id, Sprite sprite,
        int quantity=1, int width=1, int height=1)
    {

        itemName = name;
        itemDescription = description;
        itemID = id;
        this.quantity = quantity;
        itemSprite = sprite;
        this.width = width;
        this.height = height;
    }
}
