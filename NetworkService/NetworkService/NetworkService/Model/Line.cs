using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.Model
{
    public class Line : BindableBase
    {
        private double x1, x2, y1, y2;
        private int start, end;
        public double X1
        {
            get { return x1; }
            set
            {
                x1 = value;
                OnPropertyChanged(nameof(X1));
            }
        }
        public double Y1
        {
            get { return y1; }
            set
            {
                y1 = value;
                OnPropertyChanged(nameof(Y1));
            }
        }
        public double X2
        {
            get { return x2; }
            set
            {
                x2 = value;
                OnPropertyChanged(nameof(X2));
            }
        }
        public double Y2
        {
            get { return y2; }
            set
            {
                y2 = value;
                OnPropertyChanged(nameof(Y2));
            }
        }
        public int Start
        {
            get { return start; }
            set
            {
                start = value;
                OnPropertyChanged(nameof(Start));
            }
        }
        public int End
        {
            get { return end; }
            set
            {
                end = value;
                OnPropertyChanged(nameof(End));
            }
        }
    }
}
