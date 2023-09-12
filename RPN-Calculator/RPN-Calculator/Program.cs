using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPN_Calculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RPN rpnCalc = new RPN();
            List<string> lista = new List<string>() { "6", "3", "2", "+", "5", "4", "-", "*", "10", "5", "-", "/", "+" };

            Console.WriteLine(rpnCalc.Evaluador(lista));
        }
    }
}
