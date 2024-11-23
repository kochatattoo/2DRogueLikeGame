using Assets.Scripts;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public SaveManager saveManager; // ������ �� SaveManager
    public AudioManager audioManager; // ������ �� AudioManager
    public GUIManager guiManager; // ������ �� GUIManager
    public GameInput gameInput; // ������ �� GameInputManager
    public User user; // ������ �� ������ User

    private void Awake()
    {
        // �������� �� ������������� ���������� GameManager
        if (Instance == null)
        {
            Instance = this; // ��������� ����������
            DontDestroyOnLoad(gameObject); // �� ���������� ��� �������� ����� �����
            
        }
        else
        {
            Destroy(gameObject); // ������� ������ ���������
        }
        // ������������� ������ �� ���������
        InitializeSingletons();
    }

    // ����� ��� ������������� ������ �� ���������
    private void InitializeSingletons()
    {
        saveManager = SaveManager.Instance; // ��������� ������ �� SaveManager
        audioManager = AudioManager.Instance; // ��������� ������ �� AudioManager
        guiManager = GUIManager.Instance; // ��������� ������ �� GUIManager
        user = User.Instance; // ��������� ������ �� User
        gameInput = GameInput.Instance; // ��������� ������ �� GameInput
    }


    // �� ������ �������� ������, ������� ���������� � ����������� ������ ����������
    public void SaveGame(string fileName)
    {
        saveManager.SaveGame(user, fileName);
    }

    // ������ ������ ��� �������������� � ������� �����������...

}