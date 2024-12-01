using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
   
    public TextMeshProUGUI coinsText; // Текст для отображения количества монет
    public Inventory inventory; // Ссылка на инвентарь

    public GameObject slotPrefab; // Префаб слота инвентаря
    public Transform slotsParent;  // Родительский объект для слотов

    void Start()
    {
        // Создание слотов в инвентаре
        CreateInventorySlots();
    }

    void CreateInventorySlots()
    {
        // Очистка предыдущих слотов
        foreach (Transform child in slotsParent)
        {
            Destroy(child.gameObject);
        }

        // Создание слотов
        for (int x = 0; x < inventory.width; x++)
        {
            for (int y = 0; y < inventory.height; y++)
            {
                GameObject slot = Instantiate(slotPrefab, slotsParent);
                // Установка свойства или событий для слота
                Button slotButton = slot.GetComponent<Button>();
                int ix = x;
                int iy = y;

                // Пример нажатия на слот
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
            // Здесь вы можете добавить логику для отображения информации о предмете
        }
    }


}
