﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPN_Calculator
{
    internal class RPN
    {
       
        
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
                throw new ArgumentException("Unexpected operator");
            }
            
            if(actionQueue.Size > 1)
            {
                throw new ArgumentException("Sintax Error");
            }
            return actionQueue.Pop();
        }

        public void 
    }
}
