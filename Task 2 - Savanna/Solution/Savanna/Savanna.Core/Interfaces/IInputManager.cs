using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savanna.Core.Interfaces
{
    public interface IInputManager
    {
        ConsoleKey ReadKey();
        string ReadLine();
        void Clear();
        void WriteLine(string text);
        void Write(string text);
    }
}
