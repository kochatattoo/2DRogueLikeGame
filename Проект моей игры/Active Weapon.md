Данный скрипт необходим в игре для реализации возможности менять активное оружие с дальнего боя на ближний и вообще между разными видами оружия 

Создаем пустой объект Gameobject -> называем его ActiveWeapon -> и пишем ему следующий скрипт :

Объявление переменных 
```
//Делаем статический класс
public static ActiveWeapon Instance {get; private set; }

  //Объявляем переменные классов Меч и Магия
  [SerializeField] private Sword sword;
  [SerializeField] private WitchAttack _magic;

  private void Awake()
  {
      //Синглтон
      Instance = this;
  }

```

Далее в методе Update - проверяем статус нашего персонажа
```
 private void Update()
 {
 //Если игрок жив, то следуюем за игроком
     if(Player.Instance.IsAlive())
      FollowMousePosition();
 }
```

Напишем методы для получения активного оружия 

```
  //Методы для получения активного орудия атаки
  public Sword GetActiveWeapon()
  { return sword; }
  public WitchAttack GetWeapon() 
  { return _magic; }
```

Метод отвечающий за переключения оружия справа налево за нашим персонажем при изменении позиции мышки 

```
 private void FollowMousePosition()
 {
     Vector3 mousePos = GameInput.Instance.GetMousePosition();
     Vector3 playerPos = Player.Instance.GetPlayerScreenPosiyion();

     if (mousePos.x < playerPos.x)
     {
         transform.rotation=Quaternion.Euler(0,180,0);
     }
     else
     {
         transform.rotation = Quaternion.Euler(0, 0, 0);
     }
 }
```


Далее идем писать скрипты отвечающие за сами логику атаки 
[[Sword script]]
[[WitchAttack script]]
