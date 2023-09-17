using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
        private char OpeningParentheses(char closing)
        {
            switch (closing)
            {
                case ')':
                    return '(';
                case ']':
                    return '[';
                case '}':
                    return '{';
                default:
                    return ' ';
            }
        }

        public bool CheckParentheses(string expression)
        {
            ArrayStack<char> stack = new ArrayStack<char>();
            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] == '(' || expression[i] == '[' || expression[i] == '{')
                {
                    stack.Push(expression[i]);
                }
                else if (expression[i] == ')' || expression[i] == ']' || expression[i] == '}')
                {
                    if (stack.Size > 0 && stack.Peek() == OpeningParentheses(expression[i]))
                    {
                        stack.Pop();
                        continue;
                    }
                    return false;
                }
            }
            if (stack.Empty)
            {
                return true;
            }
            
            return false;
           
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
                case '[':
                case '{':
                    return 5;
                default:
                    throw new ArgumentException("Unexpected operator");
            }
        }

       
        private char checkBackwards(string expression, int i)
        {
            for (int x = i - 1; x >= 0; x--)
            {
                if (expression[x] != ' ')
                {
                    return expression[x];
                }
            }
            return ' ';
        }
        private char checkFordward(string expression, int i)
        {
            for (int x = i + 1; x < expression.Length; x++)
            {
                if (expression[x] != ' ')
                {
                    return expression[x];
                }
            }
            return ' ';
        }
        private void OutputOperators(string expresion, ArrayStack<char> stack, List<string> elements, char currentChar)
        {
            while (true)
            {
                if (stack.Empty || OpPriority(stack.Peek()) < OpPriority(currentChar) || stack.Peek() == '(' || stack.Peek() == '[' || stack.Peek() == '{')
                {
                    //Console.WriteLine($"Agregue {expresion[i]}");
                    stack.Push(currentChar);
                    break;
                }
                //Console.WriteLine($"Encontre un signo de menor importancia {stack.Peek()} es menor que el actual {expresion[i]}");
                elements.Add(stack.Pop().ToString());
            }
        }

        // TODO: Hay qu hacer que esta cosa acete numeros negativos y el -(), o sea que si pone negativo antes de algo, lo interprete
        // igual que si hay un numero delante de un paréntesis lo tome como multiplicación
        public List<string> Convertidor(string expresion)
        {
            ArrayStack<char> stack = new ArrayStack<char>();
            List<string> elements = new List<string>();
            string number = "";
            for (int i = 0; i < expresion.Length; i++)
            {
                if (Char.IsDigit(expresion[i]))
                {
                    if((i + 1 != expresion.Length) && (Char.IsDigit(checkFordward(expresion, i)) || checkFordward(expresion, i) == '.'))
                    {
                        
                        number += expresion[i];
                        Console.WriteLine(number);
                    }
                    else
                    {
                        if (number != "")
                        {
                            number += expresion[i];
                            elements.Add(number);
                            number = "";
                        }
                        else
                        {
                            elements.Add(expresion[i].ToString());
                        }
                    }
                   
                }
                else if(expresion[i] == ' '){}
                else if (expresion[i] == '.')
                {
                    if (number.Contains('.') && expresion[i] == '.') 
                    {
                        throw new ArgumentException("Sintax Error");
                    }
                    number += expresion[i];
                }
                else
                {
                    if (expresion[i] == ')' || expresion[i] == ']' || expresion[i] == '}')
                    {
                        while (!stack.Empty)
                        {
                            if (stack.Peek() == '(' || stack.Peek() == '[' || stack.Peek() == '{')
                            {
                                stack.Pop();
                                break;
                            }
                            Console.WriteLine($"Quite {stack.Peek()}");
                            elements.Add(stack.Pop().ToString());
                            
                        }
                    }
                    else
                    {
                        if (expresion[i] == '-' && !Char.IsDigit(checkBackwards(expresion, i)) && checkBackwards(expresion, i) != ')' && checkBackwards(expresion, i) != ']' && checkBackwards(expresion, i) != '}' && Char.IsDigit(checkFordward(expresion, i)))
                        {
                            number += expresion[i];
                        }
                        else
                        {
                            if ((expresion[i] == '(' || expresion[i] == '[' || expresion[i] == '{') && Char.IsDigit(checkBackwards(expresion, i)))
                            {
                                OutputOperators(expresion, stack, elements, '*');
                            }
                            OutputOperators(expresion, stack, elements, expresion[i]);
                        }
                        
                    }
                }
                
           
            }
            Console.WriteLine(stack.GetDataText());
            Console.WriteLine(stack.Size);
            int stackSize = stack.Size;
            for (int i = 0; i < stackSize; i++)
            {
                Console.WriteLine("Agregando: "+stack.Peek());
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
                    if(actionQueue.Size >= 2)
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
                                if (numero2 == 0)
                                {
                                    throw new DivideByZeroException("Tried to divide by zero");
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
                    else
                    {
                        throw new ArgumentException("Sintax error");
                    }
                    
                }
                else
                {
                    throw new ArgumentException("Sintax error");
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
