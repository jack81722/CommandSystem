using System;
using System.Collections.Generic;
using System.Text;

namespace CommandSystem
{
    public sealed class InputSystem
    {
        private StringBuilder builder;
        private int cursorOffset;
        private int cursor;

        private Dictionary<ConsoleKey, KeyEvent> keyEvents;

        public InputSystem()
        {
            builder = new StringBuilder();           
            keyEvents = new Dictionary<ConsoleKey, KeyEvent>();
        }

        public void AddDefaultEvent()
        {
            AddKeyEvent(new BackspaceHandler(this));
            AddKeyEvent(new DeleteHandler(this));
            AddKeyEvent(new LeftArrowHandler(this));
            AddKeyEvent(new RightArrowHandler(this));
        }

        public void AddKeyEvent(KeyEvent _event)
        {
            keyEvents.Add(_event.key, _event);
        }

        public void RemoveKeyEvent(KeyEvent _event)
        {
            keyEvents.Remove(_event.key);
        }

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
        
        public void CursorLeftMove()
        {
            if (Console.CursorLeft > cursorOffset)
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
        }

        public void CursorRightMove()
        {
            if (Console.CursorLeft - cursorOffset < builder.Length)
                Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
        }
        
        public string ReadLine(string title = "")
        {
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

        public static void ClearCurrentLine()
        {
            var currentLine = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLine);
        }

        public static void ClearCurrentLine(int startIndex)
        {
            var currentLine = Console.CursorTop;
            Console.SetCursorPosition(startIndex, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(startIndex, currentLine);
        }
    }

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
}
