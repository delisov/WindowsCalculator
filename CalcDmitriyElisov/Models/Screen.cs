using System;
using System.ComponentModel;

namespace CalcDmitriyElisov.Models
{
    public class Screen : BaseNPC
    {
        private string mainInfo = "0";
        public string MainInfo
        {
            get { return mainInfo; }
            set
            {
                mainInfo = value;
                OnPropertyChanged("MainInfo");
            }
        }

        private string secondaryInfo = "";
        public string SecondaryInfo
        {
            get { return secondaryInfo; }
            set
            {
                secondaryInfo = value;
                OnPropertyChanged("SecondaryInfo");
            }
        }
    }
}
