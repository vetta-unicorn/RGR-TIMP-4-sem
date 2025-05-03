using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using RGR_TIMP_4_sem.Interfaces;

namespace RGR_TIMP_4_sem.Models
{


    public class LeftMove: ICommand
    {

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

    public class Null : ICommand //ноль в клетку
    {

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
                    if (t.IsSelected)
                    {
                        break;
                    }
                    else { ind_list++; }
                }
                if (Cells[ind_list].Value == 1)
                { return 1; }

                else if (Cells[ind_list].Value == 0)
                { return 0; }

                else { return -1; }
            }
        }  
    }
}
    
    

