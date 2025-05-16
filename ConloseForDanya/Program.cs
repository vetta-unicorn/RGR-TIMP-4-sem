using RGR_TIMP_4_sem.Models;
using RGR_TIMP_4_sem.DanyaWork;
using System.Collections.ObjectModel;
public class TestMain
{
    public static void TestSave()
    {
        var move = new LeftMove();
        ComandLine CL = new ComandLine(false, 35, move);
        Save save = new Save();
        const string fullPath = "C:\\Users\\Ko4erizhka\\source\\repos\\vetta-unicorn\\RGR-TIMP-4-sem\\ConloseForDanya\\TestJsons";
        bool saveData = save.SaveData(fullPath, "SaveLeftMove", CL);
        if (saveData)
        {
            Console.Write("Сохранение закончено :)\n");
        }
        else
        {
            Console.WriteLine("Не получилось сохранить :(");
        }
    }
    public static void TestLoad()
    {
        Load load = new Load();
        const string fullPath = "C:\\Users\\Ko4erizhka\\source\\repos\\vetta-unicorn\\RGR-TIMP-4-sem\\ConloseForDanya\\TestJsons";
        List<ComandLine> loadData = load.LoadData(fullPath, "*.json");
        foreach (var item in loadData)
        {
            Console.WriteLine($"{item.IsSelected}");
            Console.WriteLine($"{item.Number}");
            Console.WriteLine($"{item.Command.NameCommand}");
        }
    }
    public static void TestSaveCollection()
    {
        List<ComandLine> comandLines = new List<ComandLine>()
        {
            new ComandLine(true, 35, new LeftMove(), "jjhkgj", "jhugytj"),
            new ComandLine(false, 1, new RightMove()),
            new ComandLine(true, 123, new One())
        };
        Save save = new Save();
        const string fullPath = "C:\\Users\\Ko4erizhka\\source\\repos\\vetta-unicorn\\RGR-TIMP-4-sem\\ConloseForDanya\\TestJsons";
        var SaveData = save.SaveData(fullPath, "SavedCommandLines", comandLines);
        if (SaveData) { Console.WriteLine("Сохранено"); }
        else { Console.WriteLine("Error"); }
    }
    public static void TestLoadCollection()
    {
        // ВНИМАНИЕ!!!
        // Приведение к типу ObservableCollection<> нужно делать отдельно!!

        List<List<ComandLine>> comands = new List<List<ComandLine>>();
        Load load = new Load();
        
        const string fullPath = "C:\\Users\\Ko4erizhka\\source\\repos\\vetta-unicorn\\RGR-TIMP-4-sem\\ConloseForDanya\\TestJsons";
        List<ComandLine> loadData = load.LoadData(fullPath, "*.json");
        
        comands.Add(loadData);
        Console.WriteLine("Загруженный данные\n");
        foreach(var item1 in comands)
        {
            foreach(var item in item1)
            {
                Console.WriteLine($"Status: {item.IsSelected} Name: {item.Command.NameCommand} " +
                                  $"Number: {item.Number} Str: {item.Str} " +
                                  $"Comments: {item.Comments}\n" +
                                  $"-----------------------------------------------------");
            }
        }
    }
    public static void Main()
    {
        //TestSave();
        //TestLoad();
       TestSaveCollection();
       TestLoadCollection();
    }
}