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
    // ����� ��� ��������� ��������� ������� �� ������
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buttonImage != null)
        {
            buttonImage.color = highlightColor; // �������� ���� �� ������������
        }
    }
    // ����� ��� ��������� ����� ������� � ������
    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonImage != null)
        {
            buttonImage.color = originalColor; // ��������������� �������� ����
        }
    }
    // ���� ����� ���������� ��� ����� �� ������
    public void OnPointerClick(PointerEventData eventData)
    {
        // ����� �� ������ ������� ���� � ����� ������� ���� � ���������
        ResetButtonColor(); // ���������� ����
    }

    // ����� ��� ������ ����� ������
    private void ResetButtonColor()
    {
        if (buttonImage != null)
        {
            buttonImage.color = originalColor; // ��������������� �������� ����
        }
    }
}
