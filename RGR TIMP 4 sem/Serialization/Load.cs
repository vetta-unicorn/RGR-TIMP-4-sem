using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using RGR_TIMP_4_sem.Interfaces;
using RGR_TIMP_4_sem.Models;

namespace RGR_TIMP_4_sem.DanyaWork;

public class Load
{
    //Deserialize From Directory
    public (List<ICommandLine>, List<ICell>, bool) LoadData(string directoryPath, string searchPattern)
    {
        List<ICommandLine> commandItems = new List<ICommandLine>();
        List<ICell> cellItems = new List<ICell>();
        bool anyFilesProcessed = false;

        if (!Directory.Exists(directoryPath))
        {
            return (commandItems, cellItems, false);
        }

        foreach (var filePath in Directory.GetFiles(directoryPath, searchPattern))
        {
            try
            {
                string fileContent = File.ReadAllText(filePath);
                using var document = JsonDocument.Parse(fileContent);
                if (document.RootElement.ValueKind != JsonValueKind.Array)
                {
                    throw new Exception($"File {filePath} does not contain JSON array.");
                }

                var deserializeItems = JsonSerializer.Deserialize<List<JsonElement>>(fileContent);
                if (deserializeItems == null)
                {
                    throw new NullReferenceException("Deserialized item is null");
                }

                foreach (var item in deserializeItems)
                {
                    if (item.TryGetProperty("$type", out var typeProp))
                    {
                        if (typeProp.GetString() == "CellModel")
                        {
                            var cell = JsonSerializer.Deserialize<ICell>(item.GetRawText());
                            if (cell != null) cellItems.Add(cell);
                        }
                        else if (typeProp.GetString() == "CommandLine")
                        {
                            var command = JsonSerializer.Deserialize<CommandLine>(item.GetRawText());
                            if (command != null) commandItems.Add(command);
                        }
                    }
                }
                anyFilesProcessed = true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error with file processing {filePath}: {ex.Message}", ex);
            }
        }

        return (commandItems, cellItems, anyFilesProcessed);
    }

    //public static class SerializationSettings
    //{
    //    public static readonly JsonSerializerOptions PolymorphicOptions = new()
    //    {
    //        TypeInfoResolver = MyPolymorphicContext.Default
    //    };
    //}


    //public (List<ICommandLine>, List<ICell>, bool) LoadData(string directoryPath, string searchPattern)
    //{
    //    List<ICommandLine> commandItems = new List<ICommandLine>();
    //    List<ICell> cellItems = new List<ICell>();
    //    bool anyFilesProcessed = false;

    //    if (!Directory.Exists(directoryPath))
    //    {
    //        return (commandItems, cellItems, false);
    //    }

    //    foreach (var filePath in Directory.GetFiles(directoryPath, searchPattern))
    //    {
    //        try
    //        {
    //            string fileContent = File.ReadAllText(filePath);
    //            Сначала попробуем как список ICell
    //           var cells = JsonSerializer.Deserialize<List<ICell>>(fileContent, new JsonSerializerOptions
    //           {
    //               TypeInfoResolver = MyPolymorphicContext.Default
    //           });
    //            if (cells != null && cells.Count > 0)
    //            {
    //                cellItems.AddRange(cells);
    //                anyFilesProcessed = true;
    //                continue;
    //            }
    //            Если не клетки, возможно, команды
    //           var commands = JsonSerializer.Deserialize<List<ICommandLine>>(fileContent, new JsonSerializerOptions
    //           {
    //               TypeInfoResolver = MyPolymorphicContext.Default
    //           });
    //            if (commands != null && commands.Count > 0)
    //            {
    //                commandItems.AddRange(commands);
    //                anyFilesProcessed = true;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new Exception($"Error with file processing {filePath}: {ex.Message}", ex);
    //        }
    //    }

    //    return (commandItems, cellItems, anyFilesProcessed);
    //}

}
