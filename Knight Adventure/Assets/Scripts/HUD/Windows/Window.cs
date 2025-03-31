using UnityEngine;
using UnityEngine.UI;
using System;
using Assets.ServiceLocator;
using Assets.Scripts.Interfaces;

public class Window : MonoBehaviour
{
    public AudioClip openSound;  // ���� ��������� ����
    public AudioClip closeSound; // ���� �������� ����

    protected ButtonClickAudio _buttonClickAudio;

    public Button closeButton;    // ������ �������� ����
    public Button okButton;       // ������ "��"

    private AudioSource _audioSource;  // �����������

    private static Window activeWindow; // ������� �������� ����

    // �������, ���������� ��� �������� ����
    public event Action OnWindowClosed;

    private void Awake()
    {
        GetButtonClickAudioController();

        _audioSource = gameObject.AddComponent<AudioSource>(); // ��������� ��������� AudioSource
        closeButton.onClick.AddListener(CloseWindow); // �������� �� ������� ������ ��������
        okButton.onClick.AddListener(OnOkButtonClicked); // �������� �� ������� ������ "��"
    }

    private void GetButtonClickAudioController() 
    {
        _buttonClickAudio = gameObject.AddComponent<ButtonClickAudio>();
        _buttonClickAudio.StartScript();
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
            // PlaySound(closeSound); // ��������������� ����� ��������
            _buttonClickAudio.PlayClickAudio();
            //  var audioManager = ServiceLocator.GetService<IAudioManager>();
            //  audioManager.PlayAudio(0); // TODO: ��� �� ��� ���� ������� � ������� � �������������

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
    private void OnDestroy()
    {
           var audioManager = ServiceLocator.GetService<IAudioManager>();
           audioManager.PlayAudio(0); // TODO: ��� �� ��� ���� ������� � ������� � �������������
    }
    protected virtual void OnOkButtonClicked()
    {
        _buttonClickAudio.PlayClickAudio();
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
}
