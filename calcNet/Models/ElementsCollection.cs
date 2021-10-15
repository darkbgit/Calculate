using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateVessels.Models
{
    public class ElementsCollection
    {
        public IList<CalculatedElement> Elements { get; set; }

        public ElementsCollection()
        {
            Elements = new List<CalculatedElement>();
        }
    }
}
