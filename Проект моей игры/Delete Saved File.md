В скрипт [[SaveLoadManager]] добавил следующие методы
```
    // Новый метод для получения списка файлов сохранений
    public List<string> GetSaveFileNames()
    {
        List<string> saveFileNames = new List<string>();
        string[] files = Directory.GetFiles(saveDirectory, "*.json");
        
        foreach (string file in files)
        {
            saveFileNames.Add(Path.GetFileNameWithoutExtension(file)); // Добавляем имя файла без расширения
        }

        return saveFileNames;
    }

    // Новый метод для удаления файла сохранения по имени
    public void DeleteSaveFile(string fileName)
    {
        string filePath = Path.Combine(saveDirectory, fileName + ".json");
        
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log($"{fileName}.json был удален!");
        }
        else
        {
            Debug.LogError($"Файл сохранения {fileName}.json не найден.");
        }
    }
```

И добавим в скрипт [[SaveLoadMenu]] 
