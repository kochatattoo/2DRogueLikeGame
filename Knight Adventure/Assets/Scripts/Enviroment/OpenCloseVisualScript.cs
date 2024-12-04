using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseVisualScript : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private TreasureChest _gameObject;

    private SpriteRenderer _spriteRenderer;
    private bool _isOpen;
    public const int CLOSE = 0;
    public const int OPEN = 1;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_sprites.Length > 0)
        {
            _spriteRenderer.sprite = _sprites[CLOSE];
        }
        else
        {
            Debug.LogError("Image didn't exist!");
        }
    }

    // Метод для открытия объекта
    public void Open()
    {
        if (!_isOpen)
        {
            _isOpen = true;
            UpdateVisual();
            // Вы можете добавить здесь дополнительные действия при открытии, такие как звук или анимацию.
        }
    }

    // Метод для закрытия объекта
    public void Close()
    {
        if (_isOpen)
        {
            _isOpen = false;
            UpdateVisual();
            // Вы можете добавить здесь дополнительные действия при закрытии, такие как звук или анимацию.
        }
    }

    // Обновление визуального состояния
    private void UpdateVisual()
    {
        if (_sprites.Length > 0)
        {
            _spriteRenderer.sprite = _isOpen ? _sprites[OPEN] : _sprites[CLOSE];
        }
    }
}
