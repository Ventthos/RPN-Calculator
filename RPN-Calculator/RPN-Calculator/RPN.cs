using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPN_Calculator
{
    internal class RPN
    {

        private int OpPriority(char op)
        {
            switch (op)
            {
                case '+':
                case '-':
                    return 1;
                case '*':
                case '/':
                    return 2;
                case '^':
                    return 3;
                case '(':
                    return 4;
                default:
                    throw new ArgumentException("Unexpected operator");
            }
        }
        public void Convertidor(string expresion)
        {
            ArrayStack<char> list = new ArrayStack<char>();
            List<string> elements = new List<string>();
            string number = "";
            for (int i = 0; i < expresion.Length; i++)
            {
                if (char.IsNumber(expresion[i]) || expresion[i] == '.')
                {
                    if (char.IsNumber(expresion[i + 1]) || expresion[i + 1] == '.')
                        if (expresion[i + 1] == '.' && number.Contains("."))
                            throw new ArgumentException("Sintaxis error");
                        number += expresion[i];
                }
                else
                {
                    if(number != "")
                    {
                        elements.Add(number);
                    }
                    if (expresion[i] == ')')
                    {
                        while(list.Peek() != '(')
                        {
                            elements.Add(list.Pop().ToString());
                        }

                    }
                    else
                    {
                        while (OpPriority(expresion[i]) >= OpPriority(list.Peek()))
                        {
                            elements.Add(list.Pop().ToString());
                        }
                    }
                }
            }

        }

        public double Evaluador(List<string> elements)
        {
            ArrayStack<double> actionQueue = new ArrayStack<double>();
            foreach (string element in elements)
            {
                if (double.TryParse(element, out double numero))
                {
                    actionQueue.Push(numero);
                }
                else if(element == "+" || element == "-" || element == "*" || element == "/" || element == "^")
                {
                    double numero2 = actionQueue.Pop();
                    double numero1 = actionQueue.Pop();
                    
                    switch (element)
                    {
                        case "+":
                            actionQueue.Push(numero1 + numero2);
                            break;

                        case "-":
                            actionQueue.Push(numero1 - numero2);
                            break;

                        case "*":
                            actionQueue.Push(numero1 * numero2);
                            break;

                        case "/":
                            if( numero2 == 0)
                            {
                                throw new DivideByZeroException("Tryed to divide by zero");
                            }
                            actionQueue.Push(numero1 / numero2);
                            break;

                        case "^":
                            actionQueue.Push(Math.Pow(numero1, numero2));
                            break;

                        default:
                            throw new ArgumentException("Unexpected operator");
                            
                    }
                }
                
            }
            
            if(actionQueue.Size > 1)
            {
                throw new ArgumentException("Sintax Error");
            }
            return actionQueue.Pop();
        }

        
    }
}
