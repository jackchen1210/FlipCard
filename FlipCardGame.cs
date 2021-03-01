using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class FlipCardGame
{
    public event Action OnRefreshEnd;
    public int Column { get; }
    public int Row { get; }
    private int[,] boardIndex;
    private List<int> randomNums;
    private int numberToCheck = 0;
    private string[,] displayBoard;
    private string[,] prevBoard;
    private bool[,] isAnswered;
    private (int x, int y) prevCoord;
    private (int x, int y) selectCoord;

    public FlipCardGame(int column, int row)
    {
        Column = column;
        Row = row;
        boardIndex = new int[column, row];
        isAnswered = new bool[column, row];
        displayBoard = CreateInitBoard(column, row);
        prevBoard = CreateInitBoard(column, row);
        SetBoardIndex();
        CreateBoardNumber();
    }

    private string[,] CreateInitBoard(int column, int row)
    {
        var output = new string[column, row];
        BoardLooper((_column, _row) =>
        {
            output[_column, _row] = "*";
        });
        return output;
    }

    private void SetBoardIndex()
    {
        int index = 0;
        BoardLooper((column, row) =>
        {
            boardIndex[column, row] = index++;
        });
    }

    private void BoardLooper(Action<int, int> action, Action columnAction = null)
    {

        for (var i = 0; i < Column; i++)
        {
            for (var j = 0; j < Row; j++)
            {
                action?.Invoke(i, j);
            }
            columnAction?.Invoke();
        }
    }

    private void CreateBoardNumber()
    {
        var rng = new Random();
        var total = Column * Row;
        randomNums = new List<int>();
        var num = 1;
        var counter = 0;
        BoardLooper((column, row) =>
        {
            randomNums.Add(num);
            counter++;
            if (counter == 2)
            {
                counter = 0;
                num++;
            }
        });
        randomNums = randomNums.OrderBy(_ => rng.Next()).ToList();
    }
    public void ReFreshBoard()
    {
        Console.Clear();
        Print();
        OnRefreshEnd?.Invoke();
    }

    private void Print()
    {
        System.Console.WriteLine($"({Column}*{Row})遊戲:");
        BoardLooper((column, row) =>
        {
            System.Console.Write(displayBoard[column, row]);
            System.Console.Write("   ");
        },
        () =>
        {
            Console.Write("\n");
        }
        );
    }

    public int GetIndexNumber(int column, int row)
    {
        try
        {
            return boardIndex[column, row];
        }
        catch (System.Exception ex)
        {
            Console.Write("Error :" + ex.Message);
            return 0;
        }
    }

    public void FlipCard(int column, int row, int indexNumber)
    {
        if(isAnswered[column,row]){
            return;
        }

        var selectNum = randomNums[indexNumber];
        displayBoard[column, row] = selectNum.ToString();
        ReFreshBoard();
        if (numberToCheck == 0)
        {
            numberToCheck = selectNum;
            prevCoord = (column, row);
        }
        else
        {
            selectCoord = (column, row);
            Task.Delay(500).ContinueWith((_) =>
            {
                CompareTwoNum(selectNum);
            });
        }
    }

    private void CompareTwoNum(int selectNum)
    {
        var num = selectNum;
        if (numberToCheck == num)
        {
            SaveBoard();
            SetAnswered();
        }
        else
        {
            ResetToPrevBoard();
        }
        numberToCheck = 0;
        ReFreshBoard();
    }

    private void SetAnswered()
    {
        isAnswered[prevCoord.x, prevCoord.y] = true;
        isAnswered[selectCoord.x, selectCoord.y] = true;
    }

    private void SaveBoard()
    {
        BoardLooper((c, r) =>
        {
            prevBoard[c, r] = displayBoard[c, r];
        });
    }

    private void ResetToPrevBoard()
    {
        prevCoord.x = 0;
        prevCoord.y = 0;
        selectCoord.x = 0;
        selectCoord.y = 0;

        BoardLooper((c, r) =>
        {
            displayBoard[c, r] = prevBoard[c, r];
        });
    }
}