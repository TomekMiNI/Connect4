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
        bool[][] checkedBoard = new bool[board.NumberOfColumns][];
        for (int i = 0; i < board.NumberOfColumns; i++)
          checkedBoard[i] = new bool[board.NumberOfRows];

        bool res = CheckSecondSituation(firstPlayer, board, checkedBoard);
      
      }

    }

    public bool CheckSecondSituation(bool firstPlayer, Board currentBoard, bool[][] checkedBoard)
    {
      int result;
      if (CheckSecondSituationHorizonatally(firstPlayer, currentBoard, out result))
        return true;
      //if (CheckSecondSituationVertically(firstPlayer, currentBoard, checkedBoard))
      //  return true;
      if (CheckSecondSituationDiagonally(firstPlayer, currentBoard, checkedBoard))
        return true;
      return false;
    }

    private bool CheckSecondSituationDiagonally(bool firstPlayer, Board currentBoard, bool[][] checkedBoard)
    {
      throw new NotImplementedException();
    }


    //_OOO_ returns infinity

    //_OOO_
    //_XOXX returns 900 000 - one opponent's mistake

    //OO__OX returns 50 000 - two opponent's mistake

    //_OOOX returns 900 000 - one opponent's mistake

    //_OOOX
    //_XOXO returns 900 000 - one opponent's mistake

    //OOOX returns 0
    public bool CheckSecondSituationHorizonatally(bool firstPlayer, Board currentBoard, out int result)
    {
      bool[] lacks = new bool[currentBoard.NumberOfColumns];
      result = 0;
      for (int r = 0; r < currentBoard.NumberOfRows; r++)
      {
        int startCol = 0;
        while (true)
        {
          int col = 0;
          int times = 0;
          while (col + startCol < currentBoard.NumberOfColumns && currentBoard[startCol + col, r] != !firstPlayer)
            if(currentBoard[col++, r] == firstPlayer) times++;
          if (startCol + col < 4)
          {
            //there is still chance to find sequence
            startCol += col + 1;
            continue;
          }
          
          if (col >= 4 && times >= 3)
          { 
            //5,6,7 in row
            if (col >= 5)
            {
              if (col == 5)
              {
                if (lacks.Skip(startCol).Contains(true))
                {
                  //_OOO_
                  //_XOXX returns 900 000
                  result = 900000;
                }
                else
                {
                  //_OOO_ returns infinity
                  result = int.MaxValue;
                }
                return true;
              }
              else
              {
                int firstToken = startCol;
                while (currentBoard[firstToken, r] != firstPlayer)
                  firstToken++;

                //OOO
                 if (currentBoard[firstToken + 1, r] == firstPlayer && currentBoard[firstToken + 2, r] == firstPlayer)
                {
                  //without break, so if its on the edge it has just one opportunity to win
                  if (firstToken == 0 || firstToken == 4)
                  {
                    result = 900000;
                  }
                  //there are lacks
                  else if (lacks[firstToken - 1] || lacks[firstToken + 3])
                  {
                    result = 900000;
                  }
                  else
                  {
                    result = int.MaxValue;
                  }
                  return true;
                }
                //O_OO
                else if (currentBoard[firstToken + 1, r] == null && currentBoard[firstToken + 2, r] == firstPlayer && currentBoard[firstToken + 3, r] == firstPlayer) 
                {
                  int numOfLacks = 0;
                  if (firstToken > 0 && lacks[firstToken - 1]) numOfLacks++;
                  if (lacks[firstToken + 1]) numOfLacks++;
                  if (firstToken + 4 < currentBoard.NumberOfColumns && lacks[firstToken + 4]) numOfLacks++;
                  if (numOfLacks <= 2)
                  {
                    if (numOfLacks <= 1)
                      result = int.MaxValue;
                    else if (numOfLacks == 2)
                      result = 900000;
                    return true;
                  }
                }
                //OO_O
                else if (currentBoard[firstToken + 1, r] == firstPlayer && currentBoard[firstToken + 2, r] == null && currentBoard[firstToken + 3, r] == firstPlayer)
                {
                  int numOfLacks = 0;
                  if (firstToken > 0 && lacks[firstToken - 1]) numOfLacks++;
                  if (lacks[firstToken + 2]) numOfLacks++;
                  if (firstToken + 4 < currentBoard.NumberOfColumns && lacks[firstToken + 4]) numOfLacks++;
                  if (numOfLacks <= 2)
                  {
                    if (numOfLacks <= 1)
                      result = int.MaxValue;
                    else if (numOfLacks == 2)
                      result = 900000;
                    return true;
                  }
                }
              }

            }
            //4 in row
            else
            {
              result = 900000;
              return true;
            }
          }
          break;
        }
        //fill lacks
        for (int i = 0; i < currentBoard.NumberOfColumns; i++)
          if (currentBoard[i, r] == null)
            lacks[i] = true;
      }
      result = 0;
      return false;
    }
    
  }
}
