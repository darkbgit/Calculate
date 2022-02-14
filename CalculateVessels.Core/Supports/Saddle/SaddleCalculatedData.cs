﻿using System.Collections.Generic;
using CalculateVessels.Core.Interfaces;

namespace CalculateVessels.Core.Supports.Saddle
{
    public class SaddleCalculatedData : ICalculatedData
    {
        public SaddleCalculatedData(SaddleInputData inputData)
        {
            InputData = inputData;
            ErrorList = new List<string>();
        }

        public IInputData InputData { get; }
        public ICollection<string> ErrorList { get; }
        public double SigmaAllow { get; set; }
        public double E { get; set; }
        public double ny { get; set; }
        public double p_d { get; set; }
        public double B1_2 { get; set; }
        public double B1 { get; set; }
        public double F_d { get; set; }
        public double M_dp { get; set; }
        public double M_de { get; set; }
        public double M_d { get; set; }
        public double Q_dp { get; set; }
        public double Q_de { get; set; }
        public double Q_d { get; set; }
        public double q { get; set; }
        public double M0 { get; set; }
        public double F1 { get; set; }
        public double F2 { get; set; }
        public double M1 { get; set; }
        public double M2 { get; set; }
        public double M12 { get; set; }
        public double Q1 { get; set; }
        public double Q2 { get; set; }
        public double y { get; set; }
        public double x { get; set; }
        public double K9_1 { get; set; }
        public double K9 { get; set; }
        public double ConditionStrength1_1 { get; set; }
        public double ConditionStrength1_2 { get; set; }
        public double ConditionStability1 { get; set; }
        public double gamma { get; set; }
        public double beta1 { get; set; }
        public double K10_1 { get; set; }
        public double K10 { get; set; }
        public double K11 { get; set; }
        public double K12 { get; set; }
        public double K13 { get; set; }
        public double K14 { get; set; }
        public double K15_2 { get; set; }
        public double K15 { get; set; }
        public double K16 { get; set; }
        public double K17 { get; set; }
        public double sigma_mx { get; set; }
        public double v1_2 { get; set; }
        public double v1_3 { get; set; }
        public double K2 { get; set; }
        public double v21_2 { get; set; }
        public double v21_3 { get; set; }
        public double v22_2 { get; set; }
        public double v22_3 { get; set; }
        public double K1_2For_v21 { get; set; }
        public double K1_2For_v22 { get; set; }
        public double K1_2 { get; set; }
        public double K1_3For_v21 { get; set; }
        public double K1_3For_v22 { get; set; }
        public double K1_3 { get; set; }
        public double sigmai2_1 { get; set; }
        public double sigmai2_2 { get; set; }
        public double sigmai2 { get; set; }
        public double sigmai3_1 { get; set; }
        public double sigmai3_2 { get; set; }
        public double sigmai3 { get; set; }
        public double F_d2 { get; set; }
        public double F_d3 { get; set; }
        public double ConditionStrength2 { get; set; }
        public double Fe { get; set; }
        public double ConditionStability2 { get; set; }
        public double sef { get; set; }
        public bool IsConditionUseFormulas { get; set; }
        public double Ak { get; set; }
    }
}