using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Models;
using CalculateVessels.Core.Shells;
using CalculateVessels.Data.Properties;

namespace CalculateVessels
{
    public class CalculateElement
    {
        private readonly IElement _element;

        private readonly Form _form;
        

        public CalculateElement(IElement element, Form form)
        {
            _element = element;
            _form = form;
        }


        public CalculatedElement Calculate(bool preCalc)
        {
            CalculatedElement calculatedElement = new(_element);

            calculatedElement.Element.Calculate();

            if (!calculatedElement.Element.IsCriticalError)
            {

                if (!preCalc)
                {
                    if (_form.Owner is MainForm main)
                    {
                        main.Word_lv.Items.Add(calculatedElement.Element.ToString());
                        main.ElementsCollection.Add(calculatedElement);

                        _form.Hide();
                    }
                    else
                    {
                        MessageBox.Show("MainForm Error");
                    }
                }


                if (calculatedElement.Element.IsError)
                {
                    MessageBox.Show(string.Join<string>(Environment.NewLine, calculatedElement.Element.ErrorList));
                }

                MessageBox.Show(Resources.CalcComplete);

            }
            else
            {
                MessageBox.Show(string.Join<string>(Environment.NewLine, calculatedElement.Element.ErrorList));
                calculatedElement = null;
            }

            return calculatedElement;
        }
        
    }
}
