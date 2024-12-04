Для сохранения процесса игры - следует подготовить такой объект как SaveLoadManger
Сперва напишем скрипт для нашего сохранения. 
Сохранять будет в файл
##### Сохранение

```
public static void SaveUser(User user)
{
    BinaryFormatter formatter = new BinaryFormatter();
    string path = Application.persistentDataPath + "/user.data";
    FileStream stream = new FileStream(path, FileMode.Create);
    
    formatter.Serialize(stream, GameManager.Instance.user);
    stream.Close();
    Debug.Log("Save Complete");
}
```
Данный метод преобразует данные класса User в бинарный код и записывает их в локальный файл

-Создаем бинарный форматер
-Записываем путь сохранения
-Открываем поток для сохранения
-Записываем данные
-Закрываем поток записи
-Выводим сообщение в консоль о успешной записи

##### Загрузка
```
public static User LoadUser()
{
    string path = Application.persistentDataPath + "/user.data";
    if (File.Exists(path))
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Open);

        User data = formatter.Deserialize(stream) as User;
        stream.Close();
        Debug.Log("Save has loaded");

        return data;
    }
    else
    {
        Debug.LogError("Save file not found in" + path);
        return null;
    }
}
```
Данный метод считывает данные из файла, преобразует их в исходные значения и записывает их в переменные класса User
-Указываем путь к файлу
-Проверяем существует ли файл (если нет выводим ошибку и возвращаем ничего)
-Создаем бинарный форматер
-Открываем поток чтения
-Десериализуем данные с файла и преобразуем их в класс User
-Закрываем поток чтения
-Выводи сообщение о том что файл считан

##### Очистка
```
public static User ResetData()
{
    User user = new User();
    string path = Application.persistentDataPath + "/user.data";

    if (File.Exists(path))
    {
       File.Delete(path);
        Debug.Log("Data reset complete");
        return user;
    }
    else
    {
        Debug.LogError("Save file not found in" + path);
        return null;
    }
}
```
Данный метод очищает файл сохранения

-Указываем новою переменную класса User и указываем путь к файлу
-Проверяем существует ли файл(Если нет то и нечего удалять)
-Очищаем файл
-Выводим сообщение
-Выводим новый класс User


Дальнейшая реализация *Save&Load* происходит в следующих скриптах
[[GUI Пользователя]]
[[User]]
[[PauseMenu]]

21.11.24
При внесении изменений касательно отображения сохраненных профилей пользователей в меню LOAD в скрипт были добавлены следующий методы и переменные в начале скрипта 

```
public string saveDirectory;
public static SaveManager Instance;

private void Start()
{
    Instance = this;
    saveDirectory=Application.persistentDataPath+"/saves/";

    if(!Directory.Exists(saveDirectory))
    {
        Directory.CreateDirectory(saveDirectory);
    }
}

public void SaveGame(User user, string fileName)
{
    string json =JsonUtility.ToJson(user);
    File.WriteAllText(saveDirectory+fileName+".json", json);
    Debug.Log("Game saved to "+saveDirectory+fileName+".json");
}

public User LoadGame(string fileName)
{
    string path = saveDirectory + fileName + ".json";
    if (File.Exists(path))
    {
        string json = File.ReadAllText(path);
        User user = JsonUtility.FromJson<User>(json);
        Debug.Log("Game loaded from " + path);
        return user;
    }
    else
    {
        Debug.LogError("Save file not found at " + path);
        return null;
    }
}..... дальнейшая часть кода
```


## Запишем скрипт с комментариями 
```
using UnityEngine; // Импортируем пространство имен Unity для доступа к основным классам и структурам
using System.IO; // Импортируем пространство имен для работы с файловой системой
using System.Collections.Generic; // Импортируем пространство имен для работы с коллекциями, такими как List

public class SaveManager : MonoBehaviour // Определяем класс SaveManager, который наследует MonoBehaviour и может взаимодействовать с Unity
{
    public string saveDirectory; // Переменная для хранения пути к директории, где будут располагаться файлы сохранений
    public static SaveManager Instance; // Статическая переменная для хранения единственного экземпляра SaveManager (паттерн Singleton)

    private void Start() // Метод, который вызывается при запуске скрипта
    {
        Instance = this; // Присваиваем текущий экземпляр (this) статической переменной Instance
        saveDirectory = Application.persistentDataPath + "/saves/"; // Задаем путь к директории для сохранений

        // Проверяем, существует ли директория, и если нет, создаем ее
        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory); // Создаем директорию для сохранений
        }
    }

    // Метод для сохранения данных игрового персонажа (User) в файл
    public void SaveGame(User user, string fileName)
    {
        string json = JsonUtility.ToJson(user); // Преобразуем объект User в формат JSON
        File.WriteAllText(saveDirectory + fileName + ".json", json); // Записываем JSON-строку в файл с именем fileName
        Debug.Log("Game saved to " + saveDirectory + fileName + ".json"); // Логируем сообщение о сохранении
    }

    // Метод для загрузки данных игрового персонажа из файла
    public User LoadGame(string fileName)
    {
        string path = saveDirectory + fileName + ".json"; // Формируем полный путь к файлу сохранения
        if (File.Exists(path)) // Проверяем, существует ли файл
        {
            string json = File.ReadAllText(path); // Читаем содержимое файла в строку
            User user = JsonUtility.FromJson<User>(json); // Преобразуем JSON-строку обратно в объект User
            Debug.Log("Game loaded from " + path); // Логируем сообщение о загрузке
            return user; // Возвращаем загруженный объект User
        }
        else
        {
            Debug.LogError("Save file not found at " + path); // Логируем ошибку, если файл не найден
            return null; // Возвращаем null, если загрузка не удалась
        }
    }

    // Метод для сохранения данных пользователя в бинарный формат (временная реплика, на данный момент не используется)
    public static void SaveUser(User user)
    {
        BinaryFormatter formatter = new BinaryFormatter(); // Создаем экземпляр BinaryFormatter для сериализации
        string path = Application.persistentDataPath + "/user.data"; // Определяем путь для сохранения данных пользователя
        FileStream stream = new FileStream(path, FileMode.Create); // Создаем поток для записи в файл
        
        // Сериализуем объект user в поток
        formatter.Serialize(stream, GameManager.Instance.user); 
        stream.Close(); // Закрываем поток
        Debug.Log("Save Complete"); // Логируем сообщение о завершении сохранения
    }

    // Метод для загрузки данных пользователя из бинарного файла
    public static User LoadUser()
    {
        string path = Application.persistentDataPath + "/user.data"; // Определяем путь к файлу
        if (File.Exists(path)) // Проверяем, существует ли файл
        {
            BinaryFormatter formatter = new BinaryFormatter(); // Создаем форматтер
            FileStream stream = new FileStream(path, FileMode.Open); // Открываем поток для чтения файла

            User data = formatter.Deserialize(stream) as User; // Десериализуем объект user из потока
            stream.Close(); // Закрываем поток
            Debug.Log("Save has loaded"); // Логируем сообщение о загрузке

            return data; // Возвращаем загруженные данные пользователя
        }
        else
        {
            Debug.LogError("Save file not found in " + path); // Логируем ошибку, если файл не найден
            return null; // Возвращаем null, если загрузка не удалась
        }
    }

    // Метод для сброса данных пользователя
    public static User ResetData()
    {
        User user = new User(); // Создаем новый объект User
        string path = Application.persistentDataPath + "/user.data"; // Определяем путь к файлу

        if (File.Exists(path)) // Проверяем, существует ли файл
        {
            File.Delete(path); // Удаляем файл
            Debug.Log("Data reset complete"); // Логируем сообщение о сбросе данных
            return user; // Возвращаем новый объект User
        }
        else
        {
            Debug.LogError("Save file not found in " + path); // Логируем ошибку, если файл не найден
            return null; // Возвращаем null, если сброс не удался
        }
    }

    // Новый метод для получения списка всех сохраненных файлов
    public List<string> GetSaveFileNames()
    {
        List<string> saveFileNames = new List<string>(); // Создаем список для хранения имен файлов
        string[] files = Directory.GetFiles(saveDirectory, "*.json"); // Получаем массив файлов с расширением .json в директории

        foreach (string file in files) // Проходим по всем найденным файлам
        {
            saveFileNames.Add(Path.GetFileNameWithoutExtension(file)); // Добавляем имя файла (без расширения) в список
        }

        return saveFileNames; // Возвращаем список имен файлов
    }

    // Новый метод для удаления файла сохранения по имени
    public void DeleteSaveFile(string fileName)
    {
        string filePath = Path.Combine(saveDirectory, fileName + ".json"); // Формируем полный путь к файлу

        if (File.Exists(filePath)) // Проверяем, существует ли файл
        {
            File.Delete(filePath); // Если да, удаляем файл
            Debug.Log($"{fileName}.json был удален!"); // Логируем сообщение об удалении
        }
        else
        {
            Debug.LogError($"Файл сохранения {fileName}.json не найден."); // Логируем ошибку, если файл не найден
        }
    }
}
```
Краткое описание 
- **using UnityEngine**: Подключает пространство имен Unity, которое предоставляет базовую функциональность для работы с игровыми объектами.
- **using System.IO**: Подключает пространство имен для работы с файлами и директориями на диске.
- **using System.Collections.Generic**: Подключает пространство имен, позволяющее использовать обобщенные коллекции, такие как `List`.
- **public class SaveManager : MonoBehaviour**: Объявляет новый класс `SaveManager`, который наследует от `MonoBehaviour`, позволяя использовать его в Unity и взаимодействовать с игровыми компонентами.
- **public string saveDirectory**: Объявляет переменную для хранения пути к директории сохранений.
- **public static SaveManager Instance**: Создает статическую реализацию паттерна Singleton, чтобы легко получать доступ к экземпляру `SaveManager`.
- **private void Start()**: Метод, вызываемый при запуске игры, используется для инициализации объекта.
- **if (!Directory.Exists(saveDirectory))**: Проверяет, существует ли директория для сохранений.
- **Directory.CreateDirectory(saveDirectory)**: Создает директорию для сохранений, если она не существует.
- **public void SaveGame(User user, string fileName)**: Метод для сохранения объекта `User` в файл с заданным именем в формате JSON.
- **JsonUtility.ToJson(user)**: Преобразует объект `User` в JSON-строку.
- **File.WriteAllText(saveDirectory + fileName + ".json", json)**: Записывает JSON-строку в файл с заданным именем.
- **public User LoadGame(string fileName)**: Метод для загрузки объекта `User` из файла сохранения по имени.
- **if (File.Exists(path))**: Проверяет, существует ли файл перед его чтением.
- **public static void SaveUser(User user)**: Статический метод для сохранения данных пользователя в бинарном формате (на данный момент может быть ненужным).
- **public static User LoadUser()**: Статический метод для загрузки бинарных данных пользователя.
- **public static User ResetData()**: Статический метод для сброса данных пользователя.
- **public List(string) GetSaveFileNames()**: Метод для получения списка всех сохраненных файлов.
- **public void DeleteSaveFile(string fileName)**: Метод для удаления файла сохранения по имени.


