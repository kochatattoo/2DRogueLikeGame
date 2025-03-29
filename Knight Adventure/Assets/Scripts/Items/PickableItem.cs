using UnityEngine;

public class PickableItem : MonoBehaviour
{
    public Item item; // Ссылка на наш предмет
    private bool _canPick=true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && _canPick)
        {
            Inventory playerInventory = collision.GetComponent<Inventory>();
            InventoryUI inventoryUI = FindObjectOfType<InventoryUI>(); // Получаем ссылку на инвентарь UI

            _canPick = false; // Флаг, что бы предотвратить повторное срабатывание
           

            if (playerInventory != null && playerInventory.AddItem(item))
            {
                if (inventoryUI != null)
                {
                    inventoryUI.UpdateInventoryUI(); // Обновляем UI инвентаря
                }
                
                Debug.Log($"Picked up {item.itemName}");
                Destroy(gameObject); // Удаляем предмет после подбора
            }
            else
            {
                Debug.Log("Not enough space in inventory!");
                _canPick=true;
            }
        }
    }
}
