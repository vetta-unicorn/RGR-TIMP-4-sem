using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RGR_TIMP_4_sem.Models;

namespace RGR_TIMP_4_sem.DanyaWork
{
    public class Load
    {
        //Deserialize From Directory
        public List<strInTable> LoadData(string directoryPath, string searchPattern)
        {
            List<strInTable> items = new List<strInTable>();
            if (Directory.Exists(directoryPath))
            {
                foreach (var filePath in Directory.GetFiles(directoryPath, searchPattern))
                {
                    try
                    {
                        string fileContent = File.ReadAllText(filePath);
                        strInTable deserializeItem = JsonSerializer.Deserialize<strInTable>(fileContent);
                        if (deserializeItem != null)
                        {
                            items.Add(deserializeItem);
                        }
                    }
                    catch (Exception ex)
                    {
                       throw new Exception($"Ошибка при обработки файла {filePath}:{ex.Message}");
                    }
                }
            }
            else
            {
                throw new Exception("Данная директория не найдена!");
            }
            return items;
        }
    }
}
