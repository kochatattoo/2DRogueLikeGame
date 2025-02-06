using Assets.ServiceLocator;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TreasureChest : MonoBehaviour
{
    public GameObject lootPrefab; // ������ ����������� ��������
    public List<GameObject> possibleLootItems; // ������ ��������� ��������� ��� ���������
    public float interactionDistance = 3f; // ��������� ��� ��������������
    public TextMeshProUGUI interactionText; // ����� ��� ����������� ���������
    public EventHandler ChestIsOpen; // ������� �������� �������

    private bool isOpen = false; // ������ �������� �������
    private Transform playerTransform; // ������ �� ������
    private Player _player;

    private IGameInput _gameInput;

    // ��������� ZENJECT
    //[Inject]
    //private void Construct(Player player)
    //{ 
    //    _player = player; 
    //    Debug.Log("��� �� ���������!!!!!!!!! "+_player.transform.position+"  ���� �� �����");
    //}


    private void Start()
    {
        _gameInput =ServiceLocator.GetService<IGameInput>();
        _gameInput.OnPlayerOpen += Player_OnPlayerOpen;
        playerTransform = Player.Instance.transform; // �������� ������ �� ������
        HideInteractionPrompt(); // �������� ��������� � ������
    }

    private void Player_OnPlayerOpen(object sender, System.EventArgs e)
    {
        if (isOpen) return;

        if (Vector3.Distance(playerTransform.position, transform.position) < interactionDistance)
        {
            // ���������� ������� �������
            ShowInteractionPrompt();
            OpenChest();

            ChestIsOpen?.Invoke(this, EventArgs.Empty);
        }
    }
    private void Update()
    {
        // ��������� ���������� �� ������ ������ ����
        if (!isOpen && Vector3.Distance(playerTransform.position, transform.position) < interactionDistance)
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
            if (UnityEngine.Random.value > 0.1f)
            {
                Vector3 dropPosition = GetRandomDropPosition(); // �������� ��������� ������� ��� ��������� ��������
                                                                
                // ���������, �������� �� ��� �����
                if (IsPositionAvailable(dropPosition))
                {
                    Instantiate(item, dropPosition, Quaternion.identity); // ������� ������� � ��������� �������
                    Debug.Log($"Dropped: {item.name} at {dropPosition}");
                }
                else
                {
                    Debug.Log($"Position {dropPosition} is not available.");
                }
            }
        }
    }
    private bool IsPositionAvailable(Vector3 position)
    {
        // �������� ����������� ������� � ������� OverlapCircle
        //float checkRadius = 0.5f; // ������ �������� (� ����������� �� ������� �������)
        //int layerMask = LayerMask.GetMask("Default"); // ������� ����� ������ ����, ���� ���������

        // ���������� true, ���� ������� �������� (��� �����������), ����� false
        // return !Physics2D.OverlapCircle(position, checkRadius, layerMask);

        //��������
        return true;
    }

    private Vector3 GetRandomDropPosition()
    {
        // ���������� ������ ��������� ���������
        float dropRadius = 1.5f; // ������� �������� ������

        // ���������� ��������� ���������� �� ���� X � Y � �������� ��������� �������
        float randomX = UnityEngine.Random.Range(-dropRadius, dropRadius);
        float randomY = UnityEngine.Random.Range(-dropRadius, dropRadius);

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
        _gameInput.OnPlayerOpen -= Player_OnPlayerOpen;
    }
}
