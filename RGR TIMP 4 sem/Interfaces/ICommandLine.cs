using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using RGR_TIMP_4_sem.Models;

namespace RGR_TIMP_4_sem.Interfaces
{
    [JsonDerivedType(typeof(CommandLine), typeDiscriminator: "CommandLine")]
    public interface ICommandLine : IJsonDataItem
    {
        public bool IsSelected { get; set; }
        public int Number { get; set; }
        public ICommand Command { get; set; }
        public string? Str { get; set; }
        public string? Comments { get; set; }

    }
}
