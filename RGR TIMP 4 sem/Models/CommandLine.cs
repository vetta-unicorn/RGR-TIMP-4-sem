using ReactiveUI;
using RGR_TIMP_4_sem.Interfaces;

namespace RGR_TIMP_4_sem.Models
{
    public class CommandLine : ReactiveObject, ICommandLine
    {
        private bool _isSelected;
        private int number;
        private ICommand command;
        private string? str; 
        private string? comments;

        public bool IsSelected
        {
            get => _isSelected;
            set => this.RaiseAndSetIfChanged(ref _isSelected, value);
        }

        public int Number
        {
            get => number;
            set => this.RaiseAndSetIfChanged(ref number, value);
        }
        public ICommand Command
        {
            get => command;
            set => this.RaiseAndSetIfChanged(ref command, value);
        }
        public string? Str
        {
            get => str;
            set => this.RaiseAndSetIfChanged(ref str, value);
        }
        public string? Comments
        {
            get => comments;
            set => this.RaiseAndSetIfChanged(ref comments, value);
        }
        public CommandLine(bool selected, int number, ICommand command, string? str, string? comments)
        {
            this._isSelected = selected;
            this.number = number;
            this.command = command;
            this.str = str;
            this.comments = comments;
        }

        public CommandLine(bool selected, int number, ICommand command)
        {
            this._isSelected = selected;
            this.number = number;
            this.command = command;
        }
        public CommandLine() { }
    }
}
