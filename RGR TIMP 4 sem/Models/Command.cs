using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using RGR_TIMP_4_sem.Interfaces;

namespace RGR_TIMP_4_sem.Models
{


    public class LeftMove: ICommand
    {

        private readonly string name = "<-";
        public string NameCommand { get { return name; } }

        public override string ToString() => NameCommand;

        public int Work(ObservableCollection<ICell> Cells)
        {
            if (Cells == null)
            {
                throw new NullReferenceException("The cell list is empty");
            }
            else
            {
                int ind_list = 0;
                foreach (var t in Cells)
                {
                    if (t.Value != 0 && t.Value != 1)
                    {
                        throw new Exception("The cell value must be 0 or 1");
                    }
                    if (t.IsSelected)
                    {
                        break;
                    }
                    else { ind_list++; }
                }

                Cells[ind_list].IsSelected = false;
                Cells[ind_list - 1].IsSelected = true;
                return 1;
            }
        }
    }

    public class RightMove : ICommand
    {
        private readonly string name = "->";
        public string NameCommand { get { return name; } }

        public override string ToString() => NameCommand;

        public int Work(ObservableCollection<ICell> Cells)
        {
            if (Cells == null)
            {
                throw new NullReferenceException("The cell list is empty");
            }
            else
            {
                int ind_list = 0;
                foreach (var t in Cells)
                {
                    if (t.Value != 0 && t.Value != 1)
                    {
                        throw new Exception("The cell value must be 0 or 1");
                    }
                    if (t.IsSelected)
                    {
                        break;
                    }
                    else { ind_list++; }
                }

                Cells[ind_list].IsSelected = false;
                Cells[ind_list + 1].IsSelected = true;
                return 1;
            }
        }
    }

    public class One : ICommand //один в клетку
    {
        private readonly string name = "1";
        public string NameCommand { get { return name; } }

        public override string ToString() => NameCommand;

        public int Work(ObservableCollection<ICell> Cells)
        {
            if (Cells == null)
            {
                throw new NullReferenceException("The cell list is empty");
            }
            else
            {
                int ind_list = 0;
                foreach (var t in Cells)
                {
                    if (t.Value != 0 && t.Value != 1)
                    {
                        throw new Exception("The cell value must be 0 or 1");
                    }
                    if (t.IsSelected)
                    {
                        break;
                    }
                    else { ind_list++; }
                }

                Cells[ind_list].Value = 1;
                return 1;
            }
        }

    }

    public class Zero : ICommand //ноль в клетку
    {
        private readonly string name = "0";
        public string NameCommand { get { return name; } }

        public override string ToString() => NameCommand;

        public int Work(ObservableCollection<ICell> Cells)
        {
            if (Cells == null)
            {
                throw new NullReferenceException("The cell list is empty");
            }
            else
            {
                int ind_list = 0;
                foreach (var t in Cells)
                {
                    if (t.Value != 0 && t.Value != 1)
                    {
                        throw new Exception("The cell value must be 0 or 1");
                    }
                    if (t.IsSelected)
                    {
                        break;
                    }
                    else { ind_list++; }
                }

                Cells[ind_list].Value = 0;
                return 1;
            }
        }

    }

    public class Stop : ICommand
    {
        private readonly string name = "Stop";
        public string NameCommand { get { return name; } }

        public override string ToString() => NameCommand;

        public int Work(ObservableCollection<ICell> Cells)
        {
            if (Cells == null)
            {
                throw new NullReferenceException("The cell list is empty");
            }
            else { 
                return -1;
             }
         }
     }

    public class Question : ICommand
    {
        private readonly string name = "?";
        public string NameCommand { get { return name; } }
        public override string ToString() => NameCommand;
        public int Work(ObservableCollection<ICell> Cells)
        {
            if (Cells == null)
            {
                throw new NullReferenceException("The cell list is empty");
            }
            else
            {
                int ind_list = 0;
                foreach (var t in Cells)
                {
                    if (t.Value != 0 && t.Value != 1)
                    {
                        throw new Exception("The cell value must be 0 or 1");
                    }

                    if (t.IsSelected)
                    {
                        break;
                    }
                    else { ind_list++; }
                }
                if (Cells[ind_list].Value == 1)
                { return 0; }

                else if (Cells[ind_list].Value == 0)
                { return 1; }

                else { return -1; }
            }
        }  
    }
}
    
    

