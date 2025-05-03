using RGR_TIMP_4_sem.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGR_TIMP_4_sem.Models
{
    public class BinAlgoritm : IAlgoritm
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="indexMove"> сколько действий делать за 1 нажатие, если -1 то делать весь алгоритм</param>
        /// <param name="WorkingCell"> лист клеток</param>
        /// <param name="Table"> лист строк алгоритма</param>
        /// <param name="position"> текущая позиция в таблице</param>
        /// <returns>возвращает 1 если выполнено, 
        /// 0 если кончились строки, 
        /// -1 если бесконечный цикл</returns>
        public int Working (int indexMove, List <ICell> WorkingCell, List <ITable> Table, int position)
        {



            return 0;
        }
    }
}
