using RGR_TIMP_4_sem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RGR_TIMP_4_sem.Interfaces
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
    [JsonDerivedType(typeof(CellModel), "CellModel")]
    [JsonDerivedType(typeof(CommandLine), "CommandLine")]
    public interface IJsonDataItem { } // Родительский интерфейс
}
