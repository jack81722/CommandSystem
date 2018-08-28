using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommandSystem
{
    class Program
    {
        private static InputSystem input;

        static void Main(string[] args)
        {
            input = new InputSystem();
            input.AddDefaultEvent();
            input.AddKeyEvent(new SearchKeyWord(CommandSystem.instance, input));
            var task = Task.Factory.StartNew(Run);
            task.Wait();
        }

        /// <summary>
        /// Main function of Console UI System
        /// </summary>
        private static void Run()
        {
            string str;
            while (true)
            {
                if ((str = input.ReadLine(">")) != null && str.ToUpper() != "EXIT")
                {
                    var n = Task.Factory.StartNew(() => CommandSystem.Command(str));
                    n.Wait();
                }
                else { break; }
            }
        }
    }

    

}
