using Assets.ServiceLocator;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TreasureChest : MonoBehaviour
{
    public GameObject lootPrefab; // Префаб выпадаемого предмета
    public List<GameObject> possibleLootItems; // Список возможных предметов для выпадения
    public float interactionDistance = 3f; // Дистанция для взаимодействия
    public TextMeshProUGUI interactionText; // Текст для отображения подсказки
    public EventHandler ChestIsOpen; // Событие открытие сундука

    private bool isOpen = false; // Статус открытия сундука
    private Transform playerTransform; // Ссылка на игрока
    private Player _player;

    private IGameInput _gameInput;

    // Тестируем ZENJECT
    //[Inject]
    //private void Construct(Player player)
    //{ 
    //    _player = player; 
    //    Debug.Log("Что то произошло!!!!!!!!! "+_player.transform.position+"  чему то равен");
    //}


    private void Start()
    {
        _gameInput =ServiceLocator.GetService<IGameInput>();
        _gameInput.OnPlayerOpen += Player_OnPlayerOpen;
        playerTransform = Player.Instance.transform; // Получаем ссылку на игрока
        HideInteractionPrompt(); // Скрываем подсказку в начале
    }

    private void Player_OnPlayerOpen(object sender, System.EventArgs e)
    {
        if (isOpen) return;

        if (Vector3.Distance(playerTransform.position, transform.position) < interactionDistance)
        {
            // Отображаем клавишу нажатия
            ShowInteractionPrompt();
            OpenChest();

            ChestIsOpen?.Invoke(this, EventArgs.Empty);
        }
    }
    private void Update()
    {
        // Проверяем расстояние до игрока каждую кадр
        if (!isOpen && Vector3.Distance(playerTransform.position, transform.position) < interactionDistance)
        {
            ShowInteractionPrompt(); // Отображаем подсказку
        }
        else
        {
            HideInteractionPrompt(); // Скрываем подсказку, если игрок далеко
        }
    }

    private void OpenChest()
    {
        isOpen = true; // Устанавливаем статус на открытый
        HideInteractionPrompt(); // Скрываем подсказку при открытии

        // Логика выпадения предметов
        DropLoot();

        // Здесь можно добавить анимацию открытия сундука или детали
        Debug.Log("Chest opened!");
    }

    private void DropLoot()
    {
        foreach (GameObject item in possibleLootItems)
        {
            // Случайное выпадение предмета (например, 50% шанс)
            if (UnityEngine.Random.value > 0.1f)
            {
                Vector3 dropPosition = GetRandomDropPosition(); // Получаем случайную позицию для выпадения предмета
                                                                
                // Проверяем, доступно ли это место
                if (IsPositionAvailable(dropPosition))
                {
                    Instantiate(item, dropPosition, Quaternion.identity); // Создаем предмет в случайной позиции
                    Debug.Log($"Dropped: {item.name} at {dropPosition}");
                }
                else
                {
                    Debug.Log($"Position {dropPosition} is not available.");
                }
            }
        }
    }
    private bool IsPositionAvailable(Vector3 position)
    {
        // Проверка доступности позиции с помощью OverlapCircle
        //float checkRadius = 0.5f; // Радиус проверки (в зависимости от размера объекта)
        //int layerMask = LayerMask.GetMask("Default"); // Укажите здесь нужный слой, если требуется

        // Возвращаем true, если область свободна (нет коллайдеров), иначе false
        // return !Physics2D.OverlapCircle(position, checkRadius, layerMask);

        //заглушка
        return true;
    }

    private Vector3 GetRandomDropPosition()
    {
        // Определяем радиус выпадения предметов
        float dropRadius = 1.5f; // Задайте желаемый радиус

        // Генерируем случайные координаты по осям X и Y в пределах заданного радиуса
        float randomX = UnityEngine.Random.Range(-dropRadius, dropRadius);
        float randomY = UnityEngine.Random.Range(-dropRadius, dropRadius);

        // Возвращаем позицию с фиксированным Z (например, 0)
        return new Vector3(transform.position.x + randomX, transform.position.y + randomY, transform.position.z);
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
        _gameInput.OnPlayerOpen -= Player_OnPlayerOpen;
    }
}
