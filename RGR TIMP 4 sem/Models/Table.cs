using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RGR_TIMP_4_sem.Interfaces;

namespace RGR_TIMP_4_sem.Models
{
    public class strInTable: ITable
    {
        private int number;
        private ICommand command;
        private string? str;
        private string? comments;

        public int Number
        {
            get { return number; }
            set { number = value; }
        }
        public ICommand Command
        {
            get { return command; }
            set { command = value; }
        }

        public string? Str
        {
            get { return str; }
            set { str = value; }
        }
        public string? Comments
        {
            get { return comments; }
            set { comments = value; }
        }
        public strInTable(int number, ICommand command, string? str, string? comments)
        {
            this.number = number;
            this.command = command;
            this.str = str;
            this.comments = comments;
        }

        public strInTable(int number, ICommand command)
        {
            this.number = number;
            this.command = command;
        }
    }
}
