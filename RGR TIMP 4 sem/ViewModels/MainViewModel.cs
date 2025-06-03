using System;
using System.IO;
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
using RGR_TIMP_4_sem.DanyaWork;
using Avalonia.Controls;
using Avalonia.Threading;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Controls.Primitives;
using System.Diagnostics;
using Avalonia.Controls.Shapes;


namespace RGR_TIMP_4_sem.ViewModels;

public class MainViewModel : ReactiveObject
{
    // вспомогательные классы
    CellViewModel cellViewModel;
    CommandFunc commandFunc;
    Load load;
    Save save;

    // команды для кнопок
    public ReactiveCommand<Unit, Unit> ButtonClickCommandLeft { get; }
    public ReactiveCommand<Unit, Unit> ButtonClickCommandRight { get; }
    
    public ReactiveCommand<Unit, Unit> ButtonClickLineUp { get; }
    public ReactiveCommand<Unit, Unit> ButtonClickLineDown { get; }

    public ReactiveCommand<Unit, Unit> AddRowCommand { get; } //команда добавления строки кода 
    public ReactiveCommand<Unit, Unit> DeleteRowCommand { get; } //команда удаления строки кода
    public ReactiveCommand<Unit, Unit> Start { get; } // запустить программу
    public ReactiveCommand<Unit, Unit> LineByLine { get; } // идти по строкам

    public ReactiveCommand<Unit, Unit> OpenFile { get; } // менюшка с выбором что открыть
    public ReactiveCommand<Unit, Unit> OpenExistingFile { get; } // открыть существующий файл
    public ReactiveCommand<Unit, Unit> SaveFile { get; } // сохранить файл
    public ReactiveCommand<Unit, Unit> DeleteFile { get; } // удалить файл
    public ReactiveCommand<Unit, Unit> CreateNewFile { get; } // создать новый файл
    public ReactiveCommand<Unit, Unit> OnSaveTo { get; } // открыть окно для ввода

    // ячейки
    private ObservableCollection<ICell> _cells;
    public ObservableCollection<ICell> Cells
    {
        get => _cells;
        set => this.RaiseAndSetIfChanged(ref _cells, value);
    }

    private ObservableCollection<ICell> _VisibleCells;
    public ObservableCollection<ICell> VisibleCells
    {
        get => _VisibleCells;
        set => this.RaiseAndSetIfChanged(ref _VisibleCells, value);
    }

    // количество ячеек
    private int allCellNum;
    private int leftDefaultBorder;
    private int rightDefaultBorder;


    // строки программы
    private ObservableCollection<ICommandLine> commandLines;

    public ObservableCollection<ICommandLine> CommandLines
    {
        get => commandLines;
        set => this.RaiseAndSetIfChanged(ref commandLines, value);
    }

    // видимыес троки программы
    private ObservableCollection<ICommandLine> _visibleCommandLines;
    public ObservableCollection<ICommandLine> VisibleCommandLines
    {
        get => _visibleCommandLines;
        set => this.RaiseAndSetIfChanged(ref _visibleCommandLines, value);
    }

    private ObservableCollection<string> _commandNames;
    public ObservableCollection<string> CommandNames
    {
        get => _commandNames;
        set => this.RaiseAndSetIfChanged(ref _commandNames, value);
    }

    public List<ICommand> AvailableCommands { get; set; }
    

    // для вывода ошибок
    private string _ConsoleBox;
    public string ConsoleBox
    {
        get => _ConsoleBox;
        set => this.RaiseAndSetIfChanged(ref _ConsoleBox, value);
    }

    private static string controlDir = AppContext.BaseDirectory;
    private static string dataSavePath = System.IO.Path.Combine(controlDir, "DataSave\\");

    private FileItem _selectedFile;
    public FileItem SelectedFile
    {
        get => _selectedFile;
        set => this.RaiseAndSetIfChanged(ref _selectedFile, value);
    }
    private FileList _MyFiles;
    public FileList MyFiles
    {
        get => _MyFiles;
        set => this.RaiseAndSetIfChanged(ref _MyFiles, value);
    }

    private bool _isOpenMenuSeen = false;
    public bool IsOpenMenuSeen
    {
        get => _isOpenMenuSeen;
        set => this.RaiseAndSetIfChanged(ref _isOpenMenuSeen, value);
    }

    private bool _isSaveMenuSeen = false;
    public bool IsSaveMenuSeen
    {
        get => _isSaveMenuSeen;
        set => this.RaiseAndSetIfChanged(ref _isSaveMenuSeen, value);
    }

    private string _saveFileName;
    public string SaveFileName
    {
        get => _saveFileName;
        set => this.RaiseAndSetIfChanged(ref _saveFileName, value);
    }

