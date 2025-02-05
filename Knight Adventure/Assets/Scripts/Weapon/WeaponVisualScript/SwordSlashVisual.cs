using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSlashVisual : MonoBehaviour
{
    [SerializeField] private Sword sword;

    private const string ATTACK = "Attack";
    private Animator animator;

    private void Update()
    {
        sword.OnSwordSwing += Sword_OnSwordSwing;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Sword_OnSwordSwing(object sender, System.EventArgs e)
    {
        animator.SetTrigger(ATTACK);
    }
}
