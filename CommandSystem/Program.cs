using System;
using System.Collections.Generic;

namespace CommandSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            // CommandSystem.Wait();

            string cmd = "test object1 object -set param1 -set param2 param3";
            string[] subCmds = cmd.Split('-');
            for(int i = 0; i < subCmds.Length; i++)
            {
                Console.WriteLine(subCmds[i]);
            }
        }
    }


    public class TestCmd<T> : CommandBase
    {
        protected T receiver;

        protected string cmdStr;

        protected class Setting
        {
            public string setStr;


        }



        public void Execute(string cmd)
        {
            string[] cubCmds = cmd.Split('-');

        }

        public override void Excute(string[] args)
        {
            throw new NotImplementedException();
        }
    }


}
