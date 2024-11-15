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

        User user = new User();

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
            FirstTextAwake();
           //Debug.Log(GameManager.Instance.user.GetName());
        }

       private void FirstTextAwake()
       {
            user.LoadUserSerialize();
        //Присваиваем значеие переменных из значения полей USER
            _name.text = user.GetName();
            _coins.text = user.GetCoins().ToString();
            _level.text = user.GetLevel().ToString();
        }

       public void SetTextAreas()
        {
            //Присваиваем значеие переменных из значения полей USER
            _name.text = user.GetName();
            _coins.text = user.GetCoins().ToString();
            _level.text = user.GetLevel().ToString();
        }

    }
