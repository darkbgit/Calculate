using System.Collections.Generic;

namespace CalculateVessels.Core.Shells.Base
{
    public abstract class Shell
    {
        public IEnumerable<string> Bibliography { get; } = new []
        {
            Data.Properties.Resources.GOST_34233_1,
            Data.Properties.Resources.GOST_34233_2
        };
    }
}