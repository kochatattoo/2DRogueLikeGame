using Assets.ServiceLocator;
using UnityEngine;
using UnityEngine.EventSystems;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Exceptions;

public class ButtonClickAudio : MonoBehaviour, IPointerClickHandler
{
    private AudioClip clickSound; // ������ �� ���� �������
    private AudioSource audioSource; // ��������� ��� ��������������� �����
    private ResourcesLoadManager resourcesLoadManager;

   public void StartScript()
    {
        resourcesLoadManager = gameObject.AddComponent<ResourcesLoadManager>();
        audioSource = gameObject.AddComponent<AudioSource>();

        LoadClickSound();
        audioSource.clip = clickSound; // ������������� ���� �������
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        var audioManager = ServiceLocator.GetService<IAudioManager>();
        var notificationManager = ServiceLocator.GetService<INotificationManager>();

        if (audioSource != null && clickSound != null)
        {
            audioSource.volume = audioManager.AudioVolume;
            try
            {
                audioSource.PlayOneShot(clickSound); // ������������� ���� �������
            }
            catch (CustomException ex) // ����� ������ ���������������� ����������
            {
                Debug.LogError("������: " + ex.Message); // �������� ������
                notificationManager.PlayNotificationAudio("Error");
            }
            catch (System.Exception ex)
            {
                Debug.LogError("������: " + ex.Message); // �������� ������
                notificationManager.PlayNotificationAudio("Error");
            }
        }
    }
    public void PlayClickAudio()
    {
        var audioManager = ServiceLocator.GetService<IAudioManager>();
        var notificationManager = ServiceLocator.GetService<INotificationManager>();

        if (audioSource != null && clickSound != null)
        {
            audioSource.volume = audioManager.AudioVolume;
            try
            {
                audioSource.PlayOneShot(clickSound); // ������������� ���� �������
            }
            catch (CustomException ex) // ����� ������ ���������������� ����������
            {
                Debug.LogError("������: " + ex.Message); // �������� ������
                notificationManager.PlayNotificationAudio("Error");
            }
            catch (System.Exception ex)
            {
                Debug.LogError("������: " + ex.Message); // �������� ������
                notificationManager.PlayNotificationAudio("Error");
            }
        }
    }
    private void LoadClickSound()
    {
        clickSound = resourcesLoadManager.LoadUIEffectClips("Click");
    }
}
