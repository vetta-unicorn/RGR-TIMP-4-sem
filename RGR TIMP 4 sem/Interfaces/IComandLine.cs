using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGR_TIMP_4_sem.Interfaces
{
    public interface IComandLine
    {

        public bool IsSelected { get; set; }
        public int Number { get; set; }
        public ICommand Command { get; set; }
        public string? Str { get; set; }
        public string? Comments { get; set; }

    }
}
