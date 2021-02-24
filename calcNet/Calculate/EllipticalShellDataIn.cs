﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calcNet
{
    class EllipticalShellDataIn : ShellDataIn, IDataIn
    {
        public EllipticalShellDataIn()
            : base(ShellType.Elliptical)
        {

        }

        //private double _ellx;
        private double _ellH;
        private double _ellh1;
        private EllipticalBottomType _ellipticalBottomType;



        //public double ellx { get => _ellx; set => _ellx = value; }
        public double ellH { get => _ellH; }
        public double ellh1 { get => _ellh1; }
        public EllipticalBottomType EllipticalBottomType { get => _ellipticalBottomType; }
        
    }
}