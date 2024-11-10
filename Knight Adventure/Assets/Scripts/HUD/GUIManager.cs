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

        User user =new User();

       private void Awake()
        {
            Instance = this;
            SetTextAreas();
            //Debug.Log(GameManager.Instance.user.GetName());

        }
   
       private void SetTextAreas()
        {
            //����������� ������� ���������� �� �������� ����� USER
            _name.text = GameManager.Instance.user.GetName();
            _coins.text = GameManager.Instance.user.GetCoins().ToString();
            _level.text = GameManager.Instance.user.GetLevel().ToString();

        }

    }
