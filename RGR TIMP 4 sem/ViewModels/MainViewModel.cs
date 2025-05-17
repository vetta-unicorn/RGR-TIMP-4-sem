using System.Collections.ObjectModel;
using System.Reactive;
using ReactiveUI;
using RGR_TIMP_4_sem.Interfaces;
using RGR_TIMP_4_sem.Models;

namespace RGR_TIMP_4_sem.ViewModels;

public class MainViewModel : ReactiveObject
{
    public ReactiveCommand<Unit, Unit> ButtonClickCommandLeft { get; }
    public ReactiveCommand<Unit, Unit> ButtonClickCommandRight { get; }

    // здесь будут храниться ячейки, которые показываются на экране
    public ObservableCollection<ICell> Cells { get; }

    // здесь будут показываться ВСЕ ячейки
    public ObservableCollection<ICell> ExtendedCells { get; }
    public int cell_num { get; set; }
    public int all_cell_num { get; set; }

    //индекс в глобальных координатах
    public int current_index { get; set; }

    public MainViewModel()
    {
        // команды для кнопок
        ButtonClickCommandLeft = ReactiveCommand.Create(OnButtonClickLeft);
        ButtonClickCommandRight = ReactiveCommand.Create(OnButtonClickRight);

        cell_num = 19;
        all_cell_num = 201;

        Cells = new ObservableCollection<ICell>();
        ExtendedCells = new ObservableCollection<ICell>();

        CellInitialize(Cells, cell_num);
        CellInitialize(ExtendedCells, all_cell_num);

        SetCell(3, 1);
        SetCell(-1, 1);
        SetCell(6, 1);
        SetCell(2, 1);
        SetCell(-5, 1);
        SetCell(8, 1);

        current_index = 0;
        SelectCell(current_index);
    }

    private void OnButtonClickLeft()
    {
        for (int i = 0; i < cell_num; i++)
        {
            Cells[i].Index--;

            foreach (var _cell in ExtendedCells)
            {
                if (_cell.Index == Cells[i].Index)
                {
                    Cells[i].Value = _cell.Value;
                    break;
                }
            }
        }
        SelectCell(current_index);
    }

    private void OnButtonClickRight()
    {
        for (int i = 0; i < cell_num; i++)
        {
            Cells[i].Index++;

            foreach (var _cell in ExtendedCells)
            {
                if (_cell.Index == Cells[i].Index)
                {
                    Cells[i].Value = _cell.Value;
                    break;
                }
            }
        }
        SelectCell(current_index);
    }

    public void CellInitialize(ObservableCollection<ICell> _Cells, int _cell_num)
    {
        for (int i = 0; i < _cell_num; i++)
        {
            ICell a = new CellModel();
            _Cells.Add(a);
            if (i == cell_num / 2)
            {
                _Cells[i].Index = 0;
            }
            else if (i < cell_num / 2)
            {
                _Cells[i].Index = -_cell_num / 2 + i;
            }
            else
            {
                _Cells[i].Index = i - _cell_num / 2;
            }
        }
    }


    public void SetCell(int index, int val)
    {
        int maxIndex = FindMaxIndex();
        int minIndex = FindMinIndex();
        for (int i = 0; i < all_cell_num; i++)
        {
            if (ExtendedCells[i].Index == index)
            {
                ExtendedCells[i].Value = val;
            }
        }

        for (int i = 0; i < cell_num; i++)
        {
            if (Cells[i].Index == index)
            {
                Cells[i].Value = val;
            }
        }
    }

    public int FindMinIndex()
    {
        int minVal = int.MaxValue;
        foreach (var _cell in Cells)
        {
            if (_cell.Index < minVal)
            {
                minVal = _cell.Index;
            }
        }
        return minVal;
    }

    public int FindMaxIndex()
    {
        int maxVal = int.MinValue;
        foreach (var _cell in Cells)
        {
            if (_cell.Index > maxVal)
            {
                maxVal = _cell.Index;
            }
        }
        return maxVal;
    }

    public void SelectCell(int index)
    {
        foreach (var e_cell in ExtendedCells)
        {
            e_cell.IsSelected = (e_cell.Index == index);
        }

        foreach (var cell in Cells)
        {
            cell.IsSelected = (cell.Index == index);
        }
    }
}
