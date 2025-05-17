using RGR_TIMP_4_sem.Models;
using RGR_TIMP_4_sem.DanyaWork;
namespace TestProject1;

[TestClass]
public sealed class Test1
{
    [TestMethod]
    public void TestSerialize_ComandLine()
    {
        ComandLine CL = new ComandLine(false, 35, new LeftMove());
        Save save = new Save();
        Load load = new Load();
        const string fullPath = "C:\\Users\\Ko4erizhka\\source\\repos\\vetta-unicorn\\RGR-TIMP-4-sem\\TestProject1\\TestJsons\\";
        save.SaveData(fullPath, "SaveLeftMove", CL);
        var loadData = load.LoadData(fullPath, "*.json");
        
    }
    [TestMethod]
    public void TestDeserialize_ComandLine()
    { 

    }
}
