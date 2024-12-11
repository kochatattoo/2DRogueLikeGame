using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Scripts.Items;
using TMPro;

public class StorageChest : MonoBehaviour
{
    public EventHandler ChestOpened; // ������� �������� �������
    public StorageInventory chestInventory; // ��������� �������

    private bool isOpen = false; // ������ �������� �������
    public float interactionDistance = 3f; // ��������� ��� ��������������
    public TextMeshProUGUI interactionText; // ����� ��� ����������� ���������

    public int inventoryWidth = 6; // ������ ���������
    public int inventoryHeight = 6; // ������ ���������

    private void Start()
    {
        //GameObject chestObject = new GameObject("ChestInventory");
        // �������� ��������� "Inventory" � ���������� �������
        //chestInventory = chestObject.AddComponent<Inventory>();

        chestInventory.width = inventoryWidth; // ������������� ������
        chestInventory.height = inventoryHeight; // ������������� ������
        chestInventory.coins = 0; // �������������� ������

        GameInput.Instance.OnPlayerOpen += PlayerOnPlayerOpen;
        HideInteractionPrompt(); // �������� ��������� � ������
    }
    private void Update()
    {
        // ��������� ���������� �� ������ ������ ����
        if (!isOpen && Vector3.Distance(Player.Instance.transform.position, transform.position) < interactionDistance)
        {
            ShowInteractionPrompt(); // ���������� ���������
        }
        else
        {
            HideInteractionPrompt(); // �������� ���������, ���� ����� ������
        }
    }
    private void PlayerOnPlayerOpen(object sender, EventArgs e)
    {
        if (isOpen) return;

        if (Vector3.Distance(Player.Instance.transform.position, transform.position) < interactionDistance)
        {
            // ���������� ������� �������
            ShowInteractionPrompt();
            OpenChest();

        }
    }

    public void OpenChest()
    {
       // isOpen = true; // ������������� ������ �� ��������
        HideInteractionPrompt(); // �������� ��������� ��� ��������

         ChestOpened?.Invoke(this, EventArgs.Empty);
        // �������� ��������� ������� ����� GUI_Manager
        GUIManager.Instance.OpenStorageChestInventory(chestInventory); // ��������� ��������� ����� GUI_Manager
    }

    public void AddItemToChest(Item item, int x, int y)
    {
        if (!chestInventory.AddItem(item))
        {
            Debug.Log("Not enough space in chest inventory!");
        }
    }

    public void RemoveItemFromChest(int x, int y)
    {
        chestInventory.RemoveItem(x, y);
    }
    private void ShowInteractionPrompt()
    {
        interactionText.text = "Press 'E' to open the chest"; // ����������� ���������
        interactionText.gameObject.SetActive(true); // �������� �����
                                                    // UpdatePromptPosition(); // ��������� ������� ���������
    }
    private void HideInteractionPrompt()
    {
        interactionText.gameObject.SetActive(false); // �������� �����
    }

    private void UpdatePromptPosition()
    {
        // ������� ������ ��� ��������
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up); // ��������� ����� ��� ��������
        interactionText.transform.position = screenPos; // ������������� ������� �� ������
    }

    private void OnDestroy() // ������������ �� ������� ��� ����������� �������
    {
        GameInput.Instance.OnPlayerOpen -= PlayerOnPlayerOpen;
    }

}
