using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts;
   
//������������� ��������� ����������� ���������
[RequireComponent (typeof(SceneManager))]
//����� ���������� �� ���������� HUD ����
  public class GUIManager : MonoBehaviour
    {
       public static GUIManager Instance {  get; private set; }
        //��������� ���������� ��������� �����
        [SerializeField] TextMeshProUGUI _name;
        [SerializeField] TextMeshProUGUI _coins;
        [SerializeField] TextMeshProUGUI _level;

    public GameObject[] uiPrefabs; // ������ �������� ��� UI ����

    private GameObject _currentWindow; // ������� ����

    //private User user;

       private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else 
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
            //Debug.Log(GameManager.Instance.user.GetName());

       }

    private void Start()
    {
        User.Instance = SaveManager.Instance.LoadLastGame();
        FirstTextAwake();
    }

    public void SetTextAreas()
    {
        //����������� ������� ���������� �� �������� ����� USER
        _name.text = User.Instance.GetName();
        _coins.text = User.Instance.GetCoins().ToString();
        _level.text = User.Instance.GetLevel().ToString();
    }

    public void OpenWindow(int windowIndex)
    {
        // ���������� ������� ����, ���� ��� ����������
        if (_currentWindow != null)
        {
            Destroy(_currentWindow);
        }

        // �������� �� ���������� �������
        if (windowIndex >= 0 && windowIndex < uiPrefabs.Length)
        {
            // �������� ������ ����
            _currentWindow = Instantiate(uiPrefabs[windowIndex]);
            // ���������, ��� ����� ���� ����������� � Canvas
            _currentWindow.transform.SetParent(GameObject.Find("PauseMenu").transform, false);
        }
        else
        {
            Debug.LogWarning("Window index out of range: " + windowIndex);
        }
    }

    // ����� ��� �������� �������� ����
    public void CloseCurrentWindow()
    {
        if (_currentWindow != null)
        {
            Destroy(_currentWindow);
            _currentWindow = null;
        }
    }

    private void FirstTextAwake()
    {
        if (User.Instance == null)
            User.Instance = SaveManager.Instance.LoadLastGame();

        //����������� ������� ���������� �� �������� ����� USER
        _name.text = User.Instance.GetName();
        _coins.text = User.Instance.GetCoins().ToString();
        _level.text = User.Instance.GetLevel().ToString();
    }
}
