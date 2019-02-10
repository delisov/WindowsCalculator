using System;
using System.ComponentModel;

namespace CalcDmitriyElisov.Models
{
    public class Calculator
    {
        private Screen screen = new Screen();
        public Screen Screen { get { return screen; } }


        private double firstOperand = 0;
        public double FirstOperand
        {
            get { return firstOperand; }
            set { firstOperand = value; }
        }

        private bool firstOperandSet = false;
        public bool FirstOperandSet
        {
            get { return firstOperandSet; }
            set { firstOperandSet = value; }
        }


        private double secondOperand = 0;
        public double SecondOperand
        {
            get { return secondOperand; }
            set { secondOperand = value; }
        }

        private CalcOperation prevOperation = CalcOperation.ButtonC;
        public CalcOperation PrevOperation
        {
            get { return prevOperation; }
            set { prevOperation = value; }
        }

        private CalcOperation currentDoubleOperation = null;
        public CalcOperation CurrentDoubleOperation
        {
            get { return currentDoubleOperation; }
            set { currentDoubleOperation = value; }
        }

        private double percentageBase = 0;
        public double PercentageBase
        {
            get { return percentageBase; }
            set { percentageBase = value; }
        }

        public bool isValidState = true;
        public bool IsValidState
        {
            get { return isValidState; }
            set { isValidState = value; }
        }
    }
}


