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

        //public void CheckData()
        //{
        //    IsDataGood = !(ErrorList?.Count > 0);
        //}

        //public bool IsDataGood { get; set; }

        public bool IsNeedpCalculate { get => isNeedpCalculate; set => isNeedpCalculate = value; }

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
        public double l3_1 { get => _l3_1; set => _l3_1 = value; }
        public double l3_2 { get => _l3_2; set => _l3_2 = value; }

        private bool isNeedpCalculate;

        private double _l;
        private double _l3_1;
        private double _l3_2;

        public double fi_t { get; set; }
    }

}
