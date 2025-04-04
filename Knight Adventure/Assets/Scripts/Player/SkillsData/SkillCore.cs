
using System.Collections;
using UnityEngine;

public class SkillCore : MonoBehaviour
 {
    public Skill[] skills; // Массив навыков
    private bool[] skillOnCooldown; // Флаги для отслеживания перезарядки

    void Start()
    {
        skillOnCooldown = new bool[skills.Length];
    }

    // Метод для использования способности
    public void UseSkill(int skillIndex)
    {
        if (skillIndex < 0 || skillIndex >= skills.Length)
        {
            Debug.LogError("Invalid skill index");
            return;
        }

        if (skillOnCooldown[skillIndex])
        {
            Debug.Log($"Skill {skills[skillIndex].skillName} is on cooldown.");
            return;
        }

        // Запускаем корутину для активации навыка
        StartCoroutine(ActivateSkill(skillIndex));
    }

    // Кортуина для активации способности с учетом времени действия и cooldown
    private IEnumerator ActivateSkill(int skillIndex)
    {
        skillOnCooldown[skillIndex] = true;

        // Воспроизводим эффект способности, если он установлен
        if (skills[skillIndex].skillEffect != null)
        {
            Instantiate(skills[skillIndex].skillEffect, transform.position, Quaternion.identity);
        }

        Debug.Log($"Skill {skills[skillIndex].skillName} activated!");

        // Ждем некоторое время, чтобы закончить действие навыка
        yield return new WaitForSeconds(skills[skillIndex].duration);

        Debug.Log($"Skill {skills[skillIndex].skillName} finished!");

        // Ждем время перезарядки перед повторным использованием навыка
        yield return new WaitForSeconds(skills[skillIndex].cooldown);

        skillOnCooldown[skillIndex] = false;
        Debug.Log($"Skill {skills[skillIndex].skillName} is ready to use again!");
    }

}

