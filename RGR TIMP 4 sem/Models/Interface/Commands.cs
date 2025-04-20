using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGR_TIMP_4_sem.Models.Interface
{
    public enum CommandNames
    {
        None,
        Left,
        Right,
        One,
        Null,
        Question,
        Stop
    }

    public interface ICommand
    {
        CommandNames name { get; set; }
        void Start(CellModel cell); 
    }

    public interface IOperation
    {
        ICommand command { get; set; }

    }

    public class Left : ICommand
    {
        public CommandNames name {  get; set; }
        public Left() { }
        public void Start (CellModel cell)
        {
            //
        }
    }

    public class BinOperation : IOperation
    {
        public ICommand command { get; set; }

        public BinOperation(ICommand command)
        {
            this.command = command;
        }

    }

    public class Program
    {
        public void TestMain()
        {
            BinOperation operation1 = new BinOperation(new Left());
        }
    }
}
