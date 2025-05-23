using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using RGR_TIMP_4_sem.Models;

namespace RGR_TIMP_4_sem.Interfaces
{
    [JsonDerivedType(typeof(CellModel), typeDiscriminator: "CellModel")]
    public interface ICell
    {
        public bool IsSelected { get; set; }
        public int Index { get; set; }
        public int Value { get; set; }
    }
}
