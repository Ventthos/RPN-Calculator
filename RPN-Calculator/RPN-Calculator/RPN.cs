﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RPN_Calculator
{
    internal class RPN
    {
       
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

        private bool EsOperador(char entrada)
        {
            return entrada == '+' || entrada == '-' || entrada == '*' || entrada == '/';
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

        public bool CheckExpressionOperators(string expression)
        {
            expression = expression.Replace(" ", "");
            Console.WriteLine(expression);
            for (int i = 0; i < expression.Length; i++)
            {
                if(i + 1 <= expression.Length)
                {
                    if(EsOperador(expression[i]) && EsOperador(expression[i + 1]))
                    {
                        return false;
                    }
                }
            }
            return true;
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

       
        
        private void OutputOperators(string expresion, ArrayStack<char> stack, List<string> elements, char currentChar)
        {
            while (true)
            {
                if (stack.Empty || OpPriority(stack.Peek()) < OpPriority(currentChar) || stack.Peek() == '(' || stack.Peek() == '[' || stack.Peek() == '{')
                {
                   
                    stack.Push(currentChar);
                    break;
                }
                
                elements.Add(stack.Pop().ToString());
            }
        }

        
        public List<string> Convertidor(string expresion)
        {
            ArrayStack<char> stack = new ArrayStack<char>();
            List<string> elements = new List<string>();
            string number = "";
            if(CheckParentheses(expresion) && CheckExpressionOperators(expresion))
            {
                for (int i = 0; i < expresion.Length; i++)
                {
                    if (Char.IsDigit(expresion[i]))
                    {
                        if ((i + 1 != expresion.Length) && (Char.IsDigit(checkFordward(expresion, i)) || checkFordward(expresion, i) == '.'))
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
                    else if (expresion[i] == ' ') { }
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
                    Console.WriteLine("Agregando: " + stack.Peek());
                    elements.Add(stack.Pop().ToString());
                }
                return elements;
            }
            throw new ArgumentException("Sintax error");

        }

        // TODO: Hacer que haga que si, es negativo, y solo queda un elemento, entonces vuelva el elemento negativo, tipo, que acepte -(6 + 3)
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
                    else if (actionQueue.Size == 1 && element == "-")
                    {
                        double number = actionQueue.Pop();
                        actionQueue.Push(number * -1);
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
