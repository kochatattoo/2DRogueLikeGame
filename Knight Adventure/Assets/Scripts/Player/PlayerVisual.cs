using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    /*[SerializeField] Player Player;*/

    private const string IS_RUNNING = "IsRunning";
    private const string IS_DEAD = "IsDead";
    private const string TAKE_DAMAGE = "TakeDamage";

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        Player.Instance.OnPlayerDeath += Player_OnPlayerDeath;
        Player.Instance.OnTakeHit += Player_OnTakeHit;
    }

    private void Player_OnTakeHit(object sender, System.EventArgs e)
    {
        animator.SetTrigger(TAKE_DAMAGE);

    }

    private void Player_OnPlayerDeath(object sender, System.EventArgs e)
    {
        animator.SetBool(IS_DEAD, true);
    }

    private void Update()
    {
        /* animator.SetBool(IS_RUNNING, Player.IsRunning());*/
        animator.SetBool(IS_RUNNING, Player.Instance.IsRunning());

        if (Player.Instance.IsAlive())
             AdjustPlayerFacingDirection();
       
    }

    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos=GameInput.Instance.GetMousePosition();
        Vector3 playerPos=Player.Instance.GetPlayerScreenPosiyion();

        if (mousePos.x < playerPos.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

}
