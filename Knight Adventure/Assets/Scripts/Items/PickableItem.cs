using UnityEngine;

public class PickableItem : MonoBehaviour
{
    public Item item; // ������ �� ��� �������
    private bool _canPick=true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && _canPick)
        {
            Inventory playerInventory = collision.GetComponent<Inventory>();
            InventoryUI inventoryUI = FindObjectOfType<InventoryUI>(); // �������� ������ �� ��������� UI

            _canPick = false; // ����, ��� �� ������������� ��������� ������������
           

            if (playerInventory != null && playerInventory.AddItem(item))
            {
                if (inventoryUI != null)
                {
                    inventoryUI.UpdateInventoryUI(); // ��������� UI ���������
                }
                
                Debug.Log($"Picked up {item.itemName}");
                Destroy(gameObject); // ������� ������� ����� �������
            }
            else
            {
                Debug.Log("Not enough space in inventory!");
                _canPick=true;
            }
        }
    }
}
