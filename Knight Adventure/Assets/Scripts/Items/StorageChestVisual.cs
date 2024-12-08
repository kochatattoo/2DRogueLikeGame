using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageChestVisual : MonoBehaviour
{
    [SerializeField] private StorageChest _StorageChest;

    private Animator animator;
    private SpriteRenderer spriteRenderer;


    //ќпишем текстовые константы дл€ упрощенной работы с аниматором
    private const string OPEN = "Open";
    private const string CLOSE = "Close";

    private void Awake()
    {
        // ешируем аниматор и спрайтрендер
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _StorageChest.ChestOpened += ChestOpen;

    }
    private void ChestOpen(object sender, EventArgs e)
    {
        animator.SetTrigger(OPEN);
    }
    public void ChestClose()
    {
        animator.SetTrigger(CLOSE);
    }
}
