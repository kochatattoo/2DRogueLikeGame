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

    private Animator animator; // ��������, ������� ����� �������� �� ��������

    private void Awake()
    {
        animator = GetComponent<Animator>(); // �������� ��������� Animator
    }

    private void Start()
    {
        SetState(State.Flying); // ������������� ��������� ���������

    }

    private void Update()
    {
        // ����� ����� ��������� ������� ��� ������������ ���������, ���� �����
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

        // ����� ���������� �������� ���������� ������
        Destroy(gameObject, 0.8f); // ����������� ����� 1 ������� (��� �� ������� ��������)
    }

    private void PlayDestroyAnimation()
    {
        animator.SetBool("IsFlying", false);
        animator.SetBool("HasHit", false);
        animator.SetBool("IsDestroying", true);

        // ����� ���������� �������� ���������� ������
        Destroy(gameObject, 0.8f); // ����������� ����� 1 ������� (��� �� ������� ��������)
    }
}