    public MainViewModel()
    {
        // команды для кнопок
        ButtonClickCommandLeft = ReactiveCommand.Create(OnButtonClickLeft);
        ButtonClickCommandRight = ReactiveCommand.Create(OnButtonClickRight);

        // команды для листания строк кода
        ButtonClickLineUp = ReactiveCommand.Create(OnButtonClickLineUp);
        ButtonClickLineDown = ReactiveCommand.Create(OnButtonClickLineDown);

        Start = ReactiveCommand.Create(StartProgram);
        AddRowCommand = ReactiveCommand.Create(AddNewRow);
        DeleteRowCommand = ReactiveCommand.Create(DeleteRow);
        LineByLine = ReactiveCommand.Create(StartLineByLine);

        // команды для работы с файлами
        CreateNewFile = ReactiveCommand.Create(CreateFile);
        SaveFile = ReactiveCommand.Create(SaveToFile);
        OnSaveTo = ReactiveCommand.Create(OnSaveToFile);
        OpenFile = ReactiveCommand.Create(Open);
        OpenExistingFile = ReactiveCommand.Create(OpenExisting);

        allCellNum = 201;
        leftDefaultBorder = 91;
        rightDefaultBorder = 109;

        AvailableCommands = new List<ICommand>
        {
            new LeftMove(),
            new RightMove(),
            new One(),
            new Zero(),
            new Stop(),
            new Question()
        };

        CreateFile();

        save = new Save();
        load = new Load();

        MyFiles = new FileList(dataSavePath);
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

    // подписываемся на изменение по видимости для строк программы
    private void SubscribeToVisibilityLines(ICommandLine line)
    {
        line.WhenAnyValue(x => x.IsVisible)
            .Subscribe(isVisible =>
            {
                if (VisibleCommandLines != null && isVisible && !VisibleCommandLines.Contains(line))
                {
                    if (VisibleCommandLines.Count() != 0 && VisibleCommandLines.First().Number > line.Number) VisibleCommandLines.Insert(0, line);
                    else VisibleCommandLines.Add(line);
                }
                else if (VisibleCommandLines != null && !isVisible && VisibleCommandLines.Contains(line))
                {
                    VisibleCommandLines.Remove(line);
                }
            });
    }

    public void Open()
    {
        IsOpenMenuSeen = !IsOpenMenuSeen;
    }

    public void OpenExisting()
    {
        try
        {
            if (SelectedFile == null || SelectedFile.FileName == "" || SelectedFile.FileName == null) 
                throw new Exception("No file selected!");

            (List<ICommandLine>, List<ICell>, bool) tuple = load.LoadData(dataSavePath, SelectedFile.FileName);
            if (!tuple.Item3)
            {
                ConsoleBox = "The file doesn't exist in this directory!";
            }
            SetNewWindow(tuple.Item1, tuple.Item2);
        }
        catch (Exception ex)
        {
            ConsoleBox = ex.Message;
        }
    }

    public void SetNewWindow(List<ICommandLine> newLines, List<ICell> nCells)
    {
        ObservableCollection<ICommandLine> newCommandLines = new ObservableCollection<ICommandLine>();
        ObservableCollection<ICell> newCells = new ObservableCollection<ICell>();

        for (int i = 0; i < nCells.Count(); i++)
        {
            newCells.Add(nCells[i]);
        }

        Cells = newCells;
        VisibleCells = new ObservableCollection<ICell>();
        CellViewModel.InitializeCells(Cells, allCellNum);

        foreach (var cell in Cells)
        {
            SubscribeToVisibility(cell);
        }

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

        for (int i = leftDefaultBorder; i < rightDefaultBorder + 1; i++)
        {
            Cells[i].IsVisible = true;
        }

        cellViewModel = new CellViewModel(Cells);

        var cmds = AvailableCommands;
        foreach (var line in newLines)
        {
            if (line.Command != null)
            {
                var name = line.Command.NameCommand;
                var match = cmds.FirstOrDefault(cmd => cmd.NameCommand == name);
                if (match != null )line.Command = match;
            }
        }

        CommandLines = new ObservableCollection<ICommandLine>(newLines);
        VisibleCommandLines = new ObservableCollection<ICommandLine>();

        // изначально выбираем первую строку программы
        commandFunc = new CommandFunc();

        foreach(var line in CommandLines)
        {
            SubscribeToVisibilityLines(line);
        }

        int lineCount = CommandLines.Count();
        if (lineCount > 7)
        {
            for (int i = 0; i < 7; i++)
            {
                CommandLines[i].IsVisible = true;
            }
        }
        else
        {
            foreach (var line in CommandLines)
            {
                line.IsVisible = true;
            }
        }

        // Подписываемся на добавление новых команд
        CommandLines.CollectionChanged += (s, e) =>
        {
            if (e.NewItems != null)
            {
                foreach (ICommandLine newLine in e.NewItems)
                {
                    SubscribeToVisibilityLines(newLine);
                }
            }
        };

        CommandNames = new ObservableCollection<string>();

        foreach (var comm in CommandLines)
        {
            CommandNames.Add(CommandToStringConverter.CommandToString(comm.Command));
        }

        ConsoleBox = "The file has been successfully opened!";

        foreach (var l in newLines)
            Debug.WriteLine(l.Command.GetType().Name + " @" + l.Command.GetHashCode());
        foreach (var c in cmds)
            Debug.WriteLine("Available: " + c.GetType().Name + " @" + c.GetHashCode());
    }

    public void SaveToFile()
    {
        IsSaveMenuSeen = !IsSaveMenuSeen;
    }

    private void OnSaveToFile()
    {
        bool Marker = true;
        List<IJsonDataItem> allItems = new List<IJsonDataItem>();
        foreach (var cell in Cells)
        {
            allItems.Add(cell);
        }
        foreach (var line in CommandLines)
        {
            allItems.Add(line);
        }

        try
        {
            bool Flag = save.SaveData(dataSavePath, SaveFileName, allItems);
            if (!Flag) ConsoleBox = "Unexpected error!";
        }
        catch (Exception ex)
        {
            Marker = false;
            ConsoleBox = ex.Message;
        }
        finally
        {
            if (Marker) ConsoleBox = "The file has been successfully saved!";
        }

    }

    public void CreateFile()
    {
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
        VisibleCommandLines = new ObservableCollection<ICommandLine>();

        // изначально выбираем первую строку программы
        commandFunc = new CommandFunc();

        AddNewRow();
        CommandLines[0].IsVisible = true;
        CommandLines[0].IsSelected = true;

        // Подписываемся на добавление новых команд
        CommandLines.CollectionChanged += (s, e) =>
        {
            if (e.NewItems != null)
            {
                foreach (ICommandLine newLine in e.NewItems)
                {
                    SubscribeToVisibilityLines(newLine);
                }
            }
        };

        ConsoleBox = "";
    }

    public async void StartProgram()
    {
        await Task.Run(() => {
            BinAlgoritm binAlgoritm = new BinAlgoritm();
            var cts = new CancellationTokenSource();
            var result = binAlgoritm.Working(-1, Cells, CommandLines, cts.Token);

            Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => {
                if (result == "The commands were successfully completed!")
                {
                    ConsoleBox = result;
                }
                else
                {
                    ConsoleBox = result;
                    if (cellViewModel.FindSelectedCell() != -1)
                        Cells[cellViewModel.FindSelectedCell()].IsSelected = false;
                    Cells[cellViewModel.FindCellByIndex(0)].IsSelected = true;
                }

                if (commandFunc.FindSelectedLine(CommandLines) != -1)
                {
                    CommandLines[commandFunc.FindSelectedLine(CommandLines)].IsSelected = false;
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

        SubscribeToVisibilityLines(CommandLines.Last());

        CommandLines.Last().IsVisible = true;
        if (commandFunc.CountVisibleRows(CommandLines) > 7)
            CommandLines[commandFunc.FindFirstVisible(CommandLines)].IsVisible = false;
    }

    public void DeleteRow()
    {
        if (CommandLines.Count > 1)
        {
            int rowNumber = CommandLines.Count;
            CommandLines[rowNumber - 1].IsVisible = false;
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

    private void OnButtonClickLineUp()
    {
        try
        {
            if (CommandLines[commandFunc.FindFirstVisible(CommandLines) - 1] != null
                && CommandLines[commandFunc.FindLastVisible(CommandLines)] != null)
            {
                CommandLines[commandFunc.FindFirstVisible(CommandLines) - 1].IsVisible = true;
                CommandLines[commandFunc.FindLastVisible(CommandLines)].IsVisible = false;
            }

            ConsoleBox = "";
        }
        catch(Exception ex) 
        {
            ConsoleBox = ex.Message;
        }

    }

    private void OnButtonClickLineDown()
    {
        try
        {
            if (commandFunc.FindLastVisible(CommandLines) == CommandLines.Count() - 1)
            {
                throw new Exception("Can't go lower than the last row!");
            }

            if (CommandLines[commandFunc.FindFirstVisible(CommandLines)] != null
                && CommandLines[commandFunc.FindLastVisible(CommandLines) + 1] != null)
            {
                CommandLines[commandFunc.FindFirstVisible(CommandLines)].IsVisible = false;
                CommandLines[commandFunc.FindLastVisible(CommandLines) + 1].IsVisible = true;
            }

            ConsoleBox = "";
        }
        catch (Exception ex)
        {
            ConsoleBox = ex.Message;
        }
    }
}
