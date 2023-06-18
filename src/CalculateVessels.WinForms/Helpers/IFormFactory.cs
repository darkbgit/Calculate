using System.Windows.Forms;

namespace CalculateVessels.Helpers;

public interface IFormFactory
{
    T? Create<T>()
     where T : Form;

    Form? Create(string calculatedElementType);
}