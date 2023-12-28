using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Savanna.Core.Interfaces;

namespace Savanna.Core.Views
{
    public class InputManager : IInputManager
    {

        public ConsoleKey ReadKey()
        {
            return Console.ReadKey().Key;
        }

        public void Clear()
        {
            Console.Clear();
        }

        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }

        public void Write(string text)
        {
            Console.Write(text);
        }

        public string ReadLine()
        {
            return Console.ReadLine();
        }
    }

}
