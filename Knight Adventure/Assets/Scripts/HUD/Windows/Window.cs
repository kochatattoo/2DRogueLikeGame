using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Window : MonoBehaviour
{
    public AudioClip openSound;  // ���� ��������� ����
    public AudioClip closeSound; // ���� �������� ����
    public Button closeButton;    // ������ �������� ����
    public Button okButton;       // ������ "��"

    private AudioSource audioSource;  // �����������

    private static Window activeWindow; // ������� �������� ����

    // �������, ���������� ��� �������� ����
    public event Action OnWindowClosed;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // ��������� ��������� AudioSource
        closeButton.onClick.AddListener(CloseWindow); // �������� �� ������� ������ ��������
        okButton.onClick.AddListener(OnOkButtonClicked); // �������� �� ������� ������ "��"
    }

    public virtual void OpenWindow()
    {
        PlaySound(openSound); // ��������������� ����� ��������
        gameObject.SetActive(true); // ���������� ����
        Time.timeScale = 0; // ������ ���� �� �����
        activeWindow = this; // ������������� �������� ����

        GameInput.Instance.DisableMovement(); // ��������� �������� ������, �� �� ���� �������� ��� ������ ESC
    }

    // ������ �� ���������� �������� ������, � �� ����� ������ ����� ������� ��������� ���������
    public virtual void CloseWindow()
    {
        if (activeWindow == this) // ��������, �������� �� ������� �������� ����
        {
            PlaySound(closeSound); // ��������������� ����� ��������
            gameObject.SetActive(false); // ������������ ����
            Time.timeScale = 1; // ������������ ����

            OnWindowClosed?.Invoke(); // �������� ������� ��������

            if (GUIManager.IsQueueEmpty())
            {
                GameInput.Instance.EnableMovement();
            }
            else
            {
                GUIManager.ShowNextWindow(); // ����� ���������� ���� �� �������
            }

        }
     
        Destroy(this.gameObject);
    }

    protected virtual void OnOkButtonClicked()
    {
        // ��������� ���� ��� ������� "��"
        CloseWindow();
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public static void QueueWindow(Window window) // ����� ��� ���������� � �������
    {
       GUIManager.QueueWindow(window);
    }

    public static void ShowPriorityWindow(Window window) // ����� ��� ������ ������������ ����
    {
        if (activeWindow != null)
        {
            activeWindow.CloseWindow(); // �������� ������� �������� ����
        }
        window.OpenWindow(); // ������� ������������ ����
    }
}
