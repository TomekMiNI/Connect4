using System;
using Connect4;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Connect4UnitTests
{
  [TestClass]
  public class AlphaBetaTests
  {
    [TestMethod]
    public void CheckSecondSituationTest()
    {
      Board board = new Board();
      bool? win;
      board.PutToken(true, 0, out win);
      board.PutToken(true, 1, out win);
      board.PutToken(true, 2, out win);

      AlphaBeta alphaBeta = new AlphaBeta();
      int result;
      alphaBeta.CheckSecondSituationHorizonatally(true, board, out result);

      Assert.AreEqual(900000, result);

      board.RemoveToken(0);
      board.PutToken(true, 3, out win);
      alphaBeta.CheckSecondSituationHorizonatally(true, board, out result);
      Assert.AreEqual(int.MaxValue, result);
    }
  }
}
