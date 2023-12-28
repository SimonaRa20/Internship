using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.ConsoleApp
{
    public interface IInputManager
    {
        void WriteLine(string message);
        string ReadLine();
        void Write(string value);
    }
}
