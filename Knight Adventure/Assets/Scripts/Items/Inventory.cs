using UnityEngine;

public class Inventory:MonoBehaviour
{
    public int width = 6;  // Ширина инвентаря
    public int height = 6; // Высота инвентаря
    private Item[,] items;  // Двумерный массив для хранения предметов

    public int coins; // Количество монет

    void Start()
    {
        items = new Item[width, height];
    }

    public bool AddItem(Item item)
    {
        // Ищем свободное место для предмета
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Проверяем, помещается ли предмет здесь
                if (CanPlaceItem(item, x, y))
                {
                    // Устанавливаем предмет в массив
                    for (int i = 0; i < item.width; i++)
                    {
                        for (int j = 0; j < item.height; j++)
                        {
                            items[x + i, y + j] = item;
                        }
                    }
                    return true;
                }
            }
        }
        return false; // Не удалось добавить предмет (недостаточно места)
    }

    private bool CanPlaceItem(Item item, int startX, int startY)
    {
        // Проверяем, помещается ли предмет в указанной позиции
        for (int i = 0; i < item.width; i++)
        {
            for (int j = 0; j < item.height; j++)
            {
                if (startX + i >= width || startY + j >= height || items[startX + i, startY + j] != null)
                {
                    return false; // Не помещается
                }
            }
        }
        return true; // Успешно помещается
    }

    public Item GetItem(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            return items[x, y];
        }
        return null;
    }

    public void RemoveItem(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            items[x, y] = null; // Удаление предмета
        }
    }
}
