using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalculateVessels
{
    public class CalclulateElement
    {
        private readonly IElement _element;

        private readonly Form _ownerForm;

        public CalclulateElement(IElement element, Form ownerForm)
        {
            _element = element;
            _ownerForm = ownerForm;
        }

        public void Calculate()
        {
            CalculatedElement calculatedElement = new(_element);

            calculatedElement.Element.Calculate();

            if (!calculatedElement.Element.IsCriticalError)
            {
                scalc_l.Text = $"sp={((EllipticalShell)calculatedElement.Element).s:f3} мм";
                p_d_l.Text = $"pd={((EllipticalShell)calculatedElement.Element).p_d:f3} МПа";

                if (_tthis.Owner is MainForm main)
                {
                    main.Word_lv.Items.Add(calculatedElement.Element.ToString());
                    main.ElementsCollection.Add(calculatedElement);
                }
                else
                {
                    MessageBox.Show("MainForm Error");
                }

                if (calculatedElement.Element.IsError)
                {
                    MessageBox.Show(string.Join<string>(Environment.NewLine, calculatedElement.Element.ErrorList));
                }

                MessageBox.Show("Calculation complete");

                MessageBoxCheckBox mbcb = new(calculatedElement.Element, _ellipticalShellDataIn) { Owner = this };
                mbcb.ShowDialog();
            }
            else
            {
                MessageBox.Show(string.Join<string>(Environment.NewLine, calculatedElement.Element.ErrorList));
            }
        }

        
    }
}
