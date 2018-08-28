using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace CommandSystem
{
    /// <summary>
    /// Console UI System (CUS).
    /// </summary>
    /// <remarks>
    /// This class is used for user to input and execute commands of other subsystems. 
    /// User may interherite Command<T> and define its actions.
    /// </remarks>
    public sealed class CommandSystem
    {
        private static readonly CommandSystem _instance = new CommandSystem();
        public static CommandSystem instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// Command dictionary for save and search command instance
        /// </summary>
        private Dictionary<string, CommandBase> cmdDict;


        /// <summary>
        /// Constructor of Console UI System
        /// </summary>
        private CommandSystem()
        { 
            cmdDict = new Dictionary<string, CommandBase>();
            _Init();
        }

        /// <summary>
        /// Initialize all Console Ui System commands
        /// </summary>
        private void _Init()
        {
            _RegistCmd(new StateCommand(this));
            _RegistCmd(new ClearCommand(this));
            _RegistCmd(new TestCommand(this));
            _RegistCmd(new RepeatCommand(this));
        }

        /// <summary>
        /// Execute command and multiple settings
        /// </summary>
        /// <param name="cmd">command</param>
        public static void Command(object c)
        {
            var cmd = (string)c;
            string[] args = cmd.Split(' ');
            if (args.Length > 0)
            {
                try
                {
                    instance.cmdDict[args[0]].Excute(args);
                }
                catch (KeyNotFoundException knfe)
                {
                    if (args[0].Length > 0)
                    {
                        Console.WriteLine("No command \"" + args[0] + "\" found.");
                    }
                }
            }
        }

        /// <summary>
        /// Register command into Console UI System.
        /// </summary>
        /// <param name="cmd">command instance</param>
        private void _RegistCmd(CommandBase cmd)
        {
            cmdDict.Add(cmd.cmd, cmd);
        }

        /// <summary>
        /// Register command into Console UI System
        /// </summary>
        /// <param name="cmd">command instance</param>
        public static void RegistCmd(CommandBase cmd)
        {
            instance._RegistCmd(cmd);
        }
        
        public string[] SearchCmd(string keyWord)
        {
            string[] cmds = cmdDict.Keys.Where(x => Match(x, keyWord)).ToArray();
            Array.Sort(cmds);
            return cmds;
        }

        public bool Match(string str1, string str2)
        {
            bool result = true;
            int min = Math.Min(str1.Length, str2.Length);
            for(int i = 0; i < min; i++)
            {
                if(!(result = str1[i].Equals(str2[i])))
                {
                    break;
                }
            }
            return result;
        }
    }

    
}
