using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseVisualScript : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private Sprite[] _shadowSprites; // Массив спрайтов для состояний тени

    private SpriteRenderer _spriteRenderer;
    private SpriteRenderer _shadowRenderer; // Рендерер тени
    private bool _isOpen;

    public const int CLOSE = 0;
    public const int OPEN = 1;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _shadowRenderer = transform.Find("ObjectsShadow").GetComponent<SpriteRenderer>(); // Предполагаем, что имя объекта тени - "ObjectsShadow"

        if (_sprites.Length > 0)
        {
            _spriteRenderer.sprite = _sprites[CLOSE];
        }

        // Устанавливаем начальное состояние для тени
        if (_shadowSprites.Length > 0)
        {
            _shadowRenderer.sprite = _shadowSprites[CLOSE]; // По умолчанию - закрыто
        }

        else
        {
            Debug.LogError("Sprites array is empty!");
        }
        // Подписка на событие открытия
        var chest = GetComponent<TreasureChest>();
        if (chest != null)
        {
            chest.ChestIsOpen += OnChestOpened; // Подписка на событие
        }
    }
    // Обработчик события открытия сундука
    private void OnChestOpened(object sender, System.EventArgs e)
    {
        if (_isOpen)
        {
            Close(); // Если сундук уже открыт, закрываем его
        }
        else
        {
            Open(); // Открываем сундук
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

        if (_shadowSprites.Length > 0)
        {
            _shadowRenderer.sprite = _isOpen ? _shadowSprites[OPEN] : _shadowSprites[CLOSE]; // Обновляем тень
        }
    }
    private void OnDestroy() // Отписка от события при уничтожении объекта
    {
        var chest = GetComponent<TreasureChest>();
        if (chest != null)
        {
            chest.ChestIsOpen -= OnChestOpened; // Отписываемся от события
        }
    }
}
