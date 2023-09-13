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

            string expresion = "(3-8)+(5-(-2)))";
            List<string> expresList = rpnCalc.Convertidor(expresion);
            string final = String.Join(" ", expresList);
            Console.WriteLine(final);
            Console.WriteLine($"Resultado: {rpnCalc.Evaluador(expresList)}");
        }
    }
}
