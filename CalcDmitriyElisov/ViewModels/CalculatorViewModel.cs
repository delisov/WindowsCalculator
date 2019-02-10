using System;
using System.ComponentModel;
using System.Windows;
using System.Linq;
using CalcDmitriyElisov.Models;
using CalcDmitriyElisov.ViewModels;
using CalcDmitriyElisov.Commands;
using CalcDmitriyElisov.Views;
using CalcDmitriyElisov.Utility;

namespace CalcDmitriyElisov.ViewModels
{
    public class CalculatorViewModel
    {
        #region private members
        private const CalcOperation.OperationType digit = CalcOperation.OperationType.Digit;
        private const CalcOperation.OperationType doubleOp = CalcOperation.OperationType.DoubleOp;
        private const CalcOperation.OperationType singleOp = CalcOperation.OperationType.SingleOp;
        private const CalcOperation.OperationType percentOp = CalcOperation.OperationType.PercentOp;
        private const CalcOperation.OperationType equalOp = CalcOperation.OperationType.EqualOp;
        private bool awaitingSecondOperand = false;// needed to save state after CE button
        private Calculator calculator;
        private Screen screen; //just a member of calculator
        #endregion

        #region Child view models
        private PadViewModel pvm;
        public PadViewModel PVM
        {
            get { return pvm; }
        }

        private ScreenViewModel svm;
        public ScreenViewModel SVM
        {
            get { return svm; }
        }

        private MemoryViewModel mvm;
        public MemoryViewModel MVM
        {
            get { return mvm; }
        }

        #endregion

        public CalculatorViewModel(CalculatorView view)
        {
            calculator = new Calculator();
            screen = calculator.Screen;
            pvm = new PadViewModel(calculator);
            svm = new ScreenViewModel(calculator.Screen);
            mvm = new MemoryViewModel(/*dummy*/);
            pvm.PadPressed += c_PadPressedHandler;
        }

        void c_PadPressedHandler(object sender,  PadViewModel.PadPressEventArgs e)
        {
            try {
                switch (e.Command.Type)
                {
                    case digit:
                        DigitButtonPress(e.Command.Name);
                        awaitingSecondOperand = false;
                        break;
                    case doubleOp:
                        DoubleOperationButtonPress(e.Command.Name);
                        calculator.CurrentDoubleOperation = e.Command;
                        break;
                    case singleOp:
                        SingularOperationButtonPress(e.Command.Name);
                        break;
                    case equalOp:
                        EqualOperationButtonPress(e.Command.Name);
                        break;
                    case percentOp:
                        PercentOperationButtonPress(e.Command.Name);
                        break;
                    default:
                        screen.MainInfo = "Нет такой команды: " + e.Command.Type.ToString();
                        break;
                }
            }
            catch(Exception ex)
            {
                calculator.IsValidState = false;
                screen.MainInfo = ex.Message;
            }
            finally
            {
                calculator.PrevOperation = e.Command;
            }
        }


        private void Reset()
        {
            calculator.IsValidState = true;
            screen.MainInfo = "0";
            svm.ClearHistory();
            calculator.FirstOperand = 0;
            calculator.SecondOperand = 0;
            calculator.FirstOperandSet = false;
            awaitingSecondOperand = false;
            calculator.PrevOperation = null;
            calculator.CurrentDoubleOperation = null;
            calculator.PercentageBase = 0;
        }

        public void DigitButtonPress(string button)
        {
            if (!calculator.IsValidState)
            {
                Reset();
            }

            if (button == CalcOperation.ButtonCE.Name)
            {
                if (calculator.PrevOperation.Type == singleOp)
                {
                    svm.RemoveLast(true);
                }
                if (calculator.FirstOperandSet) awaitingSecondOperand = true;
                screen.MainInfo = "0";
            }
            else if (button == CalcOperation.ButtonC.Name)
            {
                Reset();
            }
            else if (button == CalcOperation.DelOp.Name)
            {
                if (!new[] { digit, equalOp }.Contains(calculator.PrevOperation.Type))
                {
                    return;
                }
                if (screen.MainInfo.Length > 1)
                    screen.MainInfo = screen.MainInfo.Substring(0, screen.MainInfo.Length - 1);
                else screen.MainInfo = "0";
            }
            else if (button == CalcOperation.NegOp.Name)
            {
                if (screen.MainInfo.Contains("-") || screen.MainInfo == "0")
                {
                    screen.MainInfo = screen.MainInfo.Remove(screen.MainInfo.IndexOf("-"), 1);
                }
                else screen.MainInfo = "-" + screen.MainInfo;
                if (svm.LastIsOperand)
                {
                    svm.WrapLast(CalcOperation.NegOp.Name);
                }
            }
            else if (button == CalcOperation.DotOp.Name)
            {
                if (awaitingSecondOperand)
                {
                    screen.MainInfo = "0" + CalcOperation.DotOp.Name;
                }
                else
                {
                    if (!screen.MainInfo.Contains(CalcOperation.DotOp.Name))
                    {
                        screen.MainInfo = screen.MainInfo + CalcOperation.DotOp.Name;
                    }
                }
            }
            else
            {
                if (calculator.PrevOperation == null)
                {
                    calculator.FirstOperandSet = false;
                    awaitingSecondOperand = false;
                    screen.MainInfo = button;
                }
                else if (calculator.PrevOperation.Type == singleOp)
                {
                    svm.RemoveLast(true);
                    screen.MainInfo = button;
                }
                else if (screen.MainInfo == "0" || awaitingSecondOperand)
                {
                    screen.MainInfo = button;
                }
                else
                {
                    screen.MainInfo += button;
                }
                awaitingSecondOperand = false;
            }
        }
        

