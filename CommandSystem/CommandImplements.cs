using System;
using System.Collections.Generic;
using System.Text;

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
