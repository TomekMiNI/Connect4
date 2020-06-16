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
      board.PutToken(0);
      board.PutToken(0);
      board.PutToken(1);
      board.PutToken(1);
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
      board.PutToken(0);
      board.PutToken(0);
      board.PutToken(1);
      board.PutToken(1);
      alphaBeta.CheckThirdSituationDiagonally(true, board, ref result);
      Assert.AreEqual(result, 30000);

      //XX___XX
      //XX___XX
      board.PutToken(6);
      board.PutToken(6);
      board.PutToken(5);
      board.PutToken(5);
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
      board.PutToken(0);
      board.PutToken(0);
      board.PutToken(1);
      board.PutToken(1);
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
      board.PutToken(0);
      board.PutToken(0);
      board.PutToken(1);
      board.PutToken(1);
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
      board.PutToken(0);
      board.PutToken(2);
      board.PutToken(4);
      board.PutToken(6);
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
      board.PutToken(0);
      board.PutToken(2);
      board.PutToken(4);
      board.PutToken(6);
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
      //board.PutToken(0);
      //board.PutToken(2);
      //board.PutToken(4);
      //board.PutToken(6);
      //alphaBeta.MakeMove(false, board);

      //O_____
      //O_____
      //O___XX
      board = new Board();
      board.PutToken(1);
      board.PutToken(1);
      board.PutToken(1);
      board.PutToken(6);
      board.PutToken(5);
      alphaBeta.MakeMove(true, true, board);
    }
  }
}
