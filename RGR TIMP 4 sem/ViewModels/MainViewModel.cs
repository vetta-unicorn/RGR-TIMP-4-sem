using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

    private ObservableCollection<ICommandLine> commandLines;

    public ObservableCollection<ICommandLine> CommandLines
    {
        get => commandLines;
        set => this.RaiseAndSetIfChanged(ref commandLines, value);
    }

    public ReactiveCommand<Unit, Unit> AddRowCommand { get; } //команда добавления строки кода 
    public ReactiveCommand<Unit, Unit> DeleteRowCommand { get; } //команда удаления строки кода
    public ReactiveCommand<Unit, Unit> Start { get; } // запустить программу

    public List<ICommand> AvailableCommands => CommandList.Instance.Commands;

    // для вывода ошибок
    private string _ErrorBox;
    public string ErrorBox
    {
        get => _ErrorBox;
        set => this.RaiseAndSetIfChanged(ref _ErrorBox, value);
    }

    public MainViewModel()
    {
        // команды для кнопок
        ButtonClickCommandLeft = ReactiveCommand.Create(OnButtonClickLeft);
        ButtonClickCommandRight = ReactiveCommand.Create(OnButtonClickRight);
        Start = ReactiveCommand.Create(StartProgram);

        cell_num = 19;
        all_cell_num = 201;

        Cells = new ObservableCollection<ICell>();
        ExtendedCells = new ObservableCollection<ICell>();

        CommandLines = new ObservableCollection<ICommandLine>();
        AddRowCommand = ReactiveCommand.Create(AddNewRow);
        DeleteRowCommand = ReactiveCommand.Create(DeleteRow);
        AddNewRow();

        InitializeIndices(Cells, cell_num);
        InitializeIndices(ExtendedCells, all_cell_num);
        CommandLines[0].IsSelected = true;

        current_index = 0;
        SelectCell(current_index);
    }

    public void StartProgram()
    {
        BinAlgoritm binAlgoritm = new BinAlgoritm();
        bool Flag = false;
        try
        {
            binAlgoritm.Working(-1, ExtendedCells, CommandLines);
        }
        catch (Exception ex)
        {
            ErrorBox = ex.Message;
            Flag = true;
        }
        finally
        {
            if (!Flag)
            {
                ErrorBox = "";
            }

            UpdateVisibleItemsValues(Cells, ExtendedCells);
        }
    }


    public void AddNewRow()
    {
        int rowNumber = CommandLines.Count; // Нумерация строк
        CommandLines.Add(new CommandLine { Number = rowNumber });
    }

    public void DeleteRow()
    {
        int rowNumber = CommandLines.Count; // Нумерация строк
        CommandLines.RemoveAt(rowNumber - 1);
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

    public static void InitializeIndices(ObservableCollection<ICell> cells, int number)
    {
        // Определяем смещение для индексов
        int offset = number / 2;

        for (int i = 0; i < number; i++)
        {
            cells.Add(new CellModel());
            cells[i].Index = i - offset; // Устанавливаем индекс
        }
    }

    public void UpdateVisibleItemsValues(ObservableCollection<ICell> visibleItems, ObservableCollection<ICell> extendedItems)
    {
        // Создаем словарь для быстрого доступа к элементам extendedItems по их индексам
        Dictionary<int, ICell> extendedItemsDict = extendedItems.ToDictionary(item => item.Index);

        // Проходим по элементам visibleItems
        foreach (var visibleItem in visibleItems)
        {
            // Проверяем, есть ли соответствующий элемент в extendedItems
            if (extendedItemsDict.TryGetValue(visibleItem.Index, out var extendedItem))
            {
                // Обновляем значение Value у visibleItem
                visibleItem.Value = extendedItem.Value;
                visibleItem.IsSelected = extendedItem.IsSelected;
            }
        }
    }


    //public void SetCell(int index, int val)
    //{
    //    int maxIndex = FindMaxIndex();
    //    int minIndex = FindMinIndex();
    //    for (int i = 0; i < all_cell_num; i++)
    //    {
    //        if (ExtendedCells[i].Index == index)
    //        {
    //            ExtendedCells[i].Value = val;
    //        }
    //    }

    //    for (int i = 0; i < cell_num; i++)
    //    {
    //        if (Cells[i].Index == index)
    //        {
    //            Cells[i].Value = val;
    //        }
    //    }
    //}

    //public int FindMinIndex()
    //{
    //    int minVal = int.MaxValue;
    //    foreach (var _cell in Cells)
    //    {
    //        if (_cell.Index < minVal)
    //        {
    //            minVal = _cell.Index;
    //        }
    //    }
    //    return minVal;
    //}

    //public int FindMaxIndex()
    //{
    //    int maxVal = int.MinValue;
    //    foreach (var _cell in Cells)
    //    {
    //        if (_cell.Index > maxVal)
    //        {
    //            maxVal = _cell.Index;
    //        }
    //    }
    //    return maxVal;
    //}

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
