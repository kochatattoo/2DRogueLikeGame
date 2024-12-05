using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseVisualScript : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private Sprite[] _shadowSprites; // ������ �������� ��� ��������� ����

    private SpriteRenderer _spriteRenderer;
    private SpriteRenderer _shadowRenderer; // �������� ����
    private bool _isOpen;

    public const int CLOSE = 0;
    public const int OPEN = 1;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _shadowRenderer = transform.Find("ObjectsShadow").GetComponent<SpriteRenderer>(); // ������������, ��� ��� ������� ���� - "ObjectsShadow"

        if (_sprites.Length > 0)
        {
            _spriteRenderer.sprite = _sprites[CLOSE];
        }

        // ������������� ��������� ��������� ��� ����
        if (_shadowSprites.Length > 0)
        {
            _shadowRenderer.sprite = _shadowSprites[CLOSE]; // �� ��������� - �������
        }

        else
        {
            Debug.LogError("Sprites array is empty!");
        }
        // �������� �� ������� ��������
        var chest = GetComponent<TreasureChest>();
        if (chest != null)
        {
            chest.ChestIsOpen += OnChestOpened; // �������� �� �������
        }
    }
    // ���������� ������� �������� �������
    private void OnChestOpened(object sender, System.EventArgs e)
    {
        if (_isOpen)
        {
            Close(); // ���� ������ ��� ������, ��������� ���
        }
        else
        {
            Open(); // ��������� ������
        }
    }
    // ����� ��� �������� �������
    public void Open()
    {
        if (!_isOpen)
        {
            _isOpen = true;
            UpdateVisual();
            // �� ������ �������� ����� �������������� �������� ��� ��������, ����� ��� ���� ��� ��������.
        }
    }

    // ����� ��� �������� �������
    public void Close()
    {
        if (_isOpen)
        {
            _isOpen = false;
            UpdateVisual();
            // �� ������ �������� ����� �������������� �������� ��� ��������, ����� ��� ���� ��� ��������.
        }
    }

    // ���������� ����������� ���������
    private void UpdateVisual()
    {
        if (_sprites.Length > 0)
        {
            _spriteRenderer.sprite = _isOpen ? _sprites[OPEN] : _sprites[CLOSE];
        }

        if (_shadowSprites.Length > 0)
        {
            _shadowRenderer.sprite = _isOpen ? _shadowSprites[OPEN] : _shadowSprites[CLOSE]; // ��������� ����
        }
    }
    private void OnDestroy() // ������� �� ������� ��� ����������� �������
    {
        var chest = GetComponent<TreasureChest>();
        if (chest != null)
        {
            chest.ChestIsOpen -= OnChestOpened; // ������������ �� �������
        }
    }
}
