using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGR_TIMP_4_sem.Interfaces
{
    public interface ICell
    {
        public bool IsSelected { get; set; }
        public int Index { get; set; }
        public int Value { get; set; }
        public bool IsVisible { get; set; }
    }
}
