using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        private bool EsOperador(string entrada)
        {
            return entrada == "+" || entrada == "-" || entrada == "*" || entrada == "/" || entrada == "^";
        }
        private bool EsParentesisAbertura(char entrada)
        {
            return entrada == '(' || entrada == '[' || entrada == '{';
        }
        private bool EsParentesisCierre(char entrada)
        {
            return entrada == ')' || entrada == ']' || entrada == '}';
        }

        private char checkBackwards(string expression, int i)
        {
            if (i - 1 >= 0)
            {
                return expression[i - 1];
            }
            return ' ';
        }

        private char checkFordward(string expression, int i)
        {
            if(i + 1 < expression.Length)
            {
                return expression[i + 1];
            }
            return ' ';
        }

        public bool CheckExpressionOperators(string expression)
        {
            expression = expression.Replace(" ", "");
            for (int i = 0; i < expression.Length; i++)
            {
                if(i + 1 < expression.Length)
                {
                    if(EsOperador(expression[i].ToString()) && (EsOperador(expression[i + 1].ToString()) || EsParentesisCierre(expression[i+1])))
                    {
                        if(expression[i + 1] == '-')
                        {
                            if (i + 2 != expression.Length && Char.IsDigit(expression[i + 2]))
                            {
                                continue;
                            }
                        }
                        return false;
                    }
                }
            }
            if (EsOperador(expression[expression.Length - 1].ToString()))
                return false;

            return true;
        }

        public bool CheckParentheses(string expression)
        {
            ArrayStack<char> stack = new ArrayStack<char>();
            for (int i = 0; i < expression.Length; i++)
            {
                if (EsParentesisAbertura(expression[i]))
                {
                    stack.Push(expression[i]);
                }
                else if (EsParentesisCierre(expression[i]))
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
                if (stack.Empty || OpPriority(stack.Peek()) < OpPriority(currentChar) || EsParentesisAbertura(stack.Peek()))
                {
                   
                    stack.Push(currentChar);
                    break;
                }
                
                elements.Add(stack.Pop().ToString());
            }
        }

        
        public List<string> ConvertToRPN(string expresion)
        {
            ArrayStack<char> stack = new ArrayStack<char>();
            List<string> elements = new List<string>();
            string number = "";
            if(CheckParentheses(expresion) && CheckExpressionOperators(expresion))
            {
                expresion = expresion.Replace(" ", "");
                for (int i = 0; i < expresion.Length; i++)
                {
                    if (Char.IsDigit(expresion[i]))
                    {
                        if ((i + 1 != expresion.Length) && (Char.IsDigit(checkFordward(expresion, i)) || checkFordward(expresion, i) == '.'))
                        {

                            number += expresion[i];
                            
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
                 
                    else if (expresion[i] == '.')
                    {
                        if (number.Contains('.') || !Char.IsDigit(checkFordward(expresion, i)) )
                        {
                            throw new ArgumentException("Sintax Error");
                        }
                        number += expresion[i];
                    }
                    else
                    {
                        if (EsParentesisCierre(expresion[i]))
                        {
                            while (!stack.Empty)
                            {
                                if (EsParentesisAbertura(stack.Peek()))
                                {
                                    stack.Pop();
                                    break;
                                }
                                elements.Add(stack.Pop().ToString());

                            }
                        }
                        else
                        {
                            char backwardElement = checkBackwards(expresion, i);
                            if (expresion[i] == '-' && !Char.IsDigit(backwardElement) && !EsParentesisCierre(backwardElement) && Char.IsDigit(checkFordward(expresion, i)))
                            {
                                number += expresion[i];
                            }
                            else if (expresion[i] == '+' && !Char.IsDigit(backwardElement) && !EsParentesisCierre(backwardElement) && Char.IsDigit(checkFordward(expresion, i)))
                            {
                               
                            }
                            else
                            {
                                if (EsParentesisAbertura(expresion[i]) && (Char.IsDigit(backwardElement)|| EsParentesisCierre(backwardElement)))
                                {
                                    OutputOperators(expresion, stack, elements, '*');
                                }
                                OutputOperators(expresion, stack, elements, expresion[i]);
                            }

                        }
                    }


                }
                
                int stackSize = stack.Size;
                for (int i = 0; i < stackSize; i++)
                {
                    elements.Add(stack.Pop().ToString());
                }
                return elements;
            }
            throw new ArgumentException("Sintax error");

        }

        public double EvaluateRPN(List<string> elements)
        {
            ArrayStack<double> actionQueue = new ArrayStack<double>();
            foreach (string element in elements)
            {
                if (double.TryParse(element, out double numero))
                {
                    actionQueue.Push(numero);
                }
                else if(EsOperador(element))
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
                    else if (actionQueue.Size == 1 && element == "+") { }
                    
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
