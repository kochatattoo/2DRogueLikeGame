namespace Assets.Scripts.Items
{
    public class StorageInventory : Inventory
    {
        // Дополнительные функции или свойства для инвентаря сундука  добавлены здесь.

        // Пример: метод для получения предмета из инвентаря сундука
        public Item GetItemFromChest(int x, int y)
        {
            return GetItem(x, y); // Используем метод из родительского класса
        }

        // Метод для перемещения предмета из сундука в инвентарь игрока
        public bool MoveItemToPlayerInventory(Inventory targetInventory, int x, int y)
        {
            Item item = GetItem(x, y);
            if (item != null && targetInventory.AddItem(item))
            {
                RemoveItem(x, y); // Удаляем предмет из инвентаря сундука
                return true; // Успешно перемещен
            }
            return false; // Не удалось переместить
        }
    }
}
