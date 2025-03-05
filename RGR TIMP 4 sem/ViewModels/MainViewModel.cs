using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;
using Avalonia.Data.Converters;
using Avalonia.Media;
using ReactiveUI;

namespace RGR_TIMP_4_sem.ViewModels;

public class CellModel : ReactiveObject
{
    private int _value;
    private int _index;
    private bool _isSelected;
    public int Value
    {
        get => _value;
        set => this.RaiseAndSetIfChanged(ref _value, value);
    }

    public int Index
    {
        get => _index;
        set => this.RaiseAndSetIfChanged(ref _index, value);
    }

    public bool IsSelected
    {
        get => _isSelected;
        set => this.RaiseAndSetIfChanged(ref _isSelected, value);
    }
}

public class BoolToBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value ? Brushes.Red : Brushes.Transparent; // или любой другой цвет по умолчанию
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class MainViewModel : ViewModelBase
{
    // здесь будут храниться ячейки, которые показываются на экране
    public ObservableCollection<CellModel> Cells { get; }

    // здесь будут показываться ВСЕ ячейки
    public ObservableCollection<CellModel> ExtendedCells { get; }
    public int cell_num {  get; set; }
    public int all_cell_num {  get; set; }

    public MainViewModel()
    {
        cell_num = 19;
        all_cell_num = 200;

        Cells = new ObservableCollection<CellModel>();
        ExtendedCells = new ObservableCollection<CellModel>();

        CellInitialize(Cells, cell_num);
        CellInitialize(ExtendedCells, all_cell_num);

        SetCell(3, 1);
        SetCell(-1, 1);
        SetCell(6, 1);
        SetCell(2, 1);
        SetCell(-5, 1);
        SetCell(8, 1);

        SelectCell(0);
    }

    public void CellInitialize(ObservableCollection<CellModel> _Cells, int _cell_num)
    {
        for (int i = 0; i < _cell_num; i++)
        {
            _Cells.Add(new CellModel());
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


    public void SetCell(int index, int num)
    {
        foreach(var cell in Cells)
        {
            if (cell.Index == index)
            {
                cell.Value = num;
            }
        }
    }

    public void SelectCell(int index)
    {
        foreach (var cell in Cells)
        {
            cell.IsSelected = (cell.Index == index);
        }
    }
}
