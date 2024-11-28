using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseVisualScript : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprite;

    private SpriteRenderer _image;
    private bool _isOpen;

    private void Start()
    {
        _image = GetComponent<SpriteRenderer>();
        if (_sprite.Length > 0)
        {
            _image.sprite = _sprite[0];
        }
        else
        {
            Debug.LogError("Image didn't exist!");
        }
    }

    // Здесь будем писапть код сущности
}
