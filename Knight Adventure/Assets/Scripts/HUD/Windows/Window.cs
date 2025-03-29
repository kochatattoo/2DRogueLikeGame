using UnityEngine;
using UnityEngine.UI;
using System;
using Assets.ServiceLocator;

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

        var gameInput = ServiceLocator.GetService<IGameInput>();
        gameInput.DisableMovement(); // ��������� �������� ������, �� �� ���� �������� ��� ������ ESC
    }

    // ������ �� ���������� �������� ������, � �� ����� ������ ����� ������� ��������� ���������
    public virtual void CloseWindow()
    {
        if (activeWindow == this) // ��������, �������� �� ������� �������� ����
        {
            PlaySound(closeSound); // ��������������� ����� ��������
          //  gameObject.SetActive(false); // ������������ ����
            Time.timeScale = 1; // ������������ ����

            OnWindowClosed?.Invoke(); // �������� ������� ��������

            if (GUIManager.IsQueueEmpty())
            {
                var gameInput = ServiceLocator.GetService<IGameInput>();
                gameInput.EnableMovement();
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

    protected void PlaySound(AudioClip clip)
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
