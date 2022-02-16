using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Nozzle.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalculateVessels.Core.Shells.Base;

namespace CalculateVessels.Core.Shells.Nozzle
{
    public class NozzleInputData : IInputData
    {
        private List<string> _errorList = new();

        private readonly ICalculatedData _shellCalculatedData;

        public NozzleInputData(ICalculatedData shellCalculatedData)
        {
            _shellCalculatedData = shellCalculatedData;
        }


        public IEnumerable<string> ErrorList => _errorList;

        

        public ICalculatedData ShellCalculatedData => _shellCalculatedData;

        //public ShellDataIn ShellDataIn => (ShellDataIn) _shellCalculatedData.InputData;

        public double t
        {
            get => _t;
            set => _t = value;
        }

        public string steel1
        {
            get => _steel1;
            set => _steel1 = value;
        }

        public double SigmaAllow1
        {
            get => _sigmaAllow1;
            set => _sigmaAllow1 = value;
        }

        public double E1
        {
            get => _E1;
            set => _E1 = value;
        }

        public double E2 { get; set; }
        public double E3 { get; set; }
        public double E4 { get; set; }

        public double d
        {
            get => _d;
            set => _d = value;
        }

        public double s1
        {
            get => _s1;
            set => _s1 = value;
        }

        public double s2
        {
            get => _s2;
            set => _s2 = value;
        }

        public double s3
        {
            get => _s3;
            set => _s3 = value;
        }

        public double cs
        {
            get => _cs;
            set => _cs = value;
        }

        public double cs1
        {
            get => _cs1;
            set => _cs1 = value;
        }

        public double l1
        {
            get => _l1;
            set => _l1 = value;
        }

        public NozzleKind NozzleKind
        {
            get => _nozzleKind;
            set => _nozzleKind = value;
        }

        public double fi
        {
            get => _fi;
            set => _fi = value;
        }

        public double fi1
        {
            get => _fi1;
            set => _fi1 = value;
        }

        public double delta { get; set; }
        public double delta1 { get; set; }
        public double delta2 { get; set; }

        public string steel2 { get; set; }
        public string steel3 { get; set; }
        public string steel4 { get; set; }

        public double l2 { get; set; }
        public double l3 { get; set; }

        public NozzleLocation Location { get; set; }
        public double omega { get; set; }
        public double tTransversely { get; set; }
        public double ellx { get; set; }
        public double gamma { get; set; }
        public string Name { get; set; }
        public bool IsOval { get => isOval; set => isOval = value; }
        public double d1 { get; set; }
        public double d2 { get; set; }
        public double r { get; set; }
        public double s0 { get; set; }
        public double SigmaAllow2 { get; set; }
        public double SigmaAllow3 { get; set; }
        public double SigmaAllow4 { get; set; }
        public byte l { get; set; }

        private double _t;
        private string _steel1;
        private double _sigmaAllow1;

        private double _E1;
        private double _d;
        private double _s1;
        private double _s2;
        private double _s3;
        private double _cs;
        private double _cs1;
        private double _l1;
        private NozzleKind _nozzleKind;
        private double _fi;
        private double _fi1;

        private bool isOval;

        //public IElement Element { get; set; }

    }
}
