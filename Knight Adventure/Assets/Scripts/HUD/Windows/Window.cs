using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Window : MonoBehaviour
{
    public AudioClip openSound;  // ���� ��������� ����
    public AudioClip closeSound; // ���� �������� ����
    public Button closeButton;    // ������ �������� ����
    public Button okButton;       // ������ "��"

    private AudioSource audioSource;  // �����������
    private static Window activeWindow; // ������� �������� ����
    private static Queue<Window> windowQueue = new Queue<Window>(); // ������� ����
    private static bool isPaused = false; // ���� �����

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
        isPaused = true; // ������������� ���� �����
        activeWindow = this; // ������������� �������� ����
    }

    public virtual void CloseWindow()
    {
        if (activeWindow == this) // ��������, �������� �� ������� �������� ����
        {
            PlaySound(closeSound); // ��������������� ����� ��������
            gameObject.SetActive(false); // ������������ ����
            Time.timeScale = 1; // ������������ ����
            isPaused = false; // ���������� ���� �����
            activeWindow = null; // ������� ������ �� �������� ����
            ShowNextWindow(); // ����� ���������� ���� �� �������
        }
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
        windowQueue.Enqueue(window);
        if (activeWindow == null)
        {
            ShowNextWindow(); // �������� ��������� ���� � �������
        }
    }

    public static void ShowNextWindow() // ����� ��� ������ ���������� ����
    {
        if (windowQueue.Count > 0)
        {
            Window nextWindow = windowQueue.Dequeue(); // �������� ��������� ���� �� �������
            if (nextWindow != null)
            {
                nextWindow.OpenWindow(); // ��������� ��������� ����
            }
        }
        else
        {
            Time.timeScale = 1; // ���������� ������� �������� � �����, ����� ��� �������� ����
        }
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
