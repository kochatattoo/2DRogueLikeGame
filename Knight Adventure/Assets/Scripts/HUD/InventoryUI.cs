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

    void Start()
    {
        // �������� ������ � ���������
        CreateInventorySlots();
    }

    void CreateInventorySlots()
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

    void OnSlotClicked(int x, int y)
    {
        Item item = inventory.GetItem(x, y);
        if (item != null)
        {
            Debug.Log($"Slot {x},{y} clicked! Item: {item.itemName}");
            // ����� �� ������ �������� ������ ��� ����������� ���������� � ��������
        }
    }


}
