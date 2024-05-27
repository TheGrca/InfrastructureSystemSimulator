using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.Model
{
    public class LineModel : BindableBase
    {
        private string _startCanvas;
        public string StartCanvas
        {
            get { return _startCanvas; }
            set
            {
                if (_startCanvas != value)
                {
                    _startCanvas = value;
                    OnPropertyChanged(nameof(StartCanvas));
                }
            }
        }

        private string _endCanvas;
        public string EndCanvas
        {
            get { return _endCanvas; }
            set
            {
                if (_endCanvas != value)
                {
                    _endCanvas = value;
                    OnPropertyChanged(nameof(EndCanvas));
                }
            }
        }

        public LineModel(string startCanvas, string endCanvas)
        {
            _startCanvas = startCanvas;
            _endCanvas = endCanvas;
        }
    }
}
