using System;

namespace CommandSystem
{
    /// <summary>
    /// Command base class for Console UI System
    /// </summary>
    public abstract class CommandBase
    {
        /// <summary>
        /// Command string
        /// </summary>
        public string cmd;

        /// <summary>
        /// Execute command
        /// </summary>
        /// <remarks>
        /// Command Line will be splitted into args by UI system 
        /// </remarks>
        /// <param name="args"></param>
        public abstract void Excute(string[] args);

        public virtual void Excute(string cmd, string[] args) { }
    }

    /// <summary>
    /// Common command
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Command<T> : CommandBase
    {
        protected T receiver;

        public Command(T receiver)
        {
            this.receiver = receiver;
        }

        public override void Excute(string[] args)
        {
            throw new NotImplementedException();
        }


        protected virtual void Help()
        {
            Console.WriteLine("Help will print usage.");
        }
    }
}
