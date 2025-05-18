using System;
using System.Collections.Generic;
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

}
