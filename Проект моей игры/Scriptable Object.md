#Unity
Данные только считываются, но не записываются

-Объекты в юнити которые позволяют хранить данные и передавать их в виде скрипта, делать некий config

-Некий файл конфигурации в котором содержатся данные которые только считываются.

(Можно поменять в одном месте что бы изменить во всех других местах)

В данном проекте реализую следующим образом
```
}
using UnityEngine;

[CreateAssetMenu()]

public class EnemySO : ScriptableObject
{
    public string enemyName;
    public int enemyHealth;
    public int EnemtDamageAmount;
}
```

[CreateAssetMenu()]
Добавляет поле в Create-EnemySO который явялется Script в поле Project