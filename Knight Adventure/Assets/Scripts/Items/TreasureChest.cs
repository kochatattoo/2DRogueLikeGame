using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TreasureChest : MonoBehaviour
{
    public GameObject lootPrefab; // Префаб выпадаемого предмета
    public List<GameObject> possibleLootItems; // Список возможных предметов для выпадения
    public float interactionDistance = 3f; // Дистанция для взаимодействия
    public TextMeshProUGUI interactionText; // Текст для отображения подсказки

    private bool isOpen = false; // Статус открытия сундука
    private Transform playerTransform; // Ссылка на игрока

    private void Start()
    {
        GameInput.Instance.OnPlayerOpen += Player_OnPlayerOpen;
        playerTransform = Player.Instance.transform; // Получаем ссылку на игрока
        HideInteractionPrompt(); // Скрываем подсказку в начале
    }

    private void Player_OnPlayerOpen(object sender, System.EventArgs e)
    {
        if (isOpen) return;

        if (Vector3.Distance(Player.Instance.transform.position, transform.position) < interactionDistance)
        {
            // Отображаем клавишу нажатия
            ShowInteractionPrompt();
            OpenChest();
           
        }
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
            if (Random.value > 0.5f)
            {
                Instantiate(item, lootPrefab.transform.position, Quaternion.identity);
                Debug.Log($"Dropped: {item.name}");
            }
        }
    }
    private void ShowInteractionPrompt()
    {
        interactionText.text = "Press 'E' to open the chest"; // Отображение подсказки
        interactionText.gameObject.SetActive(true); // Включаем текст
        UpdatePromptPosition(); // Обновляем позицию подсказки
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
        GameInput.Instance.OnPlayerOpen -= Player_OnPlayerOpen;
    }
}
