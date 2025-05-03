using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGR_TIMP_4_sem.Interfaces
{
    public interface ICommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cells"> лист клеток </param>
        /// <param name="index"> индекс в глобальных координатах коорлдинатах</param>
        /// <returns></returns>
        public int Work(ObservableCollection<ICell> Cells);
    }
}
