#Unity
Позволяет выполнять код в отложенные промежутки времени

Выполняется через указанное количество фреймов, является интерфейсом, реализуется следующим образом
```
   public void TakeDamage(Transform damageSourse, int damage)
   {
       if (_canTakeDamage)
       {
           _currentHealth = Mathf.Max(0, _currentHealth -= damage);
           _knockBack.GetKnockBack(damageSourse);
           _canTakeDamage = false;
           Debug.Log(_currentHealth);
//Запускаем отчет
           StartCoroutine(DamageRecoveryRoutine());
       }
      
   }
//Метод для отчета фреймов   
   private IEnumerator DamageRecoveryRoutine()
   {
       yield return new WaitForSeconds(_damageRecoveryTime);
       _canTakeDamage = true;
   }
```
Вызываем данный метод при атаке нашего персонажа противником
```
  //Метод при пересечении коллайдера
  private void OnTriggerStay2D(Collider2D collision)
  {
     if (collision.transform.TryGetComponent(out Player player))
      {
          player.TakeDamage(transform, _enemySO.EnemtDamageAmount);
      }
  }
```
