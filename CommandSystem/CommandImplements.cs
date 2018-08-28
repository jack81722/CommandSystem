using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CommandSystem
{
    public sealed class StateCommand : Command<CommandSystem>
    {
        public StateCommand(CommandSystem receiver) : base(receiver)
        {
            cmd = "state";
        }

        public override void Excute(string[] args)
        {
            Console.WriteLine("Console UI System (CUS) is online.");
        }
    }    

    public sealed class ClearCommand : Command<CommandSystem>
    {
        public ClearCommand(CommandSystem receiver) : base(receiver)
        {
            cmd = "clear";
        }

        public override void Excute(string[] args)
        {
            Console.Clear();
        }
    }

    public sealed class RepeatCommand : Command<CommandSystem>
    {
        public RepeatCommand(CommandSystem receiver) : base(receiver)
        {
            cmd = "repeat";
        }

        public override void Excute(string[] args)
        {

            //int cmdLength = 1;

            //int countIndex = Array.IndexOf(args, "-count");
            //int count;
            //if (!int.TryParse(args[countIndex + 1], out count))
            //{
            //    throw new Exception("Invalid count setting.");
            //}
            //else
            //{
            //    cmdLength += 2;
            //}

            //int intervalIndex = Array.IndexOf(args, "-interval");
            //int interval;
            //if(!int.TryParse(args[intervalIndex + 1], out interval))
            //{
            //    throw new Exception("Invalid interval setting");
            //}
            //else
            //{
            //    cmdLength += 2;
            //}

            //StringBuilder subCmdBuilder = new StringBuilder();
            //for(int i = cmdLength; i < args.Length; i++)
            //{
            //    subCmdBuilder.Append(args[i]);
            //    if(i < args.Length - 1)
            //    {
            //        subCmdBuilder.Append(" ");
            //    }
            //}
            //Console.WriteLine(subCmdBuilder.ToString());
            //int time = 0;
            //while (time < count)
            //{
            //    Thread.Sleep(interval);
            //    CommandSystem.Command(subCmdBuilder.ToString());
            //    time++;
            //}
        }
    }

    public sealed class TestCommand : Command<CommandSystem>
    {
        public TestCommand(CommandSystem receiver) : base(receiver)
        {
            cmd = "test";
        }

        public override void Excute(string[] args)
        {
            if (args.Length == 2)
            {
                for (int i = 0; i < int.Parse(args[1]); i++)
                {
                    Console.WriteLine("Testing");
                    if (i < int.Parse(args[1]) - 1)
                        System.Threading.Thread.Sleep(1000);
                }
            }
        }
    }
}
