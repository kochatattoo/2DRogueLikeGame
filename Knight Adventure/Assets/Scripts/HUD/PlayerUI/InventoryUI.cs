using Assets.Scripts.Interfaces;
using Assets.ServiceLocator;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
   
    public TextMeshProUGUI coinsText; // Текст для отображения количества монет
    public Inventory inventory; // Ссылка на инвентарь
    public GameObject slotPrefab; // Префаб слота инвентаря
    public Transform slotsParent;  // Родительский объект для слотов
    

    private void Start()
    {
        // Попытка получить компонент Inventory из объекта Player
        if (inventory == null)
        {
            //inventory = FindObjectOfType<Player>().GetComponent<Inventory>();

            inventory = Player.Instance.playerInventory;
            if (inventory == null)
            {
                Debug.LogError("Inventory component not found on Player!");
                var notificationManager = ServiceLocator.GetService<INotificationManager>(); 
                notificationManager.HandleError("Произошла ошибка: Не найден инвентарь игрока.", 0);
            }
        }
        // Создание слотов в инвентаре
        CreateInventorySlots();
        UpdateInventoryUI(); // Обновляем интерфейс при старте
    }

    public virtual void UpdateInventoryUI()
    {
        // Удаляем старые элементы инвентаря
        foreach (Transform child in slotsParent)
        {
            Destroy(child.gameObject);
        }

        // Обновляем текст монет (если есть)
        coinsText.text = "Coins: " + inventory.coins;

        // Создаем новые элементы для каждого предмета в инвентаре
        for (int x = 0; x < inventory.width; x++)
        {
            for (int y = 0; y < inventory.height; y++)
            {
                Item item = inventory.GetItem(x, y);
                if (item != null)
                {
                    GameObject slot = Instantiate(slotPrefab, slotsParent);
                    slot.GetComponent<Image>().sprite = item.itemSprite; // Установка спрайта предмета

                    // Если нужно, установите текст или другие компоненты
                   // slot.transform.Find("ItemName").GetComponent<Text>().text = item.itemName; // Установите имя предмета
                   // slot.transform.Find("ItemQuantity").GetComponent<Text>().text = item.quantity.ToString(); // Установите количество предметов
                }
            }
        }
    }
    protected void CreateInventorySlots()
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

    protected virtual void OnSlotClicked(int x, int y)
    {
        Item item = inventory.GetItem(x, y);
        if (item != null)
        {
            Debug.Log($"Slot {x},{y} clicked! Item: {item.itemName}");
            // Здесь вы можете добавить логику для отображения информации о предмете
        }
    }
    public static void OpenInventory()
    {
        //OpenPlayerWindow(INVENTORY_WINDOW);
        var guiManager = ServiceLocator.GetService<IGUIManager>();
        guiManager.OpenPlayerWindow("Windows/Player_Windows_prefs/InventoryWindow");
       // GUIManager.Instance.OpenPlayerWindow(GameManager.Instance.resourcesLoadManager.LoadPlayerWindow("InventoryWindow")); // Новый метод по пути
        Debug.Log("Open Inventory");
    }

}
