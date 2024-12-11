using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Scripts.Items;
using TMPro;

public class StorageChest : MonoBehaviour
{
    public EventHandler ChestOpened; // Событие открытия сундука
    public StorageInventory chestInventory; // Инвентарь сундука

    private bool isOpen = false; // Статус открытия сундука
    public float interactionDistance = 3f; // Дистанция для взаимодействия
    public TextMeshProUGUI interactionText; // Текст для отображения подсказки

    public int inventoryWidth = 6; // Ширина инвентаря
    public int inventoryHeight = 6; // Высота инвентаря

    private void Start()
    {
        //GameObject chestObject = new GameObject("ChestInventory");
        // Добавьте компонент "Inventory" к созданному объекту
        //chestInventory = chestObject.AddComponent<Inventory>();

        chestInventory.width = inventoryWidth; // Устанавливаем ширину
        chestInventory.height = inventoryHeight; // Устанавливаем высоту
        chestInventory.coins = 0; // Инициализируем монеты

        GameInput.Instance.OnPlayerOpen += PlayerOnPlayerOpen;
        HideInteractionPrompt(); // Скрываем подсказку в начале
    }
    private void Update()
    {
        // Проверяем расстояние до игрока каждую кадр
        if (!isOpen && Vector3.Distance(Player.Instance.transform.position, transform.position) < interactionDistance)
        {
            ShowInteractionPrompt(); // Отображаем подсказку
        }
        else
        {
            HideInteractionPrompt(); // Скрываем подсказку, если игрок далеко
        }
    }
    private void PlayerOnPlayerOpen(object sender, EventArgs e)
    {
        if (isOpen) return;

        if (Vector3.Distance(Player.Instance.transform.position, transform.position) < interactionDistance)
        {
            // Отображаем клавишу нажатия
            ShowInteractionPrompt();
            OpenChest();

        }
    }

    public void OpenChest()
    {
       // isOpen = true; // Устанавливаем статус на открытый
        HideInteractionPrompt(); // Скрываем подсказку при открытии

         ChestOpened?.Invoke(this, EventArgs.Empty);
        // Открытие инвентаря сундука через GUI_Manager
        GUIManager.Instance.OpenStorageChestInventory(chestInventory); // Открываем инвентарь через GUI_Manager
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
    private void ShowInteractionPrompt()
    {
        interactionText.text = "Press 'E' to open the chest"; // Отображение подсказки
        interactionText.gameObject.SetActive(true); // Включаем текст
                                                    // UpdatePromptPosition(); // Обновляем позицию подсказки
    }
    private void HideInteractionPrompt()
    {
        interactionText.gameObject.SetActive(false); // Скрываем текст
    }

    private void UpdatePromptPosition()
    {
        // Позиция текста над сундуком
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up); // Поднимаем текст над сундуком
        interactionText.transform.position = screenPos; // Устанавливаем позицию на экране
    }

    private void OnDestroy() // Отписываемся от события при уничтожении объекта
    {
        GameInput.Instance.OnPlayerOpen -= PlayerOnPlayerOpen;
    }

}
