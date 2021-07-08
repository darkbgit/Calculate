using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CalculateVessels.Data.PhisicalData
{
    public static class Phisical
    {
        public static List<string> GetSteelsList()
        {
            using StreamReader file = new("PhisicalData/SteelsSigma.json");
            //try
            //{
                var json = file.ReadToEnd();
                var steels = JsonSerializer.Deserialize<Steels>(json);
                return steels.SteelsList.Select(l => l.Name).ToList();
            //}
            //catch
            //{
            //    return null;
            //}
        }
    }
}
