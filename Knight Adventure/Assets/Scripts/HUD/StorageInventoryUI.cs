using UnityEngine;
using UnityEngine.UI;

public class StorageInventoryUI : InventoryUI
{
    public StorageChest storageChest; // Привязка к сундуку

    private void Start()
    {
        // Подписка на событие открытия сундука
        storageChest.ChestOpened += OnChestOpened;
        // Создание слотов и обновление интерфейса при старте
        CreateInventorySlots();
        UpdateInventoryUI();
    }

    private void OnChestOpened(object sender, System.EventArgs e)
    {
        // Обновляем интерфейс при открытии сундука
        UpdateInventoryUI();
    }

    // Переопределяем UpdateInventoryUI для отображения инвентаря сундука и обновления специфических элементов
    public override void UpdateInventoryUI()
    {
        base.UpdateInventoryUI(); // Вызываем метод родительского класса для обновления общего интерфейса

        // Здесь можно добавить дополнительные изменения или улучшения для конкретного инвентаря сундука, если это необходимо
    }

    // Метод, который может быть расширен для других нужд, например, выбора предмета
    protected override void OnSlotClicked(int x, int y)
    {
        base.OnSlotClicked(x, y); // Вызываем метод родительского класса для обработки клика на слоте
        Item item = inventory.GetItem(x, y);

        if (item != null)
        {
            Debug.Log($"Chest slot {x},{y} clicked! Item: {item.itemName}");
            // Здесь можно добавить дополнительную логику, например, перемещение предмета в инвентарь игрока
        }
    }
}


