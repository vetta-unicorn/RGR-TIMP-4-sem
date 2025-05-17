using RGR_TIMP_4_sem.Interfaces;

namespace RGR_TIMP_4_sem.Models
{
    public class ComandLine : IComandLine
    {
        private bool _isSelected;
        private int number;
        private ICommand command;
        private string? str; 
        private string? comments;

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; }
        }

        public int Number
        {
            get { return number; }
            set { number = value; }
        }
        public ICommand Command
        {
            get { return command; }
            set { command = value; }
        }
        public string? Str
        {
            get { return str; }
            set { str = value; }
        }
        public string? Comments
        {
            get { return comments; }
            set { comments = value; }
        }
        public ComandLine(bool selected, int number, ICommand command, string? str, string? comments)
        {
            this._isSelected = selected;
            this.number = number;
            this.command = command;
            this.str = str;
            this.comments = comments;
        }

        public ComandLine(bool selected, int number, ICommand command)
        {
            this._isSelected = selected;
            this.number = number;
            this.command = command;
        }
        public ComandLine() { }
    }
}
