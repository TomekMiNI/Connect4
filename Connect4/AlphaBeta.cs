using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4
{
  public class AlphaBeta
  {
    public void MakeMove(bool firstPlayer, Board board)
    {
      int maxLevel = 5;
      int alpha = int.MinValue;
      int beta = int.MaxValue;
      var (score, move) = CalculateNextMove(firstPlayer, 1, maxLevel, alpha, beta, board);
      bool? win;
      board.PutToken(firstPlayer, move, out win);
    }

    //return (score, move)
    private (int, int) CalculateNextMove(bool firstPlayer, int level, int maxLevel, int alpha, int beta, Board board)
    {
      int bestResult = alpha;
      int bestMove = -1;
      for (int i = 0; i < board.NumberOfColumns; i++)
      {
        Board locBoard = board.Clone();
        bool? win = false;
        if (locBoard.PutToken(firstPlayer, i, out win))
        {
          if (win == true)
            bestResult = int.MaxValue;
          //draw or max depth
          else if (win == null || level == maxLevel)
          {
            var res = CalculateCurrentBoard(firstPlayer, locBoard);
            if (bestResult < res)
            {
              bestMove = i;
              bestResult = res;
            }
          }
          else
          {
            var (score, move) = CalculateNextMove(!firstPlayer, level + 1, maxLevel, alpha, beta, locBoard);
            if (bestResult < score)
            {
              bestMove = i;
              bestResult = score;
            }
          }
          //the biggest possible score
          if (bestResult == int.MaxValue)
            return (int.MaxValue, i);
        }
      }
      return (bestResult, bestMove);
    }

    private int CalculateCurrentBoard(bool firstPlayer, Board board)
    {
      //2. situation
      while (true)
      {
        int result = 0;
        bool res = CheckSecondSituation(firstPlayer, board, ref result);
      
      }

    }

    /// <summary>
    /// true if 900 000 or infinity
    /// </summary>
    /// <param name="firstPlayer"></param>
    /// <param name="currentBoard"></param>
    /// <param name="checkedBoard"></param>
    /// <returns></returns>
    public bool CheckSecondSituation(bool firstPlayer, Board currentBoard, ref int result)
    {
      if (CheckSecondSituationHorizonatally(firstPlayer, currentBoard, ref result))
        return true;
      else if (CheckSecondSituationVertically(firstPlayer, currentBoard, ref result))
        return true;
      else if (CheckSecondSituationDiagonally(firstPlayer, currentBoard, ref result))
        return true;
      return result == 900000;
    }


    public bool CheckSecondSituationVertically(bool firstPlayer, Board currentBoard, ref int result)
    {
      //if found twice, res = inf
      bool found = false;
      for(int c = 0; c < currentBoard.NumberOfColumns; c++)
      {
        int lastR = currentBoard.GetFirstFreeRow(c);
        if (lastR == currentBoard.NumberOfRows || lastR < 3) continue;
        for (int i = 1; i <= 3; i++)
          if (currentBoard[c, lastR - i] != firstPlayer)
            continue;
        //found twice!
        if(found)
        {
          result = int.MaxValue;
          return true;
        }
        found = true;
        result = 900000;
      }
      return false;
    }

    private bool CheckSecondSituationDiagonally(bool firstPlayer, Board currentBoard, ref int result)
    {
      throw new NotImplementedException();
    }

    //examples in tests
    /// <summary>
    /// true if found infinity
    /// </summary>
    /// <param name="firstPlayer"></param>
    /// <param name="currentBoard"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool CheckSecondSituationHorizonatally(bool firstPlayer, Board currentBoard, ref int result)
    {
      bool[] lacks = new bool[currentBoard.NumberOfColumns];
      for (int r = 0; r < currentBoard.NumberOfRows; r++)
      {
        List<int> starts = FindStartsOfTwoTokensInRowInRow(currentBoard, r, firstPlayer);
        foreach(var start in starts)
        {
          if (start + 3 < currentBoard.NumberOfColumns)
          {
            bool? lack = null;
            //OOO_
            if (currentBoard[start + 2, r] == firstPlayer && currentBoard[start + 3, r] == null)
              lack = lacks[start + 3];
            //OO_O
            else if (currentBoard[start + 3, r] == firstPlayer && currentBoard[start + 2, r] == null)
              lack = lacks[start + 2];

            if (lack != null)
            {
              if (start == 0 || lacks[start - 1] || currentBoard[start - 1, r] == !firstPlayer || lack == true)
                result = 900000;
              else
              {
                result = int.MaxValue;
                return true;
              }
            }

          }
          if(start - 2 >= 0)
          {
            bool lack = false;
            //_OOO
            if (currentBoard[start - 2, r] == null && currentBoard[start + 1, r] == firstPlayer)
              lack = lacks[start - 2];
            //O_OO
            else if (currentBoard[start - 1, r] == null && currentBoard[start - 2, r] == firstPlayer)
              lack = lacks[start - 1];
            else continue;

            if (start == currentBoard.NumberOfColumns - 2 || lacks[start + 2] || currentBoard[start + 2, r] == !firstPlayer || lack)
              result = 900000;
            else
            {
              result = int.MaxValue;
              return true;
            }
          }
        }
        for (int i = 0; i < currentBoard.NumberOfColumns; i++)
          if (currentBoard[i, r] == null)
            lacks[i] = true;
      }
      return false;
    }

    private List<int> FindStartsOfTwoTokensInRowInRow(Board currentBoard, int r, bool player)
    {
      List<int> starts = new List<int>();
      bool start = false;
      for(int i = 0; i < currentBoard.NumberOfColumns; i++)
      {
        if (currentBoard[i, r] == player)
        {
          //two in row
          if (start)
          {
            starts.Add(i - 1);
            start = false;
            //decrease i in case of OOO
            i--;
          }
          else start = true;
        }
        else if (start)
          start = false;
      }
      return starts;
    }
  }
}
