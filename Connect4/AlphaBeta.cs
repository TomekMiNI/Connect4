using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4
{
  public class AlphaBeta
  {
    public bool? MakeMove(bool firstPlayer, Board board)
    {
      int maxLevel = 5;
      int alpha = int.MinValue;
      int beta = int.MaxValue;
      var move = CalculateNextMove(firstPlayer, firstPlayer, 1, maxLevel, ref alpha, ref beta, board);
      bool? win;
      board.PutToken(firstPlayer, move, out win);
      return win;
    }

    //return move
    private int CalculateNextMove(bool firstPlayer, bool turnPlayer, int level, int maxLevel, ref int alpha, ref int beta, Board board)
    {
      int bestResult = alpha;
      int bestMove = -1;
      for (int i = 0; i < board.NumberOfColumns; i++)
      {
        Board locBoard = board.Clone();
        bool? win = false;
        if (locBoard.PutToken(firstPlayer, i, out win))
        {
          //time to go back in tree
          if (win != false || level == maxLevel)
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
            //PRUNNING
            //maximize level of tree (alpha)
            if (turnPlayer == firstPlayer)
            {
              if (alpha < bestResult)
              {
                alpha = bestResult;
                if (alpha > beta)
                  return i;
              }
            }
            //minimizing level of tree (beta)
            else
            {
              if (beta > -bestResult)
              {
                beta = -bestResult;
                if (beta < alpha)
                  return i;
              }
            }
          }
          //go deeper
          else
          {
            //increase in case of next starting (turn) player
            if (firstPlayer != turnPlayer) level++;
            var move = CalculateNextMove(!firstPlayer, turnPlayer, level, maxLevel, ref alpha, ref beta, locBoard);
           }
          
        }
      }
      return bestMove;
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
      return result >= 900000;
    }


    public bool CheckSecondSituationVertically(bool firstPlayer, Board currentBoard, ref int result)
    {
      //if found twice, res = inf
      bool found = false;
      for(int c = 0; c < currentBoard.NumberOfColumns; c++)
      {
        bool cont = false;
        int lastR = currentBoard.GetFirstFreeRow(c);
        if (lastR == currentBoard.NumberOfRows || lastR < 3) continue;
        for (int i = 1; i <= 3; i++)
          if (currentBoard[c, lastR - i] != firstPlayer)
          {
            cont = true;
            break;
          }
        if (cont) continue;
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
      //seperatelly check 3 in row in
      //X
      //XX
      //XXX
      if (CheckSecondSituationDiagonallyRightTop(firstPlayer, currentBoard, ref result))
        return true;
      if (CheckSecondSituationDiagonallyLeftTop(firstPlayer, currentBoard, ref result))
        return true;
      return false;
    }

    public bool CheckSecondSituationDiagonallyRightTop(bool firstPlayer, Board currentBoard, ref int result)
    {
      //starts: r=2,1,0
      //r=0,c=1
      //r=0,c=2
      //r=0,c=3
      bool[] lacks = new bool[currentBoard.NumberOfColumns];
      for(int rr = 0; rr <=2; rr++)
        for(int cc = 4 + rr; cc<=6; cc++)
          if (currentBoard[cc, rr] == null)
            lacks[cc] = true;

      int c = 3;
      int r = 0;
      //start from bottom to fill lacks
      while(r <= 2)
      {
        List<(int col, int row)> starts = FindStartsOfTwoTokensInRowInDiagonalRightTop(firstPlayer, currentBoard, c, r);
        foreach (var start in starts)
        {
          //right top
          if (start.col + 3 < currentBoard.NumberOfColumns && start.row + 3 < currentBoard.NumberOfRows)
          {
            bool? lack = null;
            //OOO_ - chance for inf
            if (currentBoard[start.col + 2, start.row + 2] == firstPlayer && currentBoard[start.col + 3, start.row + 3] == null)
            {
              lack = lacks[start.col + 3];
              if (start.col == 0 || lacks[start.col - 1] || start.row == 0 || currentBoard[start.col - 1, start.row - 1] == !firstPlayer || lack == true)
                result = 900000;
              else
              {
                result = int.MaxValue;
                return true;
              }
            }
            //no chance for inf
            if (result < 900000)
            {
              //OO_O 
              if (currentBoard[start.col + 3, start.row + 3] == firstPlayer && currentBoard[start.col + 2, start.row + 2] == null)
                result = 900000;
              //OOOX
              else if (currentBoard[start.col + 2, start.row + 2] == firstPlayer && (start.col + 3 == currentBoard.NumberOfColumns || start.row + 3 == currentBoard.NumberOfRows || currentBoard[start.col + 3, start.row + 3] == !firstPlayer))
                result = 50000;
            }

          }
          if (start.col - 2 >= 0 && start.row - 2 >= 0)
          {
            bool lack = false;
            //_OOO - chance for inf
            if (currentBoard[start.col - 2, start.row - 2] == null && currentBoard[start.col - 1, start.row - 1] == firstPlayer)
            {
              lack = lacks[start.col - 2];
              if (start.col == currentBoard.NumberOfColumns - 2 || lacks[start.col + 2] || start.row == currentBoard.NumberOfRows - 2 || currentBoard[start.col + 2, start.row + 2] == !firstPlayer || lack)
                result = 900000;
              else
              {
                result = int.MaxValue;
                return true;
              }
            }
            //no chance for inf
            if (result < 900000)
            {
              //O_OO 
              if (currentBoard[start.col - 1, start.row - 1] == null && currentBoard[start.col - 2, start.row - 2] == firstPlayer)
                result = 900000;
              //XOOO
              else if (currentBoard[start.col - 1, start.row - 1] == firstPlayer && (start.col - 1 == 0 || start.row == 0 || currentBoard[start.col - 2, start.row - 2] == !firstPlayer))
                result = 50000;
            }

          }
        }

        int cc = c;
        int rr = r;
        while (cc < currentBoard.NumberOfColumns && rr < currentBoard.NumberOfRows)
        {
          if (currentBoard[cc, rr] == null)
            lacks[cc] = true;
          cc++;
          rr++;
        }

        if (c == 0) r++;
        else c--;
      }
      return false;
    }
    public bool CheckSecondSituationDiagonallyLeftTop(bool firstPlayer, Board currentBoard, ref int result)
    {
      //starts: r=0; c=3,4,5,6
      //r=1,c=6
      //r=2,c=6
      //r=3,c=6
      bool[] lacks = new bool[currentBoard.NumberOfColumns];
      for (int rr = 0; rr <= 2; rr++)
        for (int cc = 0; cc <= 2 - rr; cc++)
          if (currentBoard[cc, rr] == null)
            lacks[cc] = true;

      int c = 3;
      int r = 0;
      //start from bottom to fill lacks
      while (r <= 2)
      {
        List<(int col, int row)> starts = FindStartsOfTwoTokensInRowInDiagonalLeftTop(firstPlayer, currentBoard, c, r);
        foreach (var start in starts)
        {
          //left top
          if (start.col - 3 >= 0 && start.row + 3 < currentBoard.NumberOfRows)
          {
            //_OOO - according to direction
            if (currentBoard[start.col - 2, start.row + 2] == firstPlayer && currentBoard[start.col - 3, start.row + 3] == null)
            {
              bool lack = lacks[start.col - 3];
              if (start.col == currentBoard.NumberOfColumns - 1 || lacks[start.col + 1] || start.row == 0 || currentBoard[start.col + 1, start.row - 1] == !firstPlayer || lack == true)
                result = 900000;
              else
              {
                result = int.MaxValue;
                return true;
              }
            }
            //no chance for inf
            if (result < 900000)
            {
              //O_OO
              if (currentBoard[start.col - 3, start.row + 3] == firstPlayer && currentBoard[start.col - 2, start.row + 2] == null)
                result = 900000;
              //OOOX
              else if (currentBoard[start.col - 2, start.row + 2] == firstPlayer && (start.col - 2 == 0 || start.row + 3 == currentBoard.NumberOfRows || currentBoard[start.col - 3, start.row + 3] == !firstPlayer))
                result = 50000;
            }

          }
          //right bottom
          if (start.col + 2 < currentBoard.NumberOfColumns && start.row - 2 >= 0)
          {
            //O[O]O_
            if (currentBoard[start.col + 2, start.row - 2] == null && currentBoard[start.col + 1, start.row - 1] == firstPlayer)
            {
              bool lack = lacks[start.col + 2];
              if (start.col == 1 || lacks[start.col - 2] || start.row == currentBoard.NumberOfRows - 2 || currentBoard[start.col - 2, start.row + 2] == !firstPlayer || lack)
                result = 900000;
              else
              {
                result = int.MaxValue;
                return true;
              }
            }
            if (result < 900000)
            {
              //O[O]_O
              if (currentBoard[start.col + 1, start.row - 1] == null && currentBoard[start.col + 2, start.row - 2] == firstPlayer)
                result = 900000;
              //XO[O]O
              else if (currentBoard[start.col + 1, start.row - 1] == firstPlayer && (start.col == 1 || start.row == currentBoard.NumberOfRows - 2 || currentBoard[start.col - 2, start.row + 2] == !firstPlayer))
                result = 50000;
            }

          }
        }

        int cc = c;
        int rr = r;
        while (cc >= 0 && rr < currentBoard.NumberOfRows)
        {
          if (currentBoard[cc, rr] == null)
            lacks[cc] = true;
          cc--;
          rr++;
        }

        if (c == 6) r++;
        else c++;
      }
      return false;
    }
    private List<(int, int)> FindStartsOfTwoTokensInRowInDiagonalRightTop(bool firstPlayer, Board currentBoard, int c, int r)
    {
      bool found = false;
      List<(int, int)> starts = new List<(int, int)>();
      while(c < currentBoard.NumberOfColumns && r < currentBoard.NumberOfRows)
      {
        if (currentBoard[c, r] == firstPlayer)
        {
          if (found)
          {
            starts.Add((c - 1, r - 1));
            found = false;
            c--;
            r--;
          }
          else
            found = true;
        }
        else if (found) found = false;
        c++;
        r++;
      }
      return starts;
    }
    private List<(int, int)> FindStartsOfTwoTokensInRowInDiagonalLeftTop(bool firstPlayer, Board currentBoard, int c, int r)
    {
      bool found = false;
      List<(int, int)> starts = new List<(int, int)>();
      while (c >= 0 && r < currentBoard.NumberOfRows)
      {
        if (currentBoard[c, r] == firstPlayer)
        {
          if (found)
          {
            starts.Add((c + 1, r - 1));
            found = false;
            c++;
            r--;
          }
          else
            found = true;
        }
        c--;
        r++;
      }
      return starts;
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
        foreach (var start in starts)
        {
          //right
          if (start + 3 < currentBoard.NumberOfColumns)
          {
            //OOO_ - chance for inf
            if (currentBoard[start + 2, r] == firstPlayer && currentBoard[start + 3, r] == null)
            {
              bool lack = lacks[start + 3];
              if (start == 0 || lacks[start - 1] || currentBoard[start - 1, r] == !firstPlayer || lack == true)
                result = 900000;
              else
              {
                result = int.MaxValue;
                return true;
              }
            }
            //no chance for inf
            else if (result < 900000)
            {
              //OO_O - 
              if (currentBoard[start + 3, r] == firstPlayer && currentBoard[start + 2, r] == null)
                result = 900000;
              //OOOX
              else if (currentBoard[start + 2, r] == firstPlayer && (start + 3 == currentBoard.NumberOfColumns || currentBoard[start + 3, r] == !firstPlayer))
                result = 50000;
            }
          }
          //left
          if (start - 2 >= 0)
          {
            //_OOO - chance for inf
            if (currentBoard[start - 2, r] == null && currentBoard[start - 1, r] == firstPlayer)
            {
              bool lack = lacks[start - 2];
              if (start == currentBoard.NumberOfColumns - 2 || lacks[start + 2] || currentBoard[start + 2, r] == !firstPlayer || lack)
                result = 900000;
              else
              {
                result = int.MaxValue;
                return true;
              }
            }
            //no chance for inf
            else if (result < 900000)
            {
              //O_OO 
              if (currentBoard[start - 1, r] == null && currentBoard[start - 2, r] == firstPlayer)
                result = 900000;
              //XOOO
              else if (currentBoard[start - 1, r] == firstPlayer && (start - 1 == 0 || currentBoard[start - 2, r] == !firstPlayer))
              {
                result = 50000;
              }
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
