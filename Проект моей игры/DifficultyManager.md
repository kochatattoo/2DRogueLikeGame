Чтобы создать скрипт, который будет управлять изменением уровня сложности в вашем окне опций (Options), вы можете следовать этим шагам. Мы создадим класс, который будет отвечать за настройки сложности, а затем добавим его к окну опций.

### Шаг 1: Создание класса DifficultyManager

Создайте новый скрипт и назовите его `DifficultyManager.cs`. Этот класс будет содержать логику, связанную с уровнем сложности игры.
```
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DifficultyManager : MonoBehaviour
{
    public enum Difficulty { Easy, Medium, Hard }
    public Difficulty currentDifficulty;

    public TextMeshProUGUI difficultyText; // Текст для отображения текущего уровня сложности
    public Button increaseDifficultyButton; // Кнопка для увеличения сложности
    public Button decreaseDifficultyButton; // Кнопка для уменьшения сложности

    private void Start()
    {
        // Устанавливаем начальное значение сложности
        currentDifficulty = Difficulty.Medium; // Установите значение по умолчанию
        UpdateDifficultyText();

        // Подписка на события кнопок
        increaseDifficultyButton.onClick.AddListener(IncreaseDifficulty);
        decreaseDifficultyButton.onClick.AddListener(DecreaseDifficulty);
    }

    private void UpdateDifficultyText()
    {
        difficultyText.text = "Difficulty: " + currentDifficulty.ToString(); // Обновляем текст
    }

    public void IncreaseDifficulty()
    {
        if (currentDifficulty < Difficulty.Hard)
        {
            currentDifficulty++; // Увеличиваем сложность
            UpdateDifficultyText(); // Обновляем текст
        }
    }

    public void DecreaseDifficulty()
    {
        if (currentDifficulty > Difficulty.Easy)
        {
            currentDifficulty--; // Уменьшаем сложность
            UpdateDifficultyText(); // Обновляем текст
        }
    }
}
```
### Шаг 2: Настройка интерфейса в Unity

1. **Создайте UI для окна опций**:
    
    - В вашем окне опций (`Options UI`), создайте текстовое поле для отображения текущего уровня сложности.
    - Создайте две кнопки: одну для увеличения сложности и другую для уменьшения.
2. **Настройте Links**:
    
    - Перетащите созданные элементы UI (текст и кнопки) в соответствующие публичные поля скрипта `DifficultyManager` в инспекторе Unity.

### Шаг 3: Подключение DifficultyManager к Options UI

На вашем объекте, представляющем окно опций, добавьте компонент `DifficultyManager`, чтобы он мог управлять настройками сложности.

### Шаг 4: Использование текущего уровня сложности в игре

Теперь, когда вы установили класс `DifficultyManager`, вам нужно убедиться, что в других частях вашего кода вы можете получить доступ к текущему уровню сложности и соответственно изменять сложность игрового процесса. Например:
```
public class GameManager : MonoBehaviour
{
    public DifficultyManager difficultyManager;

    private void Start()
    {
        // Получаем доступ к уровню сложности при старте игры
        Debug.Log("Current difficulty: " + difficultyManager.currentDifficulty);
        // Используйте значение уровня сложности для настройки сложности игры
    }
}
```
### Заключение

Теперь у вас есть класс `DifficultyManager`, который управляет изменением уровня сложности в окне опций. Вы также можете использовать значение текущей сложности в других частях вашего игрового процесса, чтобы адаптировать сложность под текущие параметры.