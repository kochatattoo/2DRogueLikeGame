using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TreasureChest : MonoBehaviour
{
    public GameObject lootPrefab; // ������ ����������� ��������
    public List<GameObject> possibleLootItems; // ������ ��������� ��������� ��� ���������
    public float interactionDistance = 3f; // ��������� ��� ��������������
    public TextMeshProUGUI interactionText; // ����� ��� ����������� ���������

    private bool isOpen = false; // ������ �������� �������
    private Transform playerTransform; // ������ �� ������

    private void Start()
    {
        GameInput.Instance.OnPlayerOpen += Player_OnPlayerOpen;
        playerTransform = Player.Instance.transform; // �������� ������ �� ������
        HideInteractionPrompt(); // �������� ��������� � ������
    }

    private void Player_OnPlayerOpen(object sender, System.EventArgs e)
    {
        if (isOpen) return;

        if (Vector3.Distance(Player.Instance.transform.position, transform.position) < interactionDistance)
        {
            // ���������� ������� �������
            ShowInteractionPrompt();
            OpenChest();
           
        }
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

    private void OpenChest()
    {
        isOpen = true; // ������������� ������ �� ��������
        HideInteractionPrompt(); // �������� ��������� ��� ��������

        // ������ ��������� ���������
        DropLoot();

        // ����� ����� �������� �������� �������� ������� ��� ������
        Debug.Log("Chest opened!");
    }

    private void DropLoot()
    {
        foreach (GameObject item in possibleLootItems)
        {
            // ��������� ��������� �������� (��������, 50% ����)
            if (Random.value > 0.1f)
            {
                Vector3 dropPosition = GetRandomDropPosition(); // �������� ��������� ������� ��� ��������� ��������
                Instantiate(item, dropPosition, Quaternion.identity); // ������� ������� � ��������� �������
                Debug.Log($"Dropped: {item.name} at {dropPosition}");
            }
        }
    }
    private Vector3 GetRandomDropPosition()
    {
        // ���������� ������ ��������� ���������
        float dropRadius = 1.5f; // ������� �������� ������

        // ���������� ��������� ���������� �� ���� X � Y � �������� ��������� �������
        float randomX = Random.Range(-dropRadius, dropRadius);
        float randomY = Random.Range(-dropRadius, dropRadius);

        // ���������� ������� � ������������� Z (��������, 0)
        return new Vector3(transform.position.x + randomX, transform.position.y + randomY, transform.position.z);
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
        GameInput.Instance.OnPlayerOpen -= Player_OnPlayerOpen;
    }
}
