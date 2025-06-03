using RGR_TIMP_4_sem.Interfaces;
using RGR_TIMP_4_sem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGR_TIMP_4_sem.ViewModels
{
    public class CellViewModel
    {
        ObservableCollection<ICell> Cells;
        public CellViewModel(ObservableCollection<ICell> cells)
        {
            Cells = cells;
        }

        public int FindSelectedCell()
        {
            for (int i = 0; i < Cells.Count(); i++)
            {
                if (Cells[i].IsSelected) return i;
            }
            return -1;
        }

        public int FindCellByIndex(int index)
        {
            for (int i = 0; i < Cells.Count();i++)
            {
                if (Cells[i].Index == index) return i;
            }
            return -1;
        }

        public int FindLeftVisible()
        {
            for (int i = 0; i < Cells.Count(); i++)
            {
                if (Cells[i].IsVisible) return i;
            }
            return -1;
        }

        public int FindRightVisible()
        {
            for (int i = Cells.Count() - 1; i >= 0; i--)
            {
                if (Cells[i].IsVisible) return i;
            }
            return -1;
        }

        public static void InitializeCells(ObservableCollection<ICell> cells, int number)
        {
            // Определяем смещение для индексов
            int offset = number / 2;

            for (int i = 0; i < number; i++)
            {
                cells.Add(new CellModel());
                cells[i].Index = i - offset; // Устанавливаем индекс
            }
        }

    }
}
