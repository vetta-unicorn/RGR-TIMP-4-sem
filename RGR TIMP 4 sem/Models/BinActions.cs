using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RGR_TIMP_4_sem.Interfaces;

namespace RGR_TIMP_4_sem.Models
{


    public class LeftMove: ICommand
    {

        public int Work(ICell cell)
        {
            if(cell.IsSelected)
            {
                cell.IsSelected = false;
                return 1;
            }
            else {
                cell.IsSelected = true;
                return 1;
            }
           
        }

    }
    public class RightMove : IBinCommand
    {

        public int Work(ICell cell)
        {
            if (cell.IsSelected)
            {
                cell.IsSelected = false;
                return 1;
            }
            else
            {
                cell.IsSelected = true;
                return 1;
            }
            
        }

    }

    public class One : IBinCommand //один в клетку
    {

        public int Work(ICell cell)
        {
            cell.Value = 1;
            return 1;
        }

    }

    public class Null : IBinCommand //ноль в клетку
    {

        public int Work(ICell cell)
        {
            cell.Value = 0;
            return 1;
        }

    }

    public class Stop : IBinCommand
    {

        public int Work(ICell cell)
        {
            return -1;
        }

    }

    public class Question : IBinCommand
    {

        public int Work(ICell cell)
        {
            if (cell.Value == 1)
            { return 1;}

            else if (cell.Value == 0)
            { return 0; }

            else{ return -1;}
        }
        

    }
    
    
}
