using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

//Скрипт отвечающий за отброс после удара
public class knockBack : MonoBehaviour
{
    [SerializeField] private float _knockBackForce = 3f;
    [SerializeField] private float _knockBackMovingTimeMax = 0.3f;

    private float _knockBackMovingTimer;

    private Rigidbody2D _rb;

    //Свойство отвечающее за отлет
    public bool IsGettingKnockedBack { get; private set; }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _knockBackMovingTimer-=Time.deltaTime;
        if(_knockBackMovingTimer < 0)
            StopKnockBackMovement();
    }

    public void GetKnockBack(Transform damageSource)
    {
        IsGettingKnockedBack = true;
        _knockBackMovingTimer=_knockBackMovingTimeMax;
        Vector2 difference = (transform.position - damageSource.position).normalized * _knockBackForce / _rb.mass;
        _rb.AddForce(difference,ForceMode2D.Impulse);
    }

    public void StopKnockBackMovement()
    {
        _rb.velocity = Vector2.zero;
        IsGettingKnockedBack=false;
    }


}
