﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateVessels.Core.Interfaces
{
    public interface IWordProvider
    {
        void MakeWord(string filePath, ICalculatedData data);
    }
}
