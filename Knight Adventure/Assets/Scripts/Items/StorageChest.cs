using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Scripts.Items;

public class StorageChest : MonoBehaviour
{
    public EventHandler ChestOpened; // Событие открытия сундука
    public Inventory chestInventory; // Инвентарь сундука

    public int inventoryWidth = 4; // Ширина инвентаря
    public int inventoryHeight = 3; // Высота инвентаря

    private void Start()
    {
        chestInventory = new StorageInventory();
        chestInventory.width = inventoryWidth; // Устанавливаем ширину
        chestInventory.height = inventoryHeight; // Устанавливаем высоту
        chestInventory.coins = 0; // Инициализируем монеты
    }

    public void OpenChest()
    {
        ChestOpened?.Invoke(this, EventArgs.Empty);
        // Здесь можно добавить код для отображения UI сундука
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
