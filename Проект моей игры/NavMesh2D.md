NavMesh отлично подходит для поиска путей в 3D, но для грамотного использования в 2D игре стоит воспользоваться следующим открытым ресурсом 

Для правильно реализации NavMesh2D : github.com/h8man/NavMeshPlus
Скачиваем архив и берем из него следующие компоненты -> NavMeshComponents
Перекидываем данную папку в наш проект игры 

По области по которой мы будем ходить должны добавить следующий скрипт - Navigation Modifier - все объекты к которым добавлены данные компоненты участвуют в расчете путей передвижения и по ним можно передвигаться.
Override Area - true
Area -> Walkable

Создаем компонент -> NavMesh Surface  
Добавляем ему два компоненты 
Navigation Surfance и Navigation CollectSource2d
Нажимаем кнопку Rotate surface to XY и после нажимаем кнопку Bake (запечь)
- Запекаем нашу область и рассчитываем пути передвижения для будущих агентов 
с Гизмо будет выделена Синяя область показывающая область движения 

Агенты - наши неигровые объекты(персонажи, враги)

Кусты, деревья и все прочие объекты - это препятствия  и нам надо им тоже добавить скрипты Navigation ModifierVolume -> Area -> Not Walkable 
В данном компоненте мы можем настроить размер области по которой мы считаем что нельзя ходить
в Edit Volume -> мы настраиваем размеры области для расчета 

Для агента устанавливаем компонент Nav Mesh Agent 
-Настройки скорости и другие находятся в графе -Steering
-Настройку размер сетки агента - происходит в графе Obstacle Avoidance

Для настройки передвижения пишем скрипт следующего вида 
```
    //Поле максимального передвижения 
    [SerializeField] private float _roamingDistanceMax = 7f;
    //Поле минимального передвижения
    [SerializeField] private float _roamingDistanceMin = 3f;
    //Поле времени передвижения
    [SerializeField] private float _roamingTimeMax = 3f;

    //Состояния
    private enum State
    {
        Idle, //Покой
        Roaming, //Брожение
    }

    private void Awake()
    {
      //Кешируем переменную класса NavMeshAgent
      _navMeshAgent=GetComponent<NavMeshAgent>();
      //Запрет вращения
      _navMeshAgent.updateRotation = false;
      //Запрет изменения от места расположения
      _navMeshAgent.updateUpAxis = false;
      //состояние
      _currentState = _startingState;
    }

    private void StateHandler()
    {
        //Выбор от состояния
        switch (_currentState)
        {
            //В случае состояния - Движение
            case State.Roaming:
                //переопределяем время с обратным отчетом каждый кадр
                _roamingTimer -= Time.deltaTime;
                //когда время стало меньше нуля
                if (_roamingTimer < 0)
                {
                    //вызываем метод Движение и переопределяем направления
                    Roaming();
                    //снова выставляем время 
                    _roamingTimer = _roamingTimeMax;
                }
         }
     }

    //Метод для разорота противника в сторону преследования игрока
    private void MovementDirectionHandler()
    {
    //Если время следующей проверки наступило
    if(Time.time >_nextCheckDirectionTime)
    {
        //Если противник бежит
        if(IsRunning)
        {
            //Поворачиваемся в сторону пршлого и текущего положения
            ChangeFacingDirection(_lastPosition,transform.position);
        }
        //Если атакует
        else if(_currentState == State.Attacking)
        {
            //Поворачиваемся в сторону игрока
                  ChangeFacingDirection(transform.position,Player.Instance.transform.position);
        }

        //Переприсваиваем последнюю позицию и время след. проверки 
        _lastPosition = transform.position;
        _nextCheckDirectionTime = Time.time+_checkDirectionDuration;
    }
}

//Метод для переопределения движения
     private void Roaming()
{
    //определяем стартовую позицию, как позицию нахождения на данный момент
    _startingPosition= transform.position;
    //определяем новую позицию для движения
    _roamPosition = GetRoamingPosition();
    //Построение нового пути движения к точке
    _navMeshAgent.SetDestination( _roamPosition );
}
//Получение рандомного направления движения
 private Vector3 GetRoamingPosition()
{
    //возвращаем значение новой позиции для движения
    return _startingPosition+ Utils.GetRandomDir()*UnityEngine.Random.Range(_roamingDistanceMin, _roamingDistanceMax);
}
//Установка спрайта по направлению движения
 private void ChangeFacingDirection(Vector3 sourcePosition, Vector3 targetPosition)
{
    //проверка точки до и точки куда ,по координатам Х
    if(sourcePosition.x>targetPosition.x)
    {
        //Поворот на 180 градусов
        transform.rotation=Quaternion.Euler(0,-180,0);
    }
    else
    {
        //поворот на 0.0.0
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
}
```