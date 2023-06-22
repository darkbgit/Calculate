using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using CalculateVessels.Data.Exceptions;
using CalculateVessels.Data.PhysicalData.Gost34233_7.Models;
using CalculateVessels.Data.Utilities;

namespace CalculateVessels.Data.PhysicalData.Gost34233_7;

internal class Gost34233D7
{
    private const string GostName = "ГОСТ 34233.7-2017";
    private const string GostFolder = "Gost34233_7/Data";
    private const string Table1 = "Table1.json";
    private const string Table2 = "PhysicalData/Gost34233_7/Table2.json";
    private const string TABLE_B1 = "PhysicalData/Gost34233_7/TableB1.json";
    private const string TABLE_G1 = "PhysicalData/Gost34233_7/TableG1.json";
    private const string TableG23 = "TableG23.json";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="etaT"></param>
    /// <returns></returns>
    /// <exception cref="PhysicalDataException"></exception>
    public static double Getpsi0(double etaT)
    {

        List<Psi> psiList;

        try
        {
            using StreamReader file = new(TABLE_B1);
            var json = file.ReadToEnd();
            file.Close();
            psiList = JsonSerializer.Deserialize<List<Psi>>(json)
                      ?? throw new InvalidOperationException();
        }
        catch
        {
            throw new PhysicalDataException($"Error open file {TABLE_B1} for psi0");
        }

        var etaTRound = Math.Round(etaT * 20) / 20;

        var result = psiList?.FirstOrDefault(p => Math.Abs(p.etaT - etaTRound) < 0.00001) ??
                     throw new PhysicalDataException($"Couldn't find value of psi0 for etaT={etaT}");

        return result.psi0;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="D"></param>
    /// <returns></returns>
    /// <exception cref="PhysicalDataException"></exception>
    public static double GetWd(double D)
    {
        var fileName = Path.Combine(Constants.DataFolder, GostFolder, Table2);

        List<W> Ws;

        try
        {
            using StreamReader file = new(fileName);
            var json = file.ReadToEnd();
            file.Close();
            Ws = JsonSerializer.Deserialize<List<W>>(json)
                 ?? throw new InvalidOperationException();
        }
        catch
        {
            throw new PhysicalDataException($"Error open table {Table2} for [W]");
        }

        var result = Ws
                         .LastOrDefault(w => w.D <= D) ??
                     throw new PhysicalDataException($"Couldn't find value of [W] for D={D}");

        return result.W_d;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="omega"></param>
    /// <param name="mn"></param>
    /// <returns></returns>
    /// <exception cref="PhysicalDataException"></exception>
    public static (double T1, double T2, double T3) TryGetT1T2T3(double omega, double mn)
    {
        if (omega is < 0 or > 10)
        {
            throw new PhysicalDataException($"Error input value for omega {omega} value must be in range 0-10");
        }

        if (mn is < 1 or > 1.54)
        {
            throw new PhysicalDataException($"Error input value for mn {mn} value must be in range 1.0-1.5");
        }

        var omegaRound = omega <= 4
                ? Math.Round(omega * 2.0) / 2.0
                : Math.Round(omega);
        var mnRound = Math.Round(mn * 10.0) / 10.0;

        List<OmegaForT1T2T3> omegaList;

        try
        {
            using StreamReader file = new(TABLE_G1);
            var json = file.ReadToEnd();
            file.Close();
            omegaList = JsonSerializer.Deserialize<List<OmegaForT1T2T3>>(json) ?? throw new InvalidOperationException();
        }
        catch
        {
            throw new PhysicalDataException("Error open file for T1, T2, T3");
        }


        var result = omegaList
            ?.FirstOrDefault(o => Math.Abs(o.Omega - omegaRound) < 0.00001)
            ?.MnList
            .FirstOrDefault(m => Math.Abs(m.mn - mnRound) < 0.00001) ??
                     throw new PhysicalDataException($"Couldn't find value of T1, T2, T3 for omega={omega} and mn={mn}");
        return (result.T1, result.T2, result.T3);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="omega"></param>
    /// <returns></returns>
    /// <exception cref="PhysicalDataException"></exception>
    public static (double Phi1, double Phi2, double Phi3) GetPhi1Phi2Phi3(double omega)
    {
        if (omega is < 0 or > 11.5)
        {
            throw new PhysicalDataException($"Error input value for omega {omega} value must be in range 0-11");
        }

        var fileName = Path.Combine(Constants.DataFolder, GostFolder, Table1);

        var omegaRound = omega <= 4
            ? Math.Round(omega * 2.0) / 2.0
            : Math.Round(omega);

        if (Math.Abs(omegaRound - 11) < 0.000001)
        {
            return (Math.Sqrt(2) * omega, omega, Math.Sqrt(2) * omega);
        }

        List<OmegaForPhi1Phi2Phi3> omegaList;

        try
        {
            using StreamReader file = new(fileName);
            var json = file.ReadToEnd();
            file.Close();
            omegaList = JsonSerializer.Deserialize<List<OmegaForPhi1Phi2Phi3>>(json)
                        ?? throw new InvalidOperationException();
        }
        catch
        {
            throw new PhysicalDataException("Error open file for Phi1, Phi2, Phi3");
        }

        var result = omegaList
            .FirstOrDefault(o => Math.Abs(o.Omega - omegaRound) < 0.00001)
                     ?? throw new PhysicalDataException($"Couldn't find value of Phi1, Phi2, Phi3 for omega={omega}");

        return (result.Phi1, result.Phi2, result.Phi3);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="omega"></param>
    /// <param name="mA"></param>
    /// <returns></returns>
    /// <exception cref="PhysicalDataException"></exception>
    public static double GetA(double omega, double mA)
    {
        if (omega is < 0 or > 10)
        {
            throw new PhysicalDataException($"Error input value for omega {omega} value must be in range 0-10");
        }

        var fileName = Path.Combine(Constants.DataFolder, GostFolder, TableG23);

        var omegaRound = omega switch
        {
            <= 2 => Math.Round(omega * 2.0) / 2.0,
            <= 5 => Math.Round(omega),
            _ => omega < 7.5 ? 5 : 10
        };

        var mARound = Math.Round(mA * 10) / 10.0;

        List<OmegaForAB> omegaList;

        try
        {
            using StreamReader file = new(fileName);
            var json = file.ReadToEnd();
            file.Close();
            omegaList = JsonSerializer.Deserialize<List<OmegaForAB>>(json)
                        ?? throw new InvalidOperationException();
        }
        catch
        {
            throw new PhysicalDataException("Error open file for A");
        }


        var result = omegaList
            .FirstOrDefault(o => Math.Abs(o.Omega - omegaRound) < 0.00001)
            ?.mAnBList
            .FirstOrDefault(m => Math.Abs(m.Value - mARound) < 0.00001) ??
                     throw new PhysicalDataException($"Couldn't find value of A for omega={omega} and mA={mA}");
        return result.A;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="omega"></param>
    /// <param name="nB"></param>
    /// <returns></returns>
    /// <exception cref="PhysicalDataException"></exception>
    public static double GetB(double omega, double nB)
    {
        if (omega is < 0 or > 10)
        {
            throw new PhysicalDataException($"Error input value for omega {omega} value must be in range 0-10");
        }

        var fileName = Path.Combine(Constants.DataFolder, GostFolder, TableG23);

        var omegaRound = omega switch
        {
            <= 2 => Math.Round(omega * 2.0) / 2.0,
            <= 5 => Math.Round(omega),
            _ => omega < 7.5 ? 5 : 10
        };

        var nBRound = Math.Round(nB * 10) / 10.0;

        List<OmegaForAB> omegaList;

        try
        {
            using StreamReader file = new(fileName);
            var json = file.ReadToEnd();
            file.Close();
            omegaList = JsonSerializer.Deserialize<List<OmegaForAB>>(json)
                        ?? throw new InvalidOperationException();
        }
        catch
        {
            throw new PhysicalDataException("Error open file for B");
        }


        var result = omegaList
            .FirstOrDefault(o => Math.Abs(o.Omega - omegaRound) < 0.00001)
            ?.mAnBList
            .FirstOrDefault(n => Math.Abs(n.Value - nBRound) < 0.00001) ??
                     throw new PhysicalDataException($"Couldn't find value of B for omega={omega} and nB={nB}");

        return result.B;
    }
}