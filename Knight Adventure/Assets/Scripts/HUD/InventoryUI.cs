using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
   
    public TextMeshProUGUI coinsText; // ����� ��� ����������� ���������� �����
    public Inventory inventory; // ������ �� ���������
    public GameObject slotPrefab; // ������ ����� ���������
    public Transform slotsParent;  // ������������ ������ ��� ������

    private void Start()
    {
        // ������� �������� ��������� Inventory �� ������� Player
        if (inventory == null)
        {
            inventory = FindObjectOfType<Player>().GetComponent<Inventory>();
            if (inventory == null)
            {
                Debug.LogError("Inventory component not found on Player!");
            }
        }
        // �������� ������ � ���������
        CreateInventorySlots();
        UpdateInventoryUI(); // ��������� ��������� ��� ������
    }

    public void UpdateInventoryUI()
    {
        // ������� ������ �������� ���������
        foreach (Transform child in slotsParent)
        {
            Destroy(child.gameObject);
        }

        // ��������� ����� ����� (���� ����)
        coinsText.text = "Coins: " + inventory.coins;

        // ������� ����� �������� ��� ������� �������� � ���������
        for (int x = 0; x < inventory.width; x++)
        {
            for (int y = 0; y < inventory.height; y++)
            {
                Item item = inventory.GetItem(x, y);
                if (item != null)
                {
                    GameObject slot = Instantiate(slotPrefab, slotsParent);
                    slot.GetComponent<Image>().sprite = item.itemSprite; // ��������� ������� ��������

                    // ���� �����, ���������� ����� ��� ������ ����������
                   // slot.transform.Find("ItemName").GetComponent<Text>().text = item.itemName; // ���������� ��� ��������
                   // slot.transform.Find("ItemQuantity").GetComponent<Text>().text = item.quantity.ToString(); // ���������� ���������� ���������
                }
            }
        }
    }
    private void CreateInventorySlots()
    {
        // ������� ���������� ������
        foreach (Transform child in slotsParent)
        {
            Destroy(child.gameObject);
        }

        // �������� ������
        for (int x = 0; x < inventory.width; x++)
        {
            for (int y = 0; y < inventory.height; y++)
            {
                GameObject slot = Instantiate(slotPrefab, slotsParent);
                // ��������� �������� ��� ������� ��� �����
                Button slotButton = slot.GetComponent<Button>();
                int ix = x;
                int iy = y;

                // ������ ������� �� ����
                slotButton.onClick.AddListener(() => OnSlotClicked(ix, iy));
            }
        }
    }

    private void OnSlotClicked(int x, int y)
    {
        Item item = inventory.GetItem(x, y);
        if (item != null)
        {
            Debug.Log($"Slot {x},{y} clicked! Item: {item.itemName}");
            // ����� �� ������ �������� ������ ��� ����������� ���������� � ��������
        }
    }


}