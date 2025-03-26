using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Image buttonImage;
    public Color highlightColor;
    private Color originalColor;

    public void Start()
    {
        buttonImage = GetComponent<Image>();
        if(buttonImage != null )
        {
            originalColor = buttonImage.color;
        }

        highlightColor = new Color(0.8396226f, 0.7222552f, 0.5663492f, 1f);
    }
    // Метод для обработки наведения курсора на кнопку
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buttonImage != null)
        {
            buttonImage.color = highlightColor; // Изменяем цвет на подсвеченный
        }
    }
    // Метод для обработки ухода курсора с кнопки
    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonImage != null)
        {
            buttonImage.color = originalColor; // Восстанавливаем исходный цвет
        }
    }
    // Этот метод вызывается при клике на кнопку
    public void OnPointerClick(PointerEventData eventData)
    {
        // Здесь вы можете открыть окно и затем вернуть цвет к исходному
        ResetButtonColor(); // Сбрасываем цвет
    }

    // Метод для сброса цвета кнопки
    private void ResetButtonColor()
    {
        if (buttonImage != null)
        {
            buttonImage.color = originalColor; // Восстанавливаем исходный цвет
        }
    }
}
