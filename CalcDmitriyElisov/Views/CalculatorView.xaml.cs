using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CalcDmitriyElisov.ViewModels;


namespace CalcDmitriyElisov.Views
{
    /// <summary>
    /// Interaction logic for CalculatorView.xaml
    /// </summary>
    public partial class CalculatorView : UserControl
    {
        public CalculatorView()
        {
            InitializeComponent();
            CalculatorViewModel viewmodel = new CalculatorViewModel(this);
            DataContext = viewmodel;
            this.PadView.DataContext = viewmodel.PVM;
            this.ScreenView.DataContext = viewmodel.SVM;
        }

    }
}

