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

    public static class CommandToStringConverter
    {
        public static string CommandToString(ICommand command)
        {
            return command.NameCommand;
        }
    }

    public class CommandFunc
    {
        public int FindSelectedLine(ObservableCollection<ICommandLine> lines)
        {
            for(int i = 0; i < lines.Count; i++)
            {
                if (lines[i].IsSelected) return i;
            }
            return -1;
        }

        public int CountVisibleRows(ObservableCollection<ICommandLine> lines)
        {
            int count = 0;
            foreach (var line in lines)
            {
                if (line.IsVisible) count++;
            }
            return count;
        }

        public int FindFirstVisible(ObservableCollection<ICommandLine> lines)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].IsVisible) return i;
            }

            return -1;
        }

        public int FindLastVisible(ObservableCollection<ICommandLine> lines)
        {
            for (int i = lines.Count - 1; i >= 0; i--)
            {
                if (lines[i].IsVisible) return i;
            }
            return -1;
        }
    }

}
