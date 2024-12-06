using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Scripts.Items;

public class StorageChest : MonoBehaviour
{
    public EventHandler ChestOpened; // ������� �������� �������
    public Inventory chestInventory; // ��������� �������

    public int inventoryWidth = 4; // ������ ���������
    public int inventoryHeight = 3; // ������ ���������

    private void Start()
    {
        chestInventory = new StorageInventory();
        chestInventory.width = inventoryWidth; // ������������� ������
        chestInventory.height = inventoryHeight; // ������������� ������
        chestInventory.coins = 0; // �������������� ������
    }

    public void OpenChest()
    {
        ChestOpened?.Invoke(this, EventArgs.Empty);
        // ����� ����� �������� ��� ��� ����������� UI �������
    }

    public void AddItemToChest(Item item, int x, int y)
    {
        if (!chestInventory.AddItem(item))
        {
            Debug.Log("Not enough space in chest inventory!");
        }
    }

    public void RemoveItemFromChest(int x, int y)
    {
        chestInventory.RemoveItem(x, y);
    }

}
