using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPN_Calculator
{
    internal class RPN
    {
        public bool Check(string expresion)
        {
            ArrayStack<char> stack = new ArrayStack<char>();
            for (int i = 0; i < expresion.Length; i++)
            {
                switch (expresion[i])
                {
                    case '(':
                        stack.Push('(');
                        break;

                    case '{':
                        stack.Push('{');
                        break;

                    case '[':
                        stack.Push('[');
                        break; 
                }
                
            }
            for (int i = 0; i < expresion.Length; i++)
            {
                switch (expresion[i])
                {
                    case ')':
                        stack.Pop();
                        break;

                    case '}':
                        stack.Pop();
                        break;

                    case ']':
                        stack.Pop();
                        break;
                }
            }
            bool isRight = false;
            for (int i = 0; i < expresion.Length; i++)
            {
                bool bandera = false;
                switch (expresion[i])
                {
                    case '+':
                        bandera = true;
                        break;
                    case '-':
                        bandera = true;
                        break;
                    case '*':
                        bandera = true;
                        break;
                    case '/':
                        bandera = true;
                        break;
                    case '^':
                        bandera = true;
                        break;

                }
            }
            if (stack.Empty)
            {
                return true;
            } else
            {
                return false;
            }
        }

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
        public List<string> Convertidor(string expresion)
        {
            ArrayStack<char> stack = new ArrayStack<char>();
            List<string> elements = new List<string>();
            string number = "";
            for (int i = 0; i < expresion.Length; i++)
            {
                if (Char.IsDigit(expresion[i]))
                {
                    elements.Add(expresion[i].ToString());
                }
                else if (expresion[i] == ')')
                {
                    while (!stack.Empty)
                    {
                        if (stack.Peek() == '(')
                        {
                            stack.Pop();
                            break;
                        }
                        elements.Add(stack.Pop().ToString());
                    }
                }
                else
                {

                    while (true)
                    {
                        if (stack.Empty || OpPriority(stack.Peek()) < OpPriority(expresion[i]) || stack.Peek() == '(')
                        {
                            stack.Push(expresion[i]);
                            break;
                        }
                        elements.Add(stack.Pop().ToString());
                    }
                }
            
            }
            Console.WriteLine(stack.GetDataText());
            for (int i = 0; i <= stack.Size; i++)
            {
                elements.Add(stack.Pop().ToString());
            }
            return elements;

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
