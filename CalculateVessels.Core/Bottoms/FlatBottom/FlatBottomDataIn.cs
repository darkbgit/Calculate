using CalculateVessels.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalculateVessels.Core.Bottoms.Enums;

namespace CalculateVessels.Core.Bottoms.FlatBottom
{
    public class FlatBottomDataIn : IDataIn
    {
        //public bool IsDataGood => !ErrorList.Any();

        public IEnumerable<string> ErrorList => _errorList;

        private List<string> _errorList = new();


        public double a { get; set; }
        public double c1 { get; set; }
        public double c2 { get; set; }
        public double c3 { get; set; }
        public double d { get; set; }
        public double D { get; set; }
        public double D2 { get; set; }
        public double D3 { get; set; }
        public double Dcp { get; set; }
        public double di { get; set; }
        public double fi { get; set; }
        public double h1 { get; set; }
        public double p { get; set; }
        public double r { get; set; }
        public double s { get; set; }
        public double s1 { get; set; }
        public double s2 { get; set; }
        public double sigma_d { get; set; }
        public double t { get; set; }
        public string Steel { get; set; }
        public double E { get; set; }
        public string Name { get; set; }




        public HoleInFlatBottom Hole { get; set; }

        public double gamma { get; set; }

        public int Type
        {
            get => _type;
            set
            {
                if (value is > 0 and <= 12)
                {
                    _type = value;
                }
                else
                {
                    _errorList.Add("Тип соединения днища должен быть 1-12");
                }
            }
        }
        private int _type;
    }
}
