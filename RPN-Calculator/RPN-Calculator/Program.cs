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

            RPN rpnFunc = new RPN();

            WriteLine("¿A qué metodo desea acceder?\n  1. Checar parentesis\n  2. Convertir a RPN y resolver \n  3. Calculadora RPN");
            int opcion = int.Parse(ReadLine());
            if( opcion == 1)
            {
                while (true)
                {
                    WriteLine("Escriba la expresión");
                    string expresion = ReadLine();
                    if(expresion == "")
                    {
                        WriteLine("Cerrando...");
                        break;
                    }
                    bool bien = rpnFunc.CheckParentheses(expresion);
                    WriteLine($"¿La expresión está correcta? {bien}");
                }
                
            }
            else if(opcion == 2)
            {
                while (true)
                {
                    WriteLine("Escriba la expresión");
                    string expresion = ReadLine();

                    List<string> expresList = rpnFunc.ConvertToRPN(expresion);
                    string final = String.Join(" ", expresList);
                    WriteLine(final);
                    WriteLine($"Resultado: {rpnFunc.EvaluateRPN(expresList)}");
                }
            }
            else if( opcion == 3)
            {
                WriteLine("Calculadora RPN");
                RPN_CALC rpnCalc = new RPN_CALC();
                rpnCalc.Calculator();
            }
             
               
        }
    }
}
