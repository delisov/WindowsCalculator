using System;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using CalcDmitriyElisov.ViewModels;
using CalcDmitriyElisov.Utility;
using CalcDmitriyElisov.Models;


namespace CalcDmitriyElisov.Commands
{
    public class PadCommand : DependencyObject, ICommand
    {
        private PadViewModel viewModel;

        public PadCommand(PadViewModel vm)
        {
            viewModel = vm;
        }

        #region ICommand Members

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            CalcOperation param = (CalcOperation)parameter;
            switch (param.Type)
            {
                case CalcOperation.OperationType.Digit:
                case CalcOperation.OperationType.EqualOp:
                    if ((new[] { CalcOperation.DotOp, CalcOperation.NegOp }.Contains(param)))
                    {
                        return viewModel.IsValidState;
                    }
                    return true;
                default:
                    return viewModel.IsValidState;
            }
        }

        public void Execute(object parameter)
        {
            PadViewModel.PadPressEventArgs args = new PadViewModel.PadPressEventArgs();
            args.Command = (CalcOperation)parameter;
            viewModel.OnPadPressed(args);
        }

        #endregion
    }
}
