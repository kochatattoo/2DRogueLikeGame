using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicalBallVisual : MonoBehaviour
{
    // private MagicalBall magicalBall;
    public enum State
    {
        Flying,
        Hit,
        Destroy
    }

    private State currentState;

    private Animator animator; // Аниматор, который будет отвечать за анимацию

    private void Awake()
    {
        animator = GetComponent<Animator>(); // Получаем компонент Animator
    }

    private void Start()
    {
        SetState(State.Flying); // Устанавливаем начальное состояние

    }

    private void Update()
    {
        // Здесь можно управлять логикой для переключения состояний, если нужно
    }

    public void SetState(State newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case State.Flying:
                PlayFlyingAnimation();
                break;

            case State.Hit:
                PlayHitAnimation();
                break;

            case State.Destroy:
                PlayDestroyAnimation();
                break;
        }
    }

    private void PlayFlyingAnimation()
    {
        animator.SetBool("IsFlying", true);
        animator.SetBool("HasHit", false);
        animator.SetBool("IsDestroying", false);
    }

    private void PlayHitAnimation()
    {
        animator.SetBool("IsFlying", false);
        animator.SetBool("HasHit", true);
        animator.SetBool("IsDestroying", false);

        // После завершения анимации уничтожите объект
        Destroy(gameObject, 0.8f); // Уничтожение через 1 секунду (или по времени анимации)
    }

    private void PlayDestroyAnimation()
    {
        animator.SetBool("IsFlying", false);
        animator.SetBool("HasHit", false);
        animator.SetBool("IsDestroying", true);

        // После завершения анимации уничтожите объект
        Destroy(gameObject, 0.8f); // Уничтожение через 1 секунду (или по времени анимации)
    }
}
