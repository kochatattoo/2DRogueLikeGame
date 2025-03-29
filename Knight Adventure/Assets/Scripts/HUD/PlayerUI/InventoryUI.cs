using Assets.Scripts.Interfaces;
using Assets.ServiceLocator;
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
            //inventory = FindObjectOfType<Player>().GetComponent<Inventory>();

            inventory = Player.Instance.playerInventory;
            if (inventory == null)
            {
                Debug.LogError("Inventory component not found on Player!");
                var notificationManager = ServiceLocator.GetService<INotificationManager>(); 
                notificationManager.HandleError("��������� ������: �� ������ ��������� ������.", 0);
            }
        }
        // �������� ������ � ���������
        CreateInventorySlots();
        UpdateInventoryUI(); // ��������� ��������� ��� ������
    }

    public virtual void UpdateInventoryUI()
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
    protected void CreateInventorySlots()
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

    protected virtual void OnSlotClicked(int x, int y)
    {
        Item item = inventory.GetItem(x, y);
        if (item != null)
        {
            Debug.Log($"Slot {x},{y} clicked! Item: {item.itemName}");
            // ����� �� ������ �������� ������ ��� ����������� ���������� � ��������
        }
    }
    public static void OpenInventory()
    {
        //OpenPlayerWindow(INVENTORY_WINDOW);
        var guiManager = ServiceLocator.GetService<IGUIManager>();
        guiManager.OpenPlayerWindow("Windows/Player_Windows_prefs/InventoryWindow");
       // GUIManager.Instance.OpenPlayerWindow(GameManager.Instance.resourcesLoadManager.LoadPlayerWindow("InventoryWindow")); // ����� ����� �� ����
        Debug.Log("Open Inventory");
    }

}
