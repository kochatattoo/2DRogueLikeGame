using UnityEngine;

//Автоматически добавляем необходимые нам компоненты
[RequireComponent(typeof(Animator))]
[RequireComponent (typeof(SpriteRenderer))]
//Класс отвечающий за отображений анимации скелета
public class SkeletonVisual : MonoBehaviour
{
    //Обращаемся к классу EnenmyAI для получения его компонентов
    [SerializeField] private EnemyAI _enemyAI;
    //Обращаемся к классу EnemuEntity для получения его компонентов
    [SerializeField] private EnemyEntity _enemyEntity;
    //обращаемся к объекту
    [SerializeField] private GameObject _enemyShadow;


    //Переменная аниматора 
    private Animator _animator;

    //Константы Параметров в аниматоре
    private const string IS_RUNNING = "IsRunning";
    private const string CHASING_SPEED_MULTIPLIER = "ChasingSpeedMultiplier";
    private const string ATTACK = "Attack";
    private const string IS_DIE = "IsDie";
    private const string TAKE_HIT = "TakeHit";

    //Доюавляем поле спрайт рендеров
    SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        //Кешируем компоненты аниматора и спрайт рендера
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

    }

    private void Start()
    {
        //Подписываемся на событие атаки в классе EnemyAI
        _enemyAI.OnEnemyAttack += _enemyAI_OnEnemyAttack;
        //Подпписываемся на событие получение урона в классе EnemyEntity
        _enemyEntity.OnTakeHit += _enemyEntity_OnTakeHit;
        //Подписываемся на событие смерти
        _enemyEntity.OnDeath += _enemyEntity_OnDeath;
    }

    //Методы вызываемые после уничтожения объекта
    private void OnDestroy()
    {
        //Отписываемся от события
        _enemyAI.OnEnemyAttack -= _enemyAI_OnEnemyAttack;
        _enemyEntity.OnTakeHit -= _enemyEntity_OnTakeHit;
        _enemyEntity.OnDeath -= _enemyEntity_OnDeath;
    }

    private void Update()
    {
        //Устанавливаем значение isrunning как у нашего объекта
        _animator.SetBool(IS_RUNNING, _enemyAI.IsRunning);
        //Устанавливаем умножение скорости для аниматора 
        _animator.SetFloat(CHASING_SPEED_MULTIPLIER, _enemyAI.GetRoamingAnimationSpeed());
    }

    //Метод для включения полигонколайдера при атаке(добавляем в аниматоре)
    public void TriggerAttackAnimationTurnOff()
    {
        _enemyEntity.PolygonCollaiderTurnOff();
    }

    //Метод для отключения полигонколайдера после атаки(добавляем в аниматоре)
    public void TriggerAttackAnimationTurnOn()
    {
        _enemyEntity.PolygonCollaiderTurnOn();
    }

    //Метод для реализации события АТАКИ
    private void _enemyAI_OnEnemyAttack(object sender, System.EventArgs e)
    {
        //Вызываем тригер атака в аниматоре
        _animator.SetTrigger(ATTACK);
    }

    //Метод вызываемый при срабатывании события
    private void _enemyEntity_OnTakeHit(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(TAKE_HIT);
    }

    //Метод вызываемый при событии смерть
    private void _enemyEntity_OnDeath(object sender, System.EventArgs e)
    {
        _animator.SetBool(IS_DIE, true);
        //Выставляем что бы был за героем после гибели
        _spriteRenderer.sortingOrder = -1;
        //Убираем тень
        _enemyShadow.SetActive(false);
    }
}
