using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : MonoBehaviour
{
    public Item item; // ������ �� ��� �������

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Inventory playerInventory = collision.GetComponent<Inventory>();
            //InventoryUI inventoryUI = collision.GetComponent<InventoryUI>(); // �������� ������ �� ��������� UI

            if (playerInventory != null && playerInventory.AddItem(item))
            {
                //inventoryUI.UpdateInventoryUI(); // ��������� UI ���������
                Debug.Log($"Picked up {item.itemName}");
                Destroy(gameObject); // ������� ������� ����� �������
            }
            else
            {
                Debug.Log("Not enough space in inventory!");
            }
        }
    }
}
