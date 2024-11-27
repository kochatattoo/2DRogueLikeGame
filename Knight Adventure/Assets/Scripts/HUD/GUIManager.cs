using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts;
   
//Автоматически добавляем необходимые компонент
[RequireComponent (typeof(SceneManager))]
    //Класс отвечающий за реализацию HUD меню
  public class GUIManager : MonoBehaviour
    {
       public static GUIManager Instance {  get; private set; }
        //Объявляем переменные текстовых полей
        [SerializeField] TextMeshProUGUI _name;
        [SerializeField] TextMeshProUGUI _coins;
        [SerializeField] TextMeshProUGUI _level;

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

    private void FirstTextAwake()
       {
        if (User.Instance == null)
            User.Instance = SaveManager.Instance.LoadLastGame();

        //Присваиваем значеие переменных из значения полей USER
            _name.text = User.Instance.GetName();
            _coins.text = User.Instance.GetCoins().ToString();
            _level.text = User.Instance.GetLevel().ToString();
        }

       public void SetTextAreas()
        {
            //Присваиваем значеие переменных из значения полей USER
            _name.text = User.Instance.GetName();
            _coins.text = User.Instance.GetCoins().ToString();
            _level.text = User.Instance.GetLevel().ToString();
        }

    }
