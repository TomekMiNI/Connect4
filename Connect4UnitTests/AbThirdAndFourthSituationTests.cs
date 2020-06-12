using Connect4;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4UnitTests
{
  [TestClass]
  public class AbThirdAndFourthSituationTests
  {
    [TestMethod]
    public void CheckThirdSituationHorizontallyTest()
    {
      Board board = new Board();
      bool? win;
      AlphaBeta alphaBeta = new AlphaBeta();
      int result = 0;

      //XX_____
      //XX_____
      board.PutToken(true, 0, out win);
      board.PutToken(true, 0, out win);
      board.PutToken(true, 1, out win);
      board.PutToken(true, 1, out win);
      alphaBeta.CheckThirdSituationHorizontally(true, board, ref result);
      Assert.AreEqual(result, 80000);

    }
    [TestMethod]
    public void CheckThirdSituationDiagonallyTest()
    {
      Board board = new Board();
      bool? win;
      AlphaBeta alphaBeta = new AlphaBeta();
      int result = 0;

      //XX_____
      //XX_____
      board.PutToken(true, 0, out win);
      board.PutToken(true, 0, out win);
      board.PutToken(true, 1, out win);
      board.PutToken(true, 1, out win);
      alphaBeta.CheckThirdSituationDiagonally(true, board, ref result);
      Assert.AreEqual(result, 30000);

      //XX___XX
      //XX___XX
      board.PutToken(true, 6, out win);
      board.PutToken(true, 6, out win);
      board.PutToken(true, 5, out win);
      board.PutToken(true, 5, out win);
      result = 0;
      alphaBeta.CheckThirdSituationDiagonally(true, board, ref result);
      Assert.AreEqual(result, 60000);

    }
    [TestMethod]
    public void CheckThirdSituationVerticallyTest()
    {
      Board board = new Board();
      bool? win;
      AlphaBeta alphaBeta = new AlphaBeta();
      int result = 0;

      //XX_____
      //XX_____
      board.PutToken(true, 0, out win);
      board.PutToken(true, 0, out win);
      board.PutToken(true, 1, out win);
      board.PutToken(true, 1, out win);
      alphaBeta.CheckThirdSituationVertically(true, board, ref result);
      Assert.AreEqual(result, 60000);

    }
    [TestMethod]
    public void CheckThirdSituationTest()
    {
      Board board = new Board();
      bool? win;
      AlphaBeta alphaBeta = new AlphaBeta();
      int result = 0;

      //XX_____
      //XX_____
      board.PutToken(true, 0, out win);
      board.PutToken(true, 0, out win);
      board.PutToken(true, 1, out win);
      board.PutToken(true, 1, out win);
      alphaBeta.CheckThirdSituation(true, board, ref result);
      Assert.AreEqual(result, 170000);

    }
    [TestMethod]
    public void CheckFourthSituationTest()
    {
      Board board = new Board();
      bool? win;
      AlphaBeta alphaBeta = new AlphaBeta();
      int result = 0;

      //_______
      //X_X_X_X
      board.PutToken(true, 0, out win);
      board.PutToken(true, 2, out win);
      board.PutToken(true, 4, out win);
      board.PutToken(true, 6, out win);
      alphaBeta.CheckFourthSituation(true, board, ref result);
      Assert.AreEqual(result, 320);

    }

    [TestMethod]
    public void CalculateCurrentBoardTest()
    {
      Board board = new Board();
      bool? win;
      AlphaBeta alphaBeta = new AlphaBeta();
      int result = 0;

      //_______
      //X_X_X_X
      board.PutToken(true, 0, out win);
      board.PutToken(true, 2, out win);
      board.PutToken(true, 4, out win);
      board.PutToken(true, 6, out win);
      alphaBeta.CalculateCurrentBoard(true, board);
    }
    [TestMethod]
    public void MakeMoveTest()
    {
      Board board = new Board();
      bool? win;
      AlphaBeta alphaBeta = new AlphaBeta();
      int result = 0;

      //_______
      //X_X_X_X
      //board.PutToken(true, 0, out win);
      //board.PutToken(true, 2, out win);
      //board.PutToken(true, 4, out win);
      //board.PutToken(true, 6, out win);
      //alphaBeta.MakeMove(false, board);

      //O_____
      //O_____
      //O___XX
      board = new Board();
      board.PutToken(false, 1, out win);
      board.PutToken(false, 1, out win);
      board.PutToken(false, 1, out win);
      board.PutToken(true, 6, out win);
      board.PutToken(true, 5, out win);
      alphaBeta.MakeMove(true, true, board);
    }
  }
}
