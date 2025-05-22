using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using ReactiveUI;
using ReactiveUI.Fody;
using RGR_TIMP_4_sem.Interfaces;
using RGR_TIMP_4_sem.Models;
using System.ComponentModel;


namespace RGR_TIMP_4_sem.ViewModels;

public class MainViewModel : ReactiveObject
{
    public ReactiveCommand<Unit, Unit> ButtonClickCommandLeft { get; }
    public ReactiveCommand<Unit, Unit> ButtonClickCommandRight { get; }

    private ObservableCollection<ICell> _cells;
    public ObservableCollection<ICell> Cells
    {
        get => _cells;
        set => this.RaiseAndSetIfChanged(ref _cells, value);
    }

    public ObservableCollection<ICell> VisibleCells { get; private set; }

    public int visibleCellNum { get; set; }
    public int allCellNum { get; set; }

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

        visibleCellNum = 19;
        allCellNum = 201;

        Cells = new ObservableCollection<ICell>();
        VisibleCells = new ObservableCollection<ICell>();

        foreach (var cell in Cells)
        {
            SubscribeToVisibility(cell);
        }

        // Подписываемся на добавление новых ячеек
        Cells.CollectionChanged += (s, e) =>
        {
            if (e.NewItems != null)
            {
                foreach (ICell newCell in e.NewItems)
                {
                    SubscribeToVisibility(newCell);
                }
            }
        };

        Cells[100].IsVisible = true;

        CommandLines = new ObservableCollection<ICommandLine>();
        AddRowCommand = ReactiveCommand.Create(AddNewRow);
        DeleteRowCommand = ReactiveCommand.Create(DeleteRow);
        AddNewRow();

        InitializeIndices(Cells, allCellNum);
        CommandLines[0].IsSelected = true;

        current_index = 0;
        SelectCell(current_index);
    }

    // подписываемся на событие по изменению видимости
    private void SubscribeToVisibility(ICell cell)
    {
        cell.WhenAnyValue(x => x.IsVisible)
            .Subscribe(isVisible =>
            {
                if (isVisible && !VisibleCells.Contains(cell))
                {
                    VisibleCells.Add(cell);
                }
                else if (!isVisible && VisibleCells.Contains(cell))
                {
                    VisibleCells.Remove(cell);
                }
            });
    }

    public void StartProgram()
    {
        BinAlgoritm binAlgoritm = new BinAlgoritm();
        bool Flag = false;
        try
        {
            binAlgoritm.Working(-1, Cells, CommandLines);
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
        //for (int i = 0; i < cell_num; i++)
        //{
        //    Cells[i].Index--;

        //    foreach (var _cell in ExtendedCells)
        //    {
        //        if (_cell.Index == Cells[i].Index)
        //        {
        //            Cells[i].Value = _cell.Value;
        //            break;
        //        }
        //    }
        //}
        SelectCell(current_index);
    }

    private void OnButtonClickRight()
    {
        //for (int i = 0; i < cell_num; i++)
        //{
        //    Cells[i].Index++;

        //    foreach (var _cell in ExtendedCells)
        //    {
        //        if (_cell.Index == Cells[i].Index)
        //        {
        //            Cells[i].Value = _cell.Value;
        //            break;
        //        }
        //    }
        //}
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

    public void SelectCell(int index)
    {
        foreach (var cell in Cells)
        {
            cell.IsSelected = (cell.Index == index);
        }
    }
}
