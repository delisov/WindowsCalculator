using System;
using System.Windows.Input;

using CalcDmitriyElisov.Models;
using CalcDmitriyElisov.Commands;
using CalcDmitriyElisov.Utility;

namespace CalcDmitriyElisov.ViewModels
{
    public class PadViewModel
    {
        private Calculator calculator;
        public PadViewModel(Calculator calculator)
        {
            this.calculator = calculator;
        }        
        
        public bool IsValidState {
            get {
                return calculator.IsValidState;
            }
        }
        

        private PadCommand buttonPressCommand;

        public ICommand ButtonPressCommand
        {
            get
            {
                if (buttonPressCommand == null)
                {
                    buttonPressCommand = new PadCommand(this);
                }
                return buttonPressCommand;
            }
        }

        public class PadPressEventArgs : EventArgs
        {
            public CalcOperation Command { get; set; }
        }


        public event EventHandler<PadPressEventArgs> PadPressed;

        public virtual void OnPadPressed(PadPressEventArgs e)
        {
            EventHandler<PadPressEventArgs> handler = PadPressed;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
