using System;
using System.Collections.Generic;
using System.Text;

namespace CommandSystem
{
    /// <summary>
    /// Extend features of Console UI
    /// </summary>
    public sealed class InputSystem
    {
        /// <summary>
        /// Current string typed by user
        /// </summary>
        public string currentStr { get { return builder.ToString(); } }

        /// <summary>
        /// String builder for combine all symbols
        /// </summary>
        private StringBuilder builder;

        /// <summary>
        /// Title length would not be saved in string builder
        /// </summary>
        private int cursorOffset;

        /// <summary>
        /// Current position of cursor
        /// </summary>
        private int cursor;

        /// <summary>
        /// All specific key events
        /// </summary>
        private Dictionary<ConsoleKey, KeyEvent> keyEvents;

        /// <summary>
        /// Constructor of input system
        /// </summary>
        public InputSystem()
        {
            builder = new StringBuilder();           
            keyEvents = new Dictionary<ConsoleKey, KeyEvent>();
        }

        /// <summary>
        /// Add default key event
        /// </summary>
        public void AddDefaultEvent()
        {
            AddKeyEvent(new BackspaceHandler(this));
            AddKeyEvent(new DeleteHandler(this));
            AddKeyEvent(new LeftArrowHandler(this));
            AddKeyEvent(new RightArrowHandler(this));
        }

        /// <summary>
        /// Add defined key event
        /// </summary>
        /// <param name="_event">defined event</param>
        public void AddKeyEvent(KeyEvent _event)
        {
            keyEvents.Add(_event.key, _event);
        }

        /// <summary>
        /// Remove one key event
        /// </summary>
        /// <param name="_event"></param>
        public void RemoveKeyEvent(KeyEvent _event)
        {
            keyEvents.Remove(_event.key);
        }

        /// <summary>
        /// BackSpace function
        /// </summary>
        public void BackSpace()
        {
            cursor = Console.CursorLeft;
            if (Console.CursorLeft > cursorOffset)
            {
                cursor = Console.CursorLeft - 1;
                builder.Remove(Console.CursorLeft - 1 - cursorOffset, 1);
                ClearCurrentLine(cursorOffset);
                Console.Write(builder.ToString());
                Console.SetCursorPosition(cursor, Console.CursorTop);
            }
        }

        /// <summary>
        /// Delete function
        /// </summary>
        public void Delete()
        {
            cursor = Console.CursorLeft;
            if (Console.CursorLeft - cursorOffset < builder.Length)
            {
                cursor = Console.CursorLeft;
                builder.Remove(Console.CursorLeft - cursorOffset, 1);
                ClearCurrentLine(cursorOffset);
                Console.Write(builder.ToString());
                Console.SetCursorPosition(cursor, Console.CursorTop);
            }
        }
        
        /// <summary>
        /// Cursor move function (Left)
        /// </summary>
        public void CursorLeftMove()
        {
            if (Console.CursorLeft > cursorOffset)
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
        }

        /// <summary>
        /// Cursor move function (Right)
        /// </summary>
        public void CursorRightMove()
        {
            if (Console.CursorLeft - cursorOffset < builder.Length)
                Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
        }
        
        /// <summary>
        /// Type word programly
        /// </summary>
        /// <param name="word"></param>
        public void AutoType(string word)
        {
            builder.Append(word);
            ClearCurrentLine(cursorOffset);
            Console.Write(builder.ToString());
        }

        private string title;

        public void Reshow()
        {
            Console.Write(title);
            Console.Write(builder.ToString());
        }

        /// <summary>
        /// Read line and show title
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public string ReadLine(string title = "")
        {
            this.title = title;
            Console.Write(title);
            cursorOffset = cursor = Console.CursorLeft;
            builder.Clear();
            var input = Console.ReadKey(true);
            
            while (input.Key != ConsoleKey.Enter)
            {
                if(keyEvents.ContainsKey(input.Key))
                {
                    keyEvents[input.Key].OnKeyPress();
                }
                else
                {
                    char c = Console.CapsLock ? input.KeyChar : char.ToLower(input.KeyChar);
                    cursor = Console.CursorLeft + 1;

                    if (Console.CursorLeft - cursorOffset < builder.Length)
                        builder.Insert(Console.CursorLeft - cursorOffset, c);
                    else
                        builder.Append(c);
                    ClearCurrentLine(cursorOffset);
                    Console.Write(builder.ToString());
                    Console.SetCursorPosition(cursor, Console.CursorTop);
                }
                input = Console.ReadKey(true);
            }
            Console.Write("\n");
            return builder.ToString();
        }

        /// <summary>
        /// Clear one line
        /// </summary>
        public static void ClearCurrentLine()
        {
            var currentLine = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLine);
        }

        /// <summary>
        /// Clear one line start at start index
        /// </summary>
        /// <param name="startIndex"></param>
        public static void ClearCurrentLine(int startIndex)
        {
            var currentLine = Console.CursorTop;
            Console.SetCursorPosition(startIndex, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(startIndex, currentLine);
        }
    }

    #region Key Event : Console event of specific key such Tab, Delete, LeftArrow, ... etc.
    public abstract class KeyEvent
    {
        public readonly ConsoleKey key;

        public KeyEvent(ConsoleKey key)
        {
            this.key = key;
        }

        public abstract void OnKeyPress();
    }

    public class BackspaceHandler : KeyEvent
    {
        private InputSystem system;

        public BackspaceHandler(InputSystem system) : base(ConsoleKey.Backspace)
        {
            this.system = system;
        }

        public override void OnKeyPress()
        {
            system.BackSpace();
        }
    }

    public class DeleteHandler : KeyEvent
    {
        private InputSystem system;

        public DeleteHandler(InputSystem system) : base(ConsoleKey.Delete)
        {
            this.system = system;
        }

        public override void OnKeyPress()
        {
            system.Delete();
        }
    }

    public class LeftArrowHandler : KeyEvent
    {
        private InputSystem system;

        public LeftArrowHandler(InputSystem system) : base(ConsoleKey.LeftArrow)
        {
            this.system = system;
        }

        public override void OnKeyPress()
        {
            system.CursorLeftMove();
        }
    }

    public class RightArrowHandler : KeyEvent
    {
        private InputSystem system;

        public RightArrowHandler(InputSystem system) : base(ConsoleKey.RightArrow)
        {
            this.system = system;
        }

        public override void OnKeyPress()
        {
            system.CursorRightMove();
        }
    }

    public class SearchKeyWord : KeyEvent
    {
        private InputSystem input;
        private CommandSystem system;

        public SearchKeyWord(CommandSystem system, InputSystem input) : base(ConsoleKey.Tab)
        {
            this.system = system;
            this.input = input;
        }

        public override void OnKeyPress()
        {
            string[] cmds = system.SearchCmd(input.currentStr);
            StringBuilder builder = new StringBuilder();
            if (cmds.Length == 1)
            {
                input.AutoType(cmds[0].Substring(input.currentStr.Length));
            }
            else if (cmds.Length > 1)
            {
                Console.WriteLine();

                for (int i = 0; i < cmds.Length; i++)
                {
                    builder.Append(string.Format("{0}. {1,-8}", i + 1, cmds[i]));
                }
                Console.WriteLine(builder.ToString());

                input.Reshow();
            }
        }
        #endregion
    }
}