### Автозагрузка последнего сохранения
Чтобы реализовать метод, который бы автоматически выбирал последний созданный файл сохранения и использовал его для загрузки пользователя без необходимости указывать имя файла, вы можете сделать несколько шагов в вашем классе `SaveManager`.

### Шаги для реализации

1. **Создайте метод для получения последнего сохраненного файла**.
2. **Измените метод `LoadGame` таким образом, чтобы он использовал этот новый метод**.
3. **Обеспечьте доступ к этому методу из других скриптов**.

### Пример реализации

Вот обновленный код с добавлением метода для автоматической загрузки последнего сохраненного файла:
```
using UnityEngine;
using System.IO;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    public string saveDirectory;
    public static SaveManager Instance;

    private void Start()
    {
        Instance = this;
        saveDirectory = Application.persistentDataPath + "/saves/";

        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
        }
    }

    public void SaveGame(User user, string fileName)
    {
        string json = JsonUtility.ToJson(user);
        File.WriteAllText(saveDirectory + fileName + ".json", json);
        Debug.Log("Game saved to " + saveDirectory + fileName + ".json");
    }

    public User LoadGame(string fileName)
    {
        string path = saveDirectory + fileName + ".json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            User user = JsonUtility.FromJson<User>(json);
            Debug.Log("Game loaded from " + path);
            return user;
        }
        else
        {
            Debug.LogError("Save file not found at " + path);
            return null;
        }
    }

    // Новый метод для загрузки последнего сохраненного файла
    public User LoadLastGame()
    {
        string lastFileName = GetLastSaveFileName(); // Получаем имя последнего сохраненного файла
        if (lastFileName != null)
        {
            return LoadGame(lastFileName); // Загружаем игру по имени файла
        }
        Debug.LogError("No save files found."); // Если нет файлов, выводим ошибку
        return null; // Возвращаем null, если не удалось загрузить
    }

    // Метод для получения имени последнего сохраненного файла
    private string GetLastSaveFileName()
    {
        if (!Directory.Exists(saveDirectory))
        {
            return null; // Если директория не существует, возвращаем null
        }

        // Получаем все файлы .json в директории и сортируем их по времени создания
        var files = Directory.GetFiles(saveDirectory, "*.json")
                             .Select(f => new FileInfo(f))
                             .OrderByDescending(fi => fi.LastWriteTime)
                             .ToList();

        return files.Count > 0 ? Path.GetFileNameWithoutExtension(files[0].FullName) : null; // Возвращаем имя последнего файла без расширения
    }

    // Остальные методы ...
}
```
### Объяснение изменений:

1. **Метод `LoadLastGame()`**:
    
    - Этот метод не требует указания имени файла. Он вызывает `GetLastSaveFileName()` для получения имени последнего сохраненного файла и затем вызывает `LoadGame()` с этим именем.
2. **Метод `GetLastSaveFileName()`**:
    
    - Этот метод отвечает за получение имени последнего файла сохранения. Он получает все файлы .json из директории сохранений, сортирует их по времени последнего изменения и возвращает имя файла с самым последним изменением. Если файлов нет, возвращается `null`.

### Использование

Теперь вы можете использовать метод `LoadLastGame()` из других скриптов, не указывая имя файла:
```
User loadedUser = SaveManager.Instance.LoadLastGame();
if (loadedUser != null)
{
    // Обработка загруженного пользователя
    Debug.Log("Last user loaded: " + loadedUser.GetName());
}
```
