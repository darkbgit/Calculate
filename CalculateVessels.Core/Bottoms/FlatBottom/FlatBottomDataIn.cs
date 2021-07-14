using CalculateVessels.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateVessels.Core.Bottoms.FlatBottom
{
    public class FlatBottomDataIn : IDataIn
    {
        public bool IsDataGood => !ErrorList.Any();

        public List<string> ErrorList { get; private set; } = new();

        public double c1 { get; set; }
        public double c2 { get; set; }
        public double c3 { get; set; }
        public double D { get; set; }
        public double D3 { get; set; }
        public double Dcp { get; set; }
        public double s { get; set; }
        public double s1 { get; set; }
        public double s2 { get; set; }
        public double a { get; set; }
        public double h1 { get; set; }
        public double r { get; set; }

        public double gamma { get; set; }

        public int Type
        {
            get => _type;
            set
            {
                if (value > 0 && value <= 12)
                {
                    _type = value;
                }
                else
                {
                    ErrorList.Add("Тип соединения днища должен быть 1-12");
                }
            }
        }
        private int _type;
    }
}
