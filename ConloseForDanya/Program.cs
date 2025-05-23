using RGR_TIMP_4_sem.Models;
using RGR_TIMP_4_sem.DanyaWork;
using System.Collections.ObjectModel;
using RGR_TIMP_4_sem.Interfaces;
public class TestMain
{
    private static string controlDir = AppContext.BaseDirectory;
    private static string dataSavePath = Path.Combine(controlDir, "DataSave\\");
    public static void TestSave()
    {
        // Cells Profiles
        CellModel cellModel1 = new CellModel();
        cellModel1.IsSelected = true;
        cellModel1.Value = 4;
        cellModel1.Index = -8;

        CellModel cellModel2 = new CellModel();
        cellModel2.IsSelected = true;
        cellModel2.Value = 2;
        cellModel2.Index = 1;

        CellModel cellModel3 = new CellModel();
        cellModel3.IsSelected = true;
        cellModel3.Value = 5;
        cellModel3.Index = 4;

        CellModel cellModel4 = new CellModel();
        cellModel4.IsSelected = true;
        cellModel4.Value = 9;
        cellModel4.Index = 12;

        CellModel cellModel5 = new CellModel();
        cellModel5.IsSelected = false;
        cellModel5.Value = 154;
        cellModel5.Index = -12;

        // CommandLine Profiles
        CommandLine commandLine0 = new CommandLine(true, 35, new RightMove());
        CommandLine commandLine1 = new CommandLine(false, 5, new LeftMove());
        CommandLine commandLine2 = new CommandLine(true, 3, new One());
        CommandLine commandLine3 = new CommandLine(true, 666, new Zero());
        CommandLine commandLine4 = new CommandLine(true, 111, new Stop());

        List<object> list = new List<object>()
        {
            commandLine0, 
            cellModel1,
            commandLine1,
            cellModel2,
            commandLine2,
            cellModel3,
            commandLine3,
            cellModel4,
            commandLine4,
            cellModel5
        };

        Save save = new Save();
        bool saveData = save.SaveData(dataSavePath, "DataSave", list);

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
        var loadData = load.LoadData(dataSavePath, "*.json");
        Console.WriteLine(dataSavePath);
        if (loadData.Item3)
        {
            Console.WriteLine("Тут есь инфа");

            foreach (var item in loadData.Item1)
            {
                Console.WriteLine("-------------------------");
                Console.WriteLine("ICommandLine: ");
                Console.Write($"IsSelected: {item.IsSelected}  ");
                Console.Write($"Number: {item.Number}  ");
                Console.Write($"NameCommand: {item.Command.NameCommand}\n");
            }
            foreach (var item in loadData.Item2)
            {
                Console.WriteLine("---------------------------------");
                Console.WriteLine("ICell: ");
                Console.Write($"IsSelected: {item.IsSelected} ");
                Console.Write($"Value: {item.Value} ");
                Console.Write($"Index: {item.Index}\n");
            }
        }
        else
        {
            Console.WriteLine("А не работает((");
        }
    }

    public static void Main()
    {
        TestSave();
        TestLoad();
    }
}