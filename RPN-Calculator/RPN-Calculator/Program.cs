using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace RPN_Calculator
{
    internal class Program
    {
        static void Main(string[] args)
        {

            
             RPN rpnCalc = new RPN();

             Console.WriteLine("Escriba la expresión");
             string expresion = Console.ReadLine();
             
             List<string> expresList = rpnCalc.Convertidor(expresion);
             string final = String.Join(" ", expresList);
             Console.WriteLine(final);
             Console.WriteLine($"Resultado: {rpnCalc.Evaluador(expresList)}");



            /*
            WriteLine("Calculadora RPN");
            Stack<double> Stack = new Stack<double>();
            RPN rpn = new RPN();
            RPN_CALC rpnCalc = new RPN_CALC();
            rpnCalc.Calculator();
           

            RPN rpnCalc = new RPN();
            while (true)
            {
                Console.WriteLine("Escriba la expresión");
                string expresion = Console.ReadLine();
                bool bien = rpnCalc.CheckParentheses(expresion);
                Console.WriteLine(bien);
            }
              */

        }
    }
}
