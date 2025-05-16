using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using RGR_TIMP_4_sem.Models;

namespace RGR_TIMP_4_sem.DanyaWork
{
    public class Load
    {
        //Deserialize From Directory
        public List<ComandLine> LoadData(string directoryPath, string searchPattern)
        {
            List<ComandLine> items = new List<ComandLine>();
            if (Directory.Exists(directoryPath))
            {
                foreach (var filePath in Directory.GetFiles(directoryPath, searchPattern))
                {
                    try
                    {
                        string fileContent = File.ReadAllText(filePath);
                        List<ComandLine> deserializeItem = JsonSerializer.Deserialize<List<ComandLine>>(fileContent);
                        if (deserializeItem != null)
                        {
                            foreach (var item in deserializeItem)
                            {
                                items.Add(item);
                            }
                        }
                        else
                        {
                            throw new NullReferenceException("deserialize item is null");
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
