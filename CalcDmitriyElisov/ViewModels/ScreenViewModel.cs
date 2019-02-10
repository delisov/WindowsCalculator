using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalcDmitriyElisov.Models;
using CalcDmitriyElisov.Utility;

namespace CalcDmitriyElisov.ViewModels
{
    public class ScreenViewModel
    {
        #region private fields
        private ObservableCollection<string> opHistory = new ObservableCollection<string>();
        private StringBuilder sb = new StringBuilder("", 100);
        private Screen screen;
        #endregion 

        public Screen Screen
        {
            get
            {
                return screen;
            }
        }

        public ScreenViewModel(Screen screen)
        {
            this.screen = screen;
            opHistory.CollectionChanged += this.UpdateSecondaryScreen;
        }


        private void UpdateSecondaryScreen(object sender, NotifyCollectionChangedEventArgs args)
        {
            try
            {
                ObservableCollection<string> obsSender = sender as ObservableCollection<string>;
                Screen.SecondaryInfo = obsSender.Aggregate("", (a, b) => a + " " + b);
            }
            catch
            {
                Screen.SecondaryInfo = "";
            }
        }

        public void ClearHistory()
        {
            LastIsOperand = false;
            opHistory.Clear();
        }

        public string WrapOp(string innerOp, string outerOp)
        {
            sb.Clear();
            if (outerOp == CalcOperation.ByXOp.Name)
                return sb.AppendFormat("1/({0})", innerOp).ToString();
            else
                return sb.AppendFormat("{0}({1})", outerOp, innerOp).ToString();
        }
        public void AddToHistory(string op, bool isOperand)
        {
            LastIsOperand = isOperand;
            opHistory.Add(op);
        }
        public void ReplaceLast(string op, bool isOperand)
        {
            if (LastIsOperand != isOperand)
                throw new Exception("Type mismatch in op history");
            opHistory[opHistory.Count - 1] = op;
        }
        public void RemoveLast(bool isOperand)
        {
            if (LastIsOperand != isOperand)
                throw new Exception("Type mismatch in op history");
            opHistory.Remove(opHistory.Last());
        }
        public void WrapLast(string op)
        {
            if (opHistory.Count == 0) return;
            if (LastIsOperand != true)
                throw new Exception("Type mismatch in op history");
            opHistory[opHistory.Count - 1] = WrapOp(opHistory[opHistory.Count - 1], op);
        }
        public bool LastIsOperand
        {
            get;
            private set;
        }
    }
}
