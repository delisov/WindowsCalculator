using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcDmitriyElisov.Models
{
    // Immutable class
    public class CalcOperation : IEquatable<CalcOperation>
    {
        public enum OperationType { Digit, SingleOp, DoubleOp, PercentOp, EqualOp } ;

        #region fields and properties
        public readonly OperationType type;
        public OperationType Type { get { return type; } }

        public readonly string name;
        public string Name { get { return name; } }

        public readonly string button;
        public string Button { get { return button; } }
        #endregion


        // Dummy element to be used in XAML for object casting
        public CalcOperation()
        {
        }

        public CalcOperation(OperationType type, string name, string button)
        {
            this.type = type;
            this.name = name;
            this.button = button;
        }

        public bool Equals(CalcOperation other)
        {
            if (other == null)
                return false;

            if (this.Type == other.Type && this.Name == other.Name && this.Button == other.Button)
                return true;
            else
                return false;
        }

        public override bool Equals(Object obj)
        {
            if (obj == null)
                return false;

            CalcOperation other = obj as CalcOperation;
            if (other == null)
                return false;
            else
                return Equals(other);
        }

        public override int GetHashCode()
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(this, null)) return 0;

            int hashType = Type.GetHashCode();
            int hashName = Name == null ? 0 : Name.GetHashCode();
            int hashButton = Button == null ? 0 : Name.GetHashCode();

            //Calculate the hash code for the product.
            return hashType ^ hashName ^ hashButton;
        }

        // To be used in XAML
        public override string ToString()
        {
            return Button;
        }

        #region digits
        public static CalcOperation ButtonCE
        {
            get { return new CalcOperation(OperationType.Digit, "CE", "CE"); }
        }
        public static CalcOperation ButtonC
        {
            get { return new CalcOperation(OperationType.Digit, "C", "C"); }
        }
        public static CalcOperation Digit0
        {
            get { return new CalcOperation(OperationType.Digit, "0", "0"); }
        }

        public static CalcOperation Digit1
        {
            get { return new CalcOperation(OperationType.Digit, "1", "1"); }
        }

        public static CalcOperation Digit2
        {
            get { return new CalcOperation(OperationType.Digit, "2", "2"); }
        }
        public static CalcOperation Digit3
        {
            get { return new CalcOperation(OperationType.Digit, "3", "3"); }
        }
        public static CalcOperation Digit4
        {
            get { return new CalcOperation(OperationType.Digit, "4", "4"); }
        }
        public static CalcOperation Digit5
        {
            get { return new CalcOperation(OperationType.Digit, "5", "5"); }
        }
        public static CalcOperation Digit6
        {
            get { return new CalcOperation(OperationType.Digit, "6", "6"); }
        }
        public static CalcOperation Digit7
        {
            get { return new CalcOperation(OperationType.Digit, "7", "7"); }
        }
        public static CalcOperation Digit8
        {
            get { return new CalcOperation(OperationType.Digit, "8", "8"); }
        }
        public static CalcOperation Digit9
        {
            get { return new CalcOperation(OperationType.Digit, "9", "9"); }
        }

        public static CalcOperation DelOp
        {
            get { return new CalcOperation(OperationType.Digit, "Del", "⌫"); }
        }

        public static CalcOperation DotOp
        {
            get { return new CalcOperation(OperationType.Digit, ",", ","); }
        }
        public static CalcOperation NegOp
        {
            get { return new CalcOperation(OperationType.Digit, "negate", "±"); }
        }
        #endregion

        #region double op
        public static CalcOperation PlusOp
        {
            get { return new CalcOperation(OperationType.DoubleOp, "+", "+"); }
        }

        public static CalcOperation MinusOp
        {
            get { return new CalcOperation(OperationType.DoubleOp, "-", "-"); }
        }

        public static CalcOperation MultOp
        {
            get { return new CalcOperation(OperationType.DoubleOp, "×", "×"); }
        }

        public static CalcOperation DivOp
        {
            get { return new CalcOperation(OperationType.DoubleOp, "÷", "÷"); }
        }
        #endregion

        #region single op
        public static CalcOperation SqrOp
        {
            get { return new CalcOperation(OperationType.SingleOp, "sqr", "𝑥²"); }
        }
        public static CalcOperation SqrtOp
        {
            get { return new CalcOperation(OperationType.SingleOp, "√", "√"); }
        }
        public static CalcOperation ByXOp
        {
            get { return new CalcOperation(OperationType.SingleOp, "1/x", "¹/𝑥"); }
        }
        #endregion

        public static CalcOperation PercentOp
        {
            get { return new CalcOperation(OperationType.PercentOp, "%", "%"); }
        }

        public static CalcOperation EqualOp
        {
            get { return new CalcOperation(OperationType.EqualOp, "=", "="); }
        }
        
    }
}

