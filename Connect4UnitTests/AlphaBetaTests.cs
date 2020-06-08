using System;
using Connect4;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Connect4UnitTests
{
  [TestClass]
  public class AlphaBetaTests
  {
    [TestMethod]
    public void CheckSecondSituationHorizontallyTest()
    {
      Board board = new Board();
      bool? win;
      AlphaBeta alphaBeta = new AlphaBeta();
      int result = 0;

      //OOO_
      board.PutToken(true, 0, out win);
      board.PutToken(true, 1, out win);
      board.PutToken(true, 2, out win);
      alphaBeta.CheckSecondSituationHorizonatally(true, board, ref result);
      Assert.AreEqual(900000, result);

      //_OO_O
      board.RemoveToken(0);
      board.PutToken(true, 4, out win);
      alphaBeta.CheckSecondSituationHorizonatally(true, board, ref result);
      Assert.AreEqual(int.MaxValue, result);

      //_OOOX
      board.RemoveToken(4);
      board.PutToken(false, 4, out win);
      board.PutToken(true, 3, out win);
      alphaBeta.CheckSecondSituationHorizonatally(true, board, ref result);
      Assert.AreEqual(900000, result);

      //_OOO_
      board.RemoveToken(4);
      alphaBeta.CheckSecondSituationHorizonatally(true, board, ref result);
      Assert.AreEqual(int.MaxValue, result);

      //_OOO___
      //_XOXOXO
      board = new Board();
      board.PutToken(true, 2, out win);
      board.PutToken(true, 4, out win);
      board.PutToken(true, 6, out win);
      board.PutToken(false, 1, out win);
      board.PutToken(false, 3, out win);
      board.PutToken(false, 5, out win);
      board.PutToken(true, 1, out win);
      board.PutToken(true, 2, out win);
      board.PutToken(true, 3, out win);
      alphaBeta.CheckSecondSituationHorizonatally(true, board, ref result);
      Assert.AreEqual(900000, result);

      //_OOO___
      //XXOXOXO
      board.PutToken(false, 0, out win);
      alphaBeta.CheckSecondSituationHorizonatally(true, board, ref result);
      Assert.AreEqual(int.MaxValue, result);


      //XOOO___
      //XXOXOXO
      board.PutToken(false, 0, out win);
      alphaBeta.CheckSecondSituationHorizonatally(true, board, ref result);
      Assert.AreEqual(900000, result);

      //_XO_OO_
      //_OOOXXX
      board = new Board();
      board.PutToken(true, 1, out win);
      board.PutToken(true, 2, out win);
      board.PutToken(true, 3, out win);
      board.PutToken(false, 4, out win);
      board.PutToken(false, 5, out win);
      board.PutToken(false, 6, out win);
      board.PutToken(false, 1, out win);
      board.PutToken(true, 2, out win);
      board.PutToken(true, 4, out win);
      board.PutToken(true, 5, out win);
      alphaBeta.CheckSecondSituationHorizonatally(true, board, ref result);
      Assert.AreEqual(int.MaxValue, result);

      //OOO____
      //XXX_OOO
      board = new Board();
      board.PutToken(false, 0, out win);
      board.PutToken(false, 1, out win);
      board.PutToken(false, 2, out win);
      board.PutToken(true, 0, out win);
      board.PutToken(true, 1, out win);
      board.PutToken(true, 2, out win);
      board.PutToken(true, 4, out win);
      board.PutToken(true, 5, out win);
      board.PutToken(true, 6, out win);
      alphaBeta.CheckSecondSituationHorizonatally(true, board, ref result);
      Assert.AreEqual(900000, result);

      //GENERALLY UPPER EXAMPLE SHOULD RETURN INFINITY, BUT WE DONT CHECK IT...
      //ITS LIKE TWO VERY GOOD SITUATIONS. WE CHECK JUST ONE.
      //IMAGINE OTHER SITUATION:
      //_OOOXOX
      //_OOOXOX
      //THIS EXAMPLE ALSO SHOWS TWO VERY GOOD SITUATIONS BUT IT SHOULD NOT RETURN INF
      //SHOULD WE CHECK THIS SPECIAL ONE? ITS ADDITIONAL IFS... TOO MANY...

    }
    [TestMethod]
    public void CheckSecondSituationVerticallyTest()
    {
      Board board = new Board();
      bool? win;
      AlphaBeta alphaBeta = new AlphaBeta();
      int result = 0;

      board.PutToken(true, 1, out win);
      board.PutToken(true, 1, out win);
      board.PutToken(true, 1, out win);
      alphaBeta.CheckSecondSituationVertically(true, board, ref result);
      Assert.AreEqual(900000, result);


      board.PutToken(true, 3, out win);
      board.PutToken(true, 3, out win);
      board.PutToken(true, 3, out win);
      alphaBeta.CheckSecondSituationVertically(true, board, ref result);
      Assert.AreEqual(int.MaxValue, result);
    }

    [TestMethod]
    public void CheckSecondSituationDiagonallyTest()
    {
      Board board = new Board();
      bool? win;
      AlphaBeta alphaBeta = new AlphaBeta();
      int result;

      //__XOX__
      //__OXO__
      //_OXXXO_
      //should be inf
    }
  }
}
