using UnityEngine;
using UnityEngine.UI;
using System;
using Assets.ServiceLocator;
using Assets.Scripts.Interfaces;
using TMPro;

public class Window : MonoBehaviour
{
    public AudioClip openSound;  // ���� ��������� ����
    public AudioClip closeSound; // ���� �������� ����

    public Button closeButton;    // ������ �������� ����
    public Button okButton;       // ������ "��"

    public TextMeshProUGUI window_text;

    private AudioSource _audioSource;  // �����������

    private static Window activeWindow; // ������� �������� ����

    // �������, ���������� ��� �������� ����
    public event Action OnWindowClosed;

    private void Awake()
    {
        _audioSource = gameObject.AddComponent<AudioSource>(); // ��������� ��������� AudioSource
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
        var audioManager = ServiceLocator.GetService<IAudioManager>();
        audioManager.PlayClick();
        // ��������� ���� ��� ������� "��"
        CloseWindow();
    }

    protected void PlaySound(AudioClip clip)
    {
        if (clip != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(clip);
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

    public virtual void SetText(string newText)
    {
        if (window_text != null)
        {
            window_text.text = newText;
        }
    }
}
