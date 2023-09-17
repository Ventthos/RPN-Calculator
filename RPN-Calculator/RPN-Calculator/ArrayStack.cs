using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPN_Calculator
{
        public class ArrayStack<T>
        {
           
            private const int INITIAL_CAPACITY = 10;
            private T[] data;
            public int Capacity { get; private set; }

            public int Size { get; private set; }

            public bool Empty => Size == 0;

            public ArrayStack()
            {
                data = new T[INITIAL_CAPACITY];
                Capacity = INITIAL_CAPACITY;
                Size = 0;
            }
            public ArrayStack(int capacity)
            {
                data = new T[capacity];
                Capacity = capacity;
                Size = 0;
            }
            public ArrayStack(ArrayStack<T> stack)
            {
                data = new T[stack.Capacity];
                Array.Copy(stack.data, data, stack.Capacity);
                Capacity = stack.Capacity;
                Size = stack.Size;
            }

            public T Pop()
            {
                if (Empty)
                {
                    throw new IndexOutOfRangeException("Empty stack");
                }
                return data[--Size];
            }

            public void Push(T element)
            {
                //Esta lleno?
                if (Size == Capacity)
                {
                    Capacity *= 2;
                    Array.Resize(ref data, 2 * Capacity);
                }//Ref pasa el arreglo real, no una copia
                data[Size++] = element;

            }

            public T Peek()
            {
                if (Empty)
                {
                    throw new IndexOutOfRangeException("Empty stack");
                }
                return data[Size - 1];
            }

            public void Clear()
            {
            Size = 0;
            }

            public T[] ToArray()
            {
                return data;
            }
#if DEBUG
            public string GetDataText()
            {
               
                return $"[{String.Join(", ", data.Take(Size))}]";
            }
#endif
        
        }
}