        //for operations with 2 operands
        public void DoubleOperationButtonPress(string operation)
        {
            // Double-press of double operation
            if (calculator.PrevOperation.Type == doubleOp)
            {
                // Replace operand in history
                svm.ReplaceLast(operation, false);
                return;
            }

            // While inputting first operand
            if (!calculator.FirstOperandSet)
            {
                calculator.FirstOperand = Convert.ToDouble(screen.MainInfo);
                calculator.PercentageBase = calculator.FirstOperand;
                calculator.FirstOperandSet = true;
            }
            // Other cases
            // After single op
            // After equals
            // After percent
            else
            {
                calculator.SecondOperand = Convert.ToDouble(screen.MainInfo);
                calculator.FirstOperand =
                    Calculations.CalculateResultDoubleOp(calculator.FirstOperand, calculator.SecondOperand, operation);
                calculator.PercentageBase = calculator.FirstOperand;
            }

            if (!svm.LastIsOperand)
            {
                svm.AddToHistory(screen.MainInfo, true);
            }
            svm.AddToHistory(operation, false);
            screen.MainInfo = calculator.FirstOperand.ToString();
            awaitingSecondOperand = true;
        }

        public void SingularOperationButtonPress(string operation)
        {
                // Press after pressing double-op
                if (awaitingSecondOperand)
                {
                    screen.MainInfo = Calculations.CalculateResultSingularOp(calculator.FirstOperand, operation).ToString();
                    svm.AddToHistory(operation + "(" + Math.Round(calculator.FirstOperand, 10).ToString() + ")", true);
                    awaitingSecondOperand = false;
                    return;
                }

                // Press after pressing single-op
                if (svm.LastIsOperand)
                {
                    svm.WrapLast(operation);
                }
                // Press while entering first operand
                else
                {
                    svm.AddToHistory(svm.WrapOp(Convert.ToDouble(screen.MainInfo).ToString(), operation), true);
                }

                // Main screen changes the same for 
                // "while entering first operand" and 
                // "after pressing single-op"
                screen.MainInfo =
                        Calculations.CalculateResultSingularOp(Convert.ToDouble(screen.MainInfo), operation).ToString();

                if (calculator.FirstOperandSet)
                    awaitingSecondOperand = true;
                else
                {
                    awaitingSecondOperand = false;
                }
        }


        public void EqualOperationButtonPress(string operation)
        {
                svm.ClearHistory();

                //After error 
                {
                    if (!calculator.IsValidState)
                    {
                        Reset();
                        return;
                    }
                }

                // Double-press of "="
                if (calculator.PrevOperation.Type == equalOp ||
                    (calculator.PrevOperation.Type == singleOp && !calculator.FirstOperandSet) )
                {
                    double firstOperand =
                           Convert.ToDouble(screen.MainInfo);
                    if (calculator.CurrentDoubleOperation != null)
                    {
                        screen.MainInfo =
                               Calculations.CalculateResultDoubleOp(firstOperand, calculator.SecondOperand, calculator.CurrentDoubleOperation.Name).ToString();
                    }
                    return;
                }

                // press after singular op on one operand or while entering first operand
                if (!calculator.FirstOperandSet)
                {
                    calculator.FirstOperand = Convert.ToDouble(screen.MainInfo);
                }

                // press after second operand entered
                else
                {
                    calculator.FirstOperandSet = false;
                    calculator.SecondOperand = Convert.ToDouble(screen.MainInfo);
                    calculator.FirstOperand =
                        Calculations.CalculateResultDoubleOp(calculator.FirstOperand, calculator.SecondOperand, calculator.CurrentDoubleOperation.Name);
                    screen.MainInfo = calculator.FirstOperand.ToString();
                }
                awaitingSecondOperand = false;
        }

        public void PercentOperationButtonPress(string operation)
        {
                // Still not entered first operand
                if (!calculator.FirstOperandSet && 
                    !((calculator.PrevOperation.Type == equalOp && calculator.CurrentDoubleOperation != null)) ) {
                    svm.ClearHistory();
                    svm.AddToHistory("0", true);
                    screen.MainInfo = "0";
                }
                else {
                    // If after =, and there was a double operation before that
                    if (calculator.PrevOperation.Type == equalOp && calculator.CurrentDoubleOperation != null)
                    {
                        screen.MainInfo = (Math.Pow(Convert.ToDouble(screen.MainInfo), 2) / 100).ToString();
                    }
                    // Press when entering second operand
                    else if (new[] { CalcOperation.MultOp, CalcOperation.DivOp }.Contains(calculator.CurrentDoubleOperation))
                    {
                        screen.MainInfo = (0.01 * Convert.ToDouble(screen.MainInfo)).ToString();
                    }
                    else
                    {
                        screen.MainInfo = (calculator.PercentageBase / 100 * Convert.ToDouble(screen.MainInfo)).ToString();
                    }

                    if (svm.LastIsOperand)
                    {
                        svm.ReplaceLast(screen.MainInfo, true);
                    }
                    else
                    {
                        svm.AddToHistory(screen.MainInfo, true);
                    }
                    awaitingSecondOperand = false;
                    return;
                }
                
                if (calculator.FirstOperandSet)
                    awaitingSecondOperand = true;
                else
                {
                    awaitingSecondOperand = false;
                }
        }
    }
}
