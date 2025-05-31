using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using RGR_TIMP_4_sem.Models;

namespace RGR_TIMP_4_sem.Interfaces
{
    [JsonPolymorphic]
    [JsonDerivedType(typeof(LeftMove), typeDiscriminator:"LeftMove")]
    [JsonDerivedType(typeof(RightMove), typeDiscriminator: "RightMove")]
    [JsonDerivedType(typeof(One), typeDiscriminator: "One")]
    [JsonDerivedType(typeof(Zero), typeDiscriminator: "Zero")]
    [JsonDerivedType(typeof(Stop), typeDiscriminator: "Stop")]
    [JsonDerivedType(typeof(Question), typeDiscriminator: "Question")]
    public interface ICommand
    {
        public string NameCommand { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cells"> лист клеток </param>
        /// <param name="index"> индекс в глобальных координатах координатах</param>
        /// <returns></returns>
        public int Work(ObservableCollection<ICell> Cells);

    }
}
