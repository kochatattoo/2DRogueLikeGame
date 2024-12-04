#Unity 
Одним из способов передачи аргументов и реализации ответ одного объекта на действия другого являются события.
Реализуются следующим образом
```
using System;

public class Class
{
\\Объявляем событие
public event EventHandler OnEvent;
...
\\В необходимой точке кода вызываем данное событие
OnEvent?.Invoke(this,EventArgs.Empty);
}

\\В другом скрипте ,файле кода и тд.
{
...
Class _classElement;
private void start()
{
\\Подписываемся на событие в методе старт
_classElement.OnEvent+=_classElement_OnEvent;
}
\\Вводим метод вызываемый при событии в основном коде
private void _classElement_OnEvent(object sender, System.EventArgs e)
{
\\Код срабатываемый в данном исполнительном файле
}
}
```
Примерно таким образом происходит СОБЫТИЕ-ПОДПИСКА НА СОБЫТИЕ в среде unity и Csh