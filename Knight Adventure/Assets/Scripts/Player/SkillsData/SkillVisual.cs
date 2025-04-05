using UnityEngine;

public class SkillVisual: MonoBehaviour
{
    [SerializeField] private Skill skill; 
    public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>(); // Получаем компонент Animator
    }
}

