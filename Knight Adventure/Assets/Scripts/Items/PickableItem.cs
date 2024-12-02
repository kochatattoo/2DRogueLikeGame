using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : MonoBehaviour
{
    public Item item; // Ссылка на наш предмет

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Inventory playerInventory = collision.GetComponent<Inventory>();
            //InventoryUI inventoryUI = collision.GetComponent<InventoryUI>(); // Получаем ссылку на инвентарь UI

            if (playerInventory != null && playerInventory.AddItem(item))
            {
                //inventoryUI.UpdateInventoryUI(); // Обновляем UI инвентаря
                Debug.Log($"Picked up {item.itemName}");
                Destroy(gameObject); // Удаляем предмет после подбора
            }
            else
            {
                Debug.Log("Not enough space in inventory!");
            }
        }
    }
}
