using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RGR_TIMP_4_sem.Interfaces;
using RGR_TIMP_4_sem.Models;

namespace RGR_TIMP_4_sem.ViewModels
{
    public class CommandList
    {
        public static CommandList Instance { get; } = new CommandList();

        public List<ICommand> Commands { get; }

        public CommandList()
        {
            Commands = new List<ICommand>
            {
                new LeftMove(),
                new RightMove(),
                new One(),
                new Zero(),
                new Stop(),
                new Question()
            };
        }
    }

    public class CommandFunc
    {
        ObservableCollection<ICommandLine> lines;

        public CommandFunc(ObservableCollection<ICommandLine> lines)
        {
            this.lines = lines;
        }

        public int FindSelectedLine()
        {
            for(int i = 0; i < lines.Count; i++)
            {
                if (lines[i].IsSelected) return i;
            }
            return -1;
        }
    }

}
