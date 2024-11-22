using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public User user;
        public static GameManager Instance { get; private set; }


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
                return;
            }
            else 
            Destroy(this.gameObject);

            user = SaveManager.Instance.LoadLastGame();
        }
        private void Start()
        {
            Debug.Log(user.GetName());
            
        }


    }
}