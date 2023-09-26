using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace RPN_Calculator
{
    public class RPN_CALC
    {
        private RPN rpnFunctions = new RPN();
    
        public void Calculator()
        {
            
            ArrayStack<double> Stack = new ArrayStack<double>();
            while (true)
            {
                ImprimirStack(Stack);
                Write("\n\nIngrese un número, operador o comando (SWAP, DUP, CLEAR, POP) o 'e' para salir: ");
                string entrada = ReadLine();

                if (entrada.ToLower() == "e")
                {
                    WriteLine("Saliendo de la calculadora.");
                    break;
                }

                if (EsNumero(entrada))
                {
                    Stack.Push(double.Parse(entrada));
                }
                else if (EsOperador(entrada))
                {
                    if (Stack.Size < 2)
                    {
                        WriteLine("Faltan operandos en el Stack.");
                        continue;
                    }

                    double operando2 = Stack.Pop();
                    double operando1 = Stack.Pop();
                    double resultado = AplicarOperador(operando1, operando2, entrada);
                    Stack.Push(resultado);
                }
                else if (entrada.ToLower() == "swap")
                {
                    if (Stack.Size < 2)
                    {
                        WriteLine("No hay suficientes elementos en el Stack para realizar SWAP.");
                        continue;
                    }

                    double operando2 = Stack.Pop();
                    double operando1 = Stack.Pop();
                    Stack.Push(operando2);
                    Stack.Push(operando1);
                }
                else if (entrada.ToLower() == "dup")
                {
                    if (Stack.Size == 0)
                    {
                        WriteLine("El Stack está vacío, no se puede realizar DUP.");
                        continue;
                    }

                    double valorTope = Stack.Peek();
                    Stack.Push(valorTope);
                }
                else if (entrada.ToLower() == "clear")
                {
                    Stack.Clear();
                    WriteLine("Stack vaciado.");
                }
                else if (entrada.ToLower() == "pop")
                {
                    if (Stack.Size == 0)
                    {
                        WriteLine("El Stack está vacío, no se puede realizar DESAPILAR.");
                        continue;
                    }

                    double valorEliminado = Stack.Pop();
                    WriteLine("Elemento eliminado: " + valorEliminado);
                }
                else
                {
                    try
                    {
                        double resultado = EvaluarExpresion(entrada, Stack);
                        Stack.Push(resultado);
                    }
                    catch (Exception e)
                    {
                        WriteLine("Error: " + e.Message);
                    }
                }
            }
        }

        private bool EsNumero(string entrada)
        {
            return double.TryParse(entrada, out _);
        }

        private bool EsOperador(string entrada)
        {
            return entrada == "+" || entrada == "-" || entrada == "*" || entrada == "/" || entrada =="^";
        }

        private double AplicarOperador(double operando1, double operando2, string operador)
        {
            switch (operador)
            {
                case "+":
                    return operando1 + operando2;
                case "-":
                    return operando1 - operando2;
                case "*":
                    return operando1 * operando2;
                case "/":
                    if (operando2 == 0)
                    {
                        WriteLine("No se puede dividir entre 0");
                        return operando1;
                    }
                    return operando1 / operando2;
                case "^":
                    return Math.Pow(operando1, operando2);
                default:
                    WriteLine("Operador no válido: " + operador);
                    return operando1;
            }
        }

        private double EvaluarExpresion(string expresion, ArrayStack<double> arrayStack)
        {
            
            string[] elements = expresion.Split(' ');
            List<string> elementsList = elements.ToList();

            if(elementsList.Count == 2 && arrayStack.Size > 0)
            {
                if( EsNumero(elementsList[0]) && EsOperador(elementsList[1]))
                {
                    return AplicarOperador(arrayStack.Pop(), double.Parse(elementsList[0]), elementsList[1]);
                }
                else
                {
                    throw new ArgumentException("Sintax Error");
                }

            }

            return rpnFunctions.EvaluateRPN(elementsList);
            
        }
        
        void ImprimirStack(ArrayStack<double> arrayStack)
        {
            double[] elementos = arrayStack.ToArray();
            WriteLine();
            WriteLine("-----------------------------------------------------");
            WriteLine();
            if(arrayStack.Size <= 3)
            {
                int contador = 0;
                for (int i = 2; i >= 0; i--)
                {
                    
                    if (i < arrayStack.Size)
                    {
                        WriteLine(elementos[contador++]);
                    }
                    WriteLine();
                    WriteLine("-----------------------------------------------------");
                    WriteLine();
                }
            }
            else
            {
                for (int i = arrayStack.Size - 3; i < arrayStack.Size; i++)
                {
                    WriteLine(elementos[i]);
                    
                    WriteLine();
                    WriteLine("-----------------------------------------------------");
                    WriteLine();
                }
            }
            
        }
        
    }   
}
