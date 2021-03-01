using System;

namespace FlipCard
{
    class Program
    {
        private const int defaultColumn = 4;
        private const int defaultRow = 4;
        private static int Column;
        private static int Row;
        private static FlipCardGame flipCard;
        private static InputSystem inputSystem;
        private static string errorMsg;

        static void Main(string[] args)
        {
            var data = CreateMenu1();
            CreateBoard(data);
            CreateMenu2();
            CreateInputSystem();
            flipCard.ReFreshBoard();
            inputSystem.Run();
        }

        private static string[] CreateMenu1()
        {
            System.Console.WriteLine("翻牌遊戲　作者：jack");
            System.Console.WriteLine("請輸入相乘為偶數的行列");
            System.Console.WriteLine("請按任一鍵繼續...");
            Console.ReadLine();
            System.Console.WriteLine("請輸入行數:");
            var column = Console.ReadLine();
            System.Console.WriteLine("請輸入列數:");
            var row = Console.ReadLine();
            return new string[] { column, row };
        }

        private static void CreateInputSystem()
        {
            inputSystem = new InputSystem(flipCard.Column, flipCard.Row);
            inputSystem.OnArrowInputEnd += flipCard.ReFreshBoard;
            inputSystem.OnGameEnd += OnGameEnd;
            inputSystem.OnSpaceBarPressed += CheckNumber;
        }

        private static void CreateBoard(string[] args)
        {

            try
            {
                if (int.TryParse(args[0], out var column) && int.TryParse(args[1], out var row))
                {
                    if(column*row%2!=0){
                        throw new Exception();
                    }

                    System.Console.WriteLine($"建立({column}*{row})遊戲");
                    Column = column;
                    Row = row;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (System.Exception ex)
            {
                errorMsg= ex.Message;
                Console.Clear();
                Console.WriteLine($"參數錯誤，使用預設 : ({defaultColumn}*{defaultRow})");
                Console.ReadLine();
                Column = defaultColumn;
                Row = defaultRow;
            }
            finally
            {
                flipCard = new FlipCardGame(Column, Row);
            }
            flipCard.OnRefreshEnd += OnBoardRefreshEnd;
        }
        private static void CreateMenu2()
        {
            Console.Clear();
            System.Console.WriteLine("使用方向鍵控制");
            System.Console.WriteLine("點擊Q離開");
            System.Console.WriteLine("點擊空白鍵選擇");
            System.Console.WriteLine("請按任一鍵繼續...");
            Console.ReadLine();

        }

        private static void OnBoardRefreshEnd()
        {
            Console.SetCursorPosition(inputSystem.GetCursorLeft(), inputSystem.GetCursorTop());
        }

        private static void OnGameEnd()
        {
            Console.Clear();
            System.Console.WriteLine("\n遊戲結束，感謝遊玩");
            Console.ReadKey();
        }

        private static void CheckNumber()
        {
            var column = inputSystem.Pointer.X;
            var row = inputSystem.Pointer.Y;
            var number = flipCard.GetIndexNumber(column, row);
            //System.Console.WriteLine($"選擇數字:{number}");
            flipCard.FlipCard(column,row,number);
        }
    }
}
