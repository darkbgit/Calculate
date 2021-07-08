using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace CalculateVessels.Data.PhysicalData
{
    public static class Physical
    {
        public static List<string> GetSteelsList()
        {
            using StreamReader file = new("PhysicalData/SteelsSigma.json");
            //try
            //{
                var json = file.ReadToEnd();
                var steels = JsonSerializer.Deserialize<List<Steel>>(json);
                return steels.Select(l => l.Name).ToList();
            //}
            //catch
            //{
            //    return null;
            //}
        }
    }
}
