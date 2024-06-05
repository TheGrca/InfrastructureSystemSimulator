using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.Model
{
    public class CanvasAction<T>
    {
        public int number { get; }
        public Stack<T> Stack { get; }
        public CanvasAction(int number)
        {
            this.number = number;
            Stack = new Stack<T>();

        }
    }
}
