using System;

namespace CommandSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            DropoutStack<int> stack = new DropoutStack<int>();
            for(int i = 0; i < 30; i++)
            {
                stack.Push(i);
                Console.WriteLine(i + ", count = " + stack.Count);
            }

            // CommandSystem.Wait();
        }
    }
}
