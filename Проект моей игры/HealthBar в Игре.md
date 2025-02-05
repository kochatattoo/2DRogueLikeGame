#Unity 
Реализуем HealthBar на интерфейсе в игре 
Воспользуемся способом через использование спрайта как объект типа Filled

Для этого, выставляем UI-Image, и присваиваем ему спрайт типа Square (Выставляем цвет черный), это будет наша подложка, назовем Boarder
После, создаем два таких же объекта ,один назовем Red(Присвоим цвет красный - он снизу), другой Health (Цвет на ваше усмотрение, я взял зеленый)
В графе Image у Health, Inage Type=Filled, Filled=horizontal

Создаем EmptyObject и называем его HealthManager - присваиваем ему скрипт с одноименным наванием и прописываем следующий код 
```
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour
{
    //Обращаемся к переменой Изображения
    public Image _healthBar;
    //Переменная количества здоровья
    public float _healthAmount = 100f;

    private void Start()
    {
        
    }
    private void Update()
    {
        if(_healthAmount<=0)
        {
            SceneManager.LoadScene(1);
        }

        if(Input.GetKeyDown(KeyCode.Return))
        {
            TakeDamage(20);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Heal(20);
        }
    }

    //Метод отвечающий за получение урона
    public void TakeDamage(float damage)
    {
        //Переприсваиваем значение здоровья
        _healthAmount-=damage;
        //Выставляем соответствующий процент на шкале Filled
        _healthBar.fillAmount = _healthAmount/100f;
    }
    //Метод отвечающий за исцеление
    public void Heal(float healingAmount)
    {
        //Переприсваиваем значение здоровья
        _healthAmount+=healingAmount;
        //Не может превышать 100 и быть ниже 0
        _healthAmount=Mathf.Clamp(_healthAmount, 0, 100);

        //Выставляем соответствующий процент на шкале Filled
        _healthBar.fillAmount = _healthAmount / 100f;
    }
}
```
Данный код был взят с видео с Ютуб, сейчас же в коде ниже, я интегрирую его в логику игры. 

```

```

Дальнейшая логика отвечающая за Хелфбар будет представнлена в следующем скрипте 
[[PlayerStatsUIManager]]