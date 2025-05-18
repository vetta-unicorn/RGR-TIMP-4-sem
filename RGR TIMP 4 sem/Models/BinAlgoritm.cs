using DynamicData.Binding;
using RGR_TIMP_4_sem.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RGR_TIMP_4_sem.Models
{
    public class BinAlgoritm : IAlgoritm
    {

        /// <summary>
        /// Бинарный алгоритм
        /// </summary>
        /// <param name="indexMove"> до какой строки делать за 1 нажатие, если -1 то делать весь алгоритм</param>
        /// <param name="WorkingCell"> ObservableCollection клеток (видимых) </param>
        /// <param name="Table"> ObservableCollection строк алгоритма</param>
        /// <returns>возвращает строку если выполнено, 
        /// 0 если кончились строки, 
        /// -1 если бесконечный цикл</returns>
        public string Working(int indexMove, ObservableCollection<ICell> Cells, ObservableCollection<IComandLine> ComandLine)
        {
            if (Cells == null)
            {
                throw new NullReferenceException("The cell list is empty");
            }
            else if (ComandLine == null)
            {
                throw new NullReferenceException("The Comands list is empty");
            }
            else
            {

                int flag = 1;
                int count_cycle = 0;
                while (flag != -1)
                {
                    int now = Number_of_SelectedStr(ComandLine);
                    if (now == indexMove)
                    {
                        return ($"The commands were successfully completed - {indexMove}");
                    }
                    if (count_cycle == 1000)
                    {
                        return "The endless loop";
                    }
                    try
                    {
                        if (ComandLine[now].Command is Question)
                        {
                            flag = ComandLine[now].Command.Work(Cells);
                            int[] mass = Split(ComandLine[now].Str);
                            switchNumberLine(ComandLine, mass[flag]);
                            count_cycle++;
                        }
                        else
                        {
                            flag = ComandLine[now].Command.Work(Cells);
                            switchNumberLine(ComandLine, now + 1);
                        }
                    }
                    catch (NullReferenceException ex)
                    {
                        throw new NullReferenceException(ex.Message);
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        throw new ArgumentOutOfRangeException(ex.Message);
                    }

                }
                return " ";
            }
        }

        public int[] Split(string str)
        {
            string[] parts = str.Split(',');
            int[] mass = new int [2];
            for(int i =0; i<2; i++)
            {
                mass[i] = Convert.ToInt32(parts[i]);
            }
            return mass;
        }


        
        public int Number_of_SelectedStr(ObservableCollection<IComandLine> ComandLine)
        {
            if (ComandLine == null)
            {
                throw new NullReferenceException("The Comands list is empty");
            }
            else
            {
                int index = 0;
                foreach (var t in ComandLine)
                {
                    if (t.IsSelected)
                    {
                        break;
                    }
                    else { index++; }
                }
                return index;
            }
        }

        //пофиксить выход за пределы массива ObservableCollection<IComandLine> ? 1,7 когда всего 6 строк
        public void switchNumberLine(ObservableCollection<IComandLine> ComandLine, int index)
        {
            if (ComandLine == null)
            {
                throw new NullReferenceException("The Comands list is empty");
            }
            else
            {
                if (index < 0 || index >= ComandLine.Count)
                {
                    throw new ArgumentOutOfRangeException("Index is out of range");
                }
                else
                {
                    foreach (var t in ComandLine)
                    {
                        if (t.Number == index)
                        {
                            t.IsSelected = true;
                            break;
                        }
                        else { t.IsSelected = false; }
                    }
                }
            }
        }
    }
}
