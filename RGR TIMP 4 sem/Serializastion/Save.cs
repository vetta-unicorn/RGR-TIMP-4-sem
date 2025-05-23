using System;
using System.IO;
using System.Text.Json;

namespace RGR_TIMP_4_sem.DanyaWork
{
    public class Save
    {
        public bool SaveData(string directoryPath, string fileName, dynamic fileContent)
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

                    // Сохраняет кодировку текста в "читабильном" виде
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                };
                var jsonContent = JsonSerializer.Serialize(fileContent, JsonFormater);
                File.WriteAllText(filePath, jsonContent);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Не удалось создать файл: {fileName} \n Ошибка: {ex}");
            }
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
}
