using System.Numerics;
using System;
using System.Threading.Tasks;

namespace FlipCard
{
    public class InputSystem
    {
        private const int cursorTopOffset = 1;
        private const int cursorRowUnit = 4;
        public event Action OnGameEnd;
        public event Action OnArrowInputEnd;
        public event Action OnSpaceBarPressed;
        private ConsoleKeyInfo keyCode;
        private int cursorLeft;
        private int cursorTop = 1;
        private int column;
        private int row;
        public (int X, int Y) Pointer;

        public InputSystem(int column, int row)
        {
            this.column = column;
            this.row = row;
        }
        public void Run()
        {
            while (CheckIsEnd() == false)
            {
                keyCode = Console.ReadKey(true);
                //System.Console.WriteLine($"{keyCode.Key}");
                CheckArrowMovement();
            }
        }

        private void CheckArrowMovement()
        {
            switch (keyCode.Key)
            {
                case ConsoleKey.UpArrow:
                    MoveTop();
                    OnArrowInputEnd?.Invoke();
                    break;
                case ConsoleKey.DownArrow:
                    MoveDown();
                    OnArrowInputEnd?.Invoke();
                    break;
                case ConsoleKey.LeftArrow:
                    MoveLeft();
                    OnArrowInputEnd?.Invoke();
                    break;
                case ConsoleKey.RightArrow:
                    MoveRight();
                    OnArrowInputEnd?.Invoke();
                    break;
                case ConsoleKey.Spacebar:
                    OnSpaceBarPressed?.Invoke();
                    break;
            }
        }
        private void MoveTop()
        {
            cursorTop--;
            if (cursorTop < cursorTopOffset)
            {
                cursorTop = cursorTopOffset;
            }
            Pointer.X--;
            if (Pointer.X < 0)
            {
                Pointer.X = 0;
            }
        }

        private void MoveDown()
        {
            cursorTop++;
            var bound = (column - 1) + cursorTopOffset;
            if (cursorTop > bound)
            {
                cursorTop = bound;
                Pointer.X = bound;
            }
            Pointer.X++;
            if (Pointer.X > (column - 1))
            {
                Pointer.X = column - 1;
            }
        }

        private void MoveLeft()
        {
            cursorLeft -= cursorRowUnit;
            if (cursorLeft < 0)
            {
                cursorLeft = 0;
            }

            Pointer.Y--;
            if (Pointer.Y < 0)
            {
                Pointer.Y = 0;
            }
        }

        private void MoveRight()
        {
            cursorLeft += cursorRowUnit;
            var bound = (row - 1) * cursorRowUnit;
            if (cursorLeft >= bound)
            {
                cursorLeft = bound;
            }

            Pointer.Y++;
            if (Pointer.Y > row - 1)
            {
                Pointer.Y = row - 1;
            }
        }

        public int GetCursorTop()
        {
            return cursorTop;
        }

        public int GetCursorLeft()
        {
            return cursorLeft;
        }

        private bool CheckIsEnd()
        {
            var isEnd = keyCode.Key == ConsoleKey.Q;
            if (isEnd)
            {
                OnGameEnd?.Invoke();
            }
            return isEnd;
        }
    }
}