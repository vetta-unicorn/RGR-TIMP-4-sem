using DynamicData.Binding;
using RGR_TIMP_4_sem.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RGR_TIMP_4_sem.Models
{
    public class BinAlgoritm : IAlgoritm
    {

        /// <summary>
        /// Бинарный алгоритм
        /// </summary>
        /// <param name="indexMove"> до какой строки делать за 1 нажатие, если -1 то делать весь алгоритм</param>
        /// <param name="Cells"> ObservableCollection клеток (всех) </param>
        /// <param name="CommandLine"> ObservableCollection строк алгоритма</param>
        /// <returns>возвращает строку если выполнено, 
        /// 0 если кончились строки, 
        /// -1 если бесконечный цикл</returns>
        public string Working (int indexMove, ObservableCollection<ICell> Cells, ObservableCollection<ICommandLine> CommandLine, CancellationToken token)
        {
            if(Cells==null)
            {
                return "The cell list is empty";
            }
            else if (CommandLine == null)
            {
                return "The command list is empty";
            }
            else {
                
                int flag = 1;
                int count_cycle = 0;
                while (flag!=-1)
                {
                    token.ThrowIfCancellationRequested();
                    int now = Number_of_SelectedStr(CommandLine);
                    if (now==indexMove)
                    {
                        return "The commands were successfully completed.";
                    }
                    if(count_cycle==1000)
                    {
                        CommandLine[now].IsSelected = false;
                        CommandLine[0].IsSelected = true;
                        return "The endless loop";
                    }

                    int stop_flag = 0;
                    foreach (var command in CommandLine)
                    {
                        if (command.Command is Stop) stop_flag++;
                    }
                    if (stop_flag < 1) return "No Stop has been detected. Please add Stop command!";
                    if (stop_flag > 1) return "More than 1 Stop has been detected. Please keep 1 Stop command!";

                    try
                    {
                        if (CommandLine[now].Command == null)
                        {
                            throw new Exception("The command is null");
                        }
                        if (CommandLine[now].Command is Question)
                        {
                            flag = CommandLine[now].Command.Work(Cells);
                            int[] mass = Split(CommandLine[now].Str);
                            if (mass.Length > 2 || mass.Length < 2) throw new Exception("Incorrect argument");
                            switchNumberLine(CommandLine, mass[flag]);
                            count_cycle++;
                        }
                        else
                        {
                            flag = CommandLine[now].Command.Work(Cells);
                            if (CommandLine[now].Command is Stop)
                            {
                                CommandLine[now].IsSelected = false;
                                CommandLine[0].IsSelected = true;
                                flag = -1;
                            }
                            else
                            {
                                switchNumberLine(CommandLine, Convert.ToInt32(CommandLine[now].Str));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                } 
            }
            return "The commands were successfully completed!";
        }

        public string OneCommandWorking(ObservableCollection<ICell> Cells, ObservableCollection<ICommandLine> CommandLine, CancellationToken token)
        {
            if (Cells == null)
            {
                return "The cell list is empty";
            }
            else if (CommandLine == null)
            {
                return "The command list is empty";
            }
            else
            {
                int flag = 1;
                token.ThrowIfCancellationRequested();
                int now = Number_of_SelectedStr(CommandLine);
                try
                {
                    if (CommandLine[now].Command == null)
                    {
                        throw new Exception("The command is null");
                    }
                    if (CommandLine[now].Command is Question)
                    {
                        flag = CommandLine[now].Command.Work(Cells);
                        int[] mass = Split(CommandLine[now].Str);
                        switchNumberLine(CommandLine, mass[flag]);
                    }
                    else
                    {
                        flag = CommandLine[now].Command.Work(Cells);
                        if (CommandLine[now].Command is Stop)
                        {
                            CommandLine[now].IsSelected = false;
                            CommandLine[0].IsSelected = true;
                            return "The command was successfully completed!";
                        }
                        else
                        {
                            switchNumberLine(CommandLine, Convert.ToInt32(CommandLine[now].Str));
                        }
                    }
                }

                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            return "The command was successfully completed!";
        }

        public int[] Split(string str)
        {
            if (str == null || str == "" || !str.Contains(","))
            {
                throw new Exception("Incorrect input!");
            }
            string[] parts = str.Split(',');
            int[] mass = new int [2];
            for(int i =0; i<2; i++)
            {
                mass[i] = Convert.ToInt32(parts[i]);
            }
            return mass;
        }


        
        public int Number_of_SelectedStr(ObservableCollection<ICommandLine> CommandLine)
        {
            if (CommandLine == null)
            {
                throw new NullReferenceException("The command list is empty");
            }
            else
            {
                int index = 0;
                foreach (var t in CommandLine)
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
        public void switchNumberLine(ObservableCollection<ICommandLine> CommandLine, int index)
        {
            if (index > CommandLine.Count() - 1 || index < 0)
            {
                throw new Exception("The line you want to switch to doesn't exist");
            }
            if (CommandLine == null)
            {
                throw new NullReferenceException("The Command list is empty");
            }
            else
            {
                for (int i = 0; i < CommandLine.Count(); i++)
                {
                    if (CommandLine[i].Number != index)
                    {
                        CommandLine[i].IsSelected = false;
                    }
                    else if (CommandLine[i].Number == index)
                    {
                        CommandLine[i].IsSelected = true;
                    }
                }
            }
        }
    }
}
