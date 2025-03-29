using UnityEngine;

public class Inventory:MonoBehaviour
{
    public int width = 6;  // ������ ���������
    public int height = 6; // ������ ���������
    private Item[,] items;  // ��������� ������ ��� �������� ���������

    public int coins; // ���������� �����

    void Start()
    {
        items = new Item[width, height];
    }

    public bool AddItem(Item item)
    {
        // ���� ��������� ����� ��� ��������
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // ���������, ���������� �� ������� �����
                if (CanPlaceItem(item, x, y))
                {
                    // ������������� ������� � ������
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
        return false; // �� ������� �������� ������� (������������ �����)
    }

    private bool CanPlaceItem(Item item, int startX, int startY)
    {
        // ���������, ���������� �� ������� � ��������� �������
        for (int i = 0; i < item.width; i++)
        {
            for (int j = 0; j < item.height; j++)
            {
                if (startX + i >= width || startY + j >= height || items[startX + i, startY + j] != null)
                {
                    return false; // �� ����������
                }
            }
        }
        return true; // ������� ����������
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
            items[x, y] = null; // �������� ��������
        }
    }
}
