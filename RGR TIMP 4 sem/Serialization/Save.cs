using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace RGR_TIMP_4_sem.DanyaWork;

public class Save
{
    public bool SaveData(string directoryPath, string fileName, List<object> fileContent)
    {
        try
        {
            Directory.CreateDirectory(directoryPath);

            string uniqueFileName = GenerateUniqueName(directoryPath, fileName, "json");

            string filePath = Path.Combine(directoryPath, uniqueFileName);
            var JsonFormater = new JsonSerializerOptions
            {
                // Читаемый формат JSON
                WriteIndented = true,
                TypeInfoResolver = new DefaultJsonTypeInfoResolver()
            };
            var jsonContent = JsonSerializer.Serialize(fileContent, JsonFormater);
            File.WriteAllText(filePath, jsonContent);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Не удалось создать файл: {fileName} \n Ошибка: {ex}");
        }
        return false;
    }
    private string GenerateUniqueName(string directoryPath, string baseName, string extension)
    {
        int counter = 0;
        string fileName;
        do
        {
            // Формируем имя файла: Text1.txt, Text2.txt и т.д.
            fileName = $"{baseName}{counter}.{extension}";
            counter++;
        }
        while (File.Exists(Path.Combine(directoryPath, fileName)));

        return fileName;
    }
}
