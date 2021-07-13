using System;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Enums;

namespace CalculateVessels.Core.Shells.DataIn
{
    public class CylindricalShellDataIn : ShellDataIn, IDataIn
    {
        public CylindricalShellDataIn()
            : base(ShellType.Cylindrical)
        {

        }

        public double l
        {
            get => _l;
            set
            {
                if (value > 0)
                {
                    _l = value;
                }
                else
                {
                    isError = true;
                    errorList.Add("l должно быть больше 0");
                }
            }
        }
        public double l3 { get; set; }


        private double _l;
        private double _l3;


        public double fi_t { get; set; }

        public bool ConditionForCalcF5341 { get; set; }

        private int _FCalcSchema;
        public int FCalcSchema
        {
            get => _FCalcSchema;
            set
            {
                if (value > 0 && value <= 7)
                {
                    _FCalcSchema = value;
                }
                else
                {
                    isError = true;
                    errorList.Add("Схема расчета F [1,7]");
                }
            }
        } //1-7

        public double f { get; set; }

        public bool IsFTensile { get; set; }
    }

}
