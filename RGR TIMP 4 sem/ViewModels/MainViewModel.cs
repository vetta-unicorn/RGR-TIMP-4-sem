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
using System.Threading.Tasks;
using System.Threading;


namespace RGR_TIMP_4_sem.ViewModels;

public class MainViewModel : ReactiveObject
{
    // вспомогательные классы
    CellViewModel cellViewModel;
    CommandFunc commandFunc;

    // команды для кнопок
    public ReactiveCommand<Unit, Unit> ButtonClickCommandLeft { get; }
    public ReactiveCommand<Unit, Unit> ButtonClickCommandRight { get; }
    public ReactiveCommand<Unit, Unit> AddRowCommand { get; } //команда добавления строки кода 
    public ReactiveCommand<Unit, Unit> DeleteRowCommand { get; } //команда удаления строки кода
    public ReactiveCommand<Unit, Unit> Start { get; } // запустить программу
    public ReactiveCommand<Unit, Unit> LineByLine { get; } // идти по строкам

    // ячейки
    private ObservableCollection<ICell> _cells;
    public ObservableCollection<ICell> Cells
    {
        get => _cells;
        set => this.RaiseAndSetIfChanged(ref _cells, value);
    }

    public ObservableCollection<ICell> VisibleCells { get; private set; }

    // количество ячеек
    public int allCellNum;


    // строки программы
    private ObservableCollection<ICommandLine> commandLines;

    public ObservableCollection<ICommandLine> CommandLines
    {
        get => commandLines;
        set => this.RaiseAndSetIfChanged(ref commandLines, value);
    }

    public List<ICommand> AvailableCommands => CommandList.Instance.Commands;


    // для вывода ошибок
    private string _ConsoleBox;
    public string ConsoleBox
    {
        get => _ConsoleBox;
        set => this.RaiseAndSetIfChanged(ref _ConsoleBox, value);
    }

    // последняя выполняемая строка
    private int _LastMadeLine;
    public int LastMadeLine
    {
        get => _LastMadeLine;
        set => this.RaiseAndSetIfChanged(ref _LastMadeLine, value);
    }

    public MainViewModel()
    {
        // команды для кнопок
        ButtonClickCommandLeft = ReactiveCommand.Create(OnButtonClickLeft);
        ButtonClickCommandRight = ReactiveCommand.Create(OnButtonClickRight);
        Start = ReactiveCommand.Create(StartProgram);

        allCellNum = 201;
        int leftDefaultBorder = 91;
        int rightDefaultBorder = 109;

        Cells = new ObservableCollection<ICell>();
        VisibleCells = new ObservableCollection<ICell>();
        CellViewModel.InitializeCells(Cells, allCellNum);

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

        // отображаем только 19 клеток от -9 до 9
        for (int i = leftDefaultBorder; i < rightDefaultBorder + 1; i++)
        {
            Cells[i].IsVisible = true;
        }

        // вспомогательные классы
        cellViewModel = new CellViewModel(Cells);
        // ставим каретку по умолчанию на 0 ячейку
        Cells[cellViewModel.FindCellByIndex(0)].IsSelected = true;

        // линии программы
        CommandLines = new ObservableCollection<ICommandLine>();
        AddRowCommand = ReactiveCommand.Create(AddNewRow);
        DeleteRowCommand = ReactiveCommand.Create(DeleteRow);
        LineByLine = ReactiveCommand.Create(StartLineByLine);
        AddNewRow();

        // изначально выбираем первую строку программы
        CommandLines[0].IsSelected = true;
        commandFunc = new CommandFunc(CommandLines);
    }

    // подписываемся на событие по изменению видимости
    private void SubscribeToVisibility(ICell cell)
    {
        cell.WhenAnyValue(x => x.IsVisible)
            .Subscribe(isVisible =>
            {
                if (VisibleCells != null && isVisible && !VisibleCells.Contains(cell))
                {
                    if (VisibleCells.Count() != 0 && VisibleCells.First().Index > cell.Index) VisibleCells.Insert(0, cell);
                    else VisibleCells.Add(cell);
                }
                else if (VisibleCells != null && !isVisible && VisibleCells.Contains(cell))
                {
                    VisibleCells.Remove(cell);
                }
            });
    }
    public async void StartProgram()
    {
        await Task.Run(() => {
            BinAlgoritm binAlgoritm = new BinAlgoritm();
            var cts = new CancellationTokenSource();
            var result = binAlgoritm.Working(-1, Cells, CommandLines, cts.Token);

            Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => {
                if (result == "The commands were successfully completed.")
                {
                    ConsoleBox = "The program was successfully completed!";
                }
                else
                {
                    ConsoleBox = result; 
                    if (cellViewModel.FindSelectedCell() != -1) 
                        Cells[cellViewModel.FindSelectedCell()].IsSelected = false;
                    Cells[cellViewModel.FindCellByIndex(0)].IsSelected = true;
                }

                if (commandFunc.FindSelectedLine() != -1)
                {
                    CommandLines[commandFunc.FindSelectedLine()].IsSelected = false;
                    CommandLines[0].IsSelected = true;
                }
                else
                {
                    ConsoleBox = "No command line selected!";
                    CommandLines[0].IsSelected = true;
                }
            });
        });
    }

    public async void StartLineByLine()
    {
        await Task.Run(() => {
            BinAlgoritm binAlgoritm = new BinAlgoritm();
            var cts = new CancellationTokenSource();
            var result = binAlgoritm.OneCommandWorking(Cells, CommandLines, cts.Token);

            Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => {
                if (result == "The command was successfully completed!")
                {
                    ConsoleBox = "The command was successfully completed!";
                }
                else
                {
                    ConsoleBox = result; 
                    if (cellViewModel.FindSelectedCell() != -1)
                        Cells[cellViewModel.FindSelectedCell()].IsSelected = false;
                    Cells[cellViewModel.FindCellByIndex(0)].IsSelected = true;
                }
            });
        });
    }


    public void AddNewRow()
    {
        int rowNumber = CommandLines.Count; 
        CommandLines.Add(new CommandLine { Number = rowNumber });
    }

    public void DeleteRow()
    {
        if (CommandLines.Count > 0)
        {
            int rowNumber = CommandLines.Count; 
            CommandLines.RemoveAt(rowNumber - 1);
        }
    }

    private void OnButtonClickLeft()
    {
        Cells[cellViewModel.FindLeftVisible() - 1].IsVisible = true;
        Cells[cellViewModel.FindRightVisible()].IsVisible = false;
    }

    private void OnButtonClickRight()
    {
        Cells[cellViewModel.FindLeftVisible()].IsVisible = false;
        Cells[cellViewModel.FindRightVisible() + 1].IsVisible = true;
    }
}
