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

      result = 0;
      //_OO_O
      board.RemoveToken(0);
      board.PutToken(true, 4, out win);
      alphaBeta.CheckSecondSituationHorizonatally(true, board, ref result);
      Assert.AreEqual(900000, result);

      result = 0;
      //_OOOX
      board.RemoveToken(4);
      board.PutToken(false, 4, out win);
      board.PutToken(true, 3, out win);
      alphaBeta.CheckSecondSituationHorizonatally(true, board, ref result);
      Assert.AreEqual(900000, result);

      result = 0;
      //_OOO_
      board.RemoveToken(4);
      alphaBeta.CheckSecondSituationHorizonatally(true, board, ref result);
      Assert.AreEqual(int.MaxValue, result);

      result = 0;
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

      result = 0;
      //_OOO___
      //XXOXOXO
      board.PutToken(false, 0, out win);
      alphaBeta.CheckSecondSituationHorizonatally(true, board, ref result);
      Assert.AreEqual(int.MaxValue, result);


      result = 0;
      //XOOO___
      //XXOXOXO
      board.PutToken(false, 0, out win);
      alphaBeta.CheckSecondSituationHorizonatally(true, board, ref result);
      Assert.AreEqual(900000, result);

      result = 0;
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
      Assert.AreEqual(900000, result);

      result = 0;
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

      //OOOX___
      result = 0;
      board = new Board();
      board.PutToken(true, 0, out win);
      board.PutToken(true, 1, out win);
      board.PutToken(true, 2, out win);
      board.PutToken(false, 3, out win);
      var res = alphaBeta.CheckSecondSituationHorizonatally(true, board, ref result);
      Assert.IsFalse(res);
      Assert.AreEqual(50000, result);

      //_OOX___
      result = 0;
      board.RemoveToken(0);
      alphaBeta.CheckSecondSituationHorizonatally(true, board, ref result);
      Assert.IsFalse(res);
      Assert.AreEqual(0, result);

      //___OOO_
      result = 0;
      board = new Board();
      board.PutToken(true, 3, out win);
      board.PutToken(true, 4, out win);
      board.PutToken(true, 5, out win);
      alphaBeta.CheckSecondSituationHorizonatally(true, board, ref result);
      Assert.AreEqual(int.MaxValue, result);

      board.PutToken(false, 2, out win);
      alphaBeta.CheckSecondSituationHorizonatally(true, board, ref result);
      Assert.AreEqual(900000, result);
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
    public void CheckSecondSituationDiagonallyRightTopTest()
    {
      Board board = new Board();
      bool? win;
      AlphaBeta alphaBeta = new AlphaBeta();
      int result = 0;

      //__XOX__
      //__OXO__
      //_OXXXO_
      //should be inf
      //but its not

      //__X____
      //_XX____
      //XXX____
      board.PutToken(true, 0, out win);
      board.PutToken(true, 1, out win);
      board.PutToken(true, 1, out win);
      board.PutToken(true, 2, out win);
      board.PutToken(true, 2, out win);
      board.PutToken(true, 2, out win);
      alphaBeta.CheckSecondSituationDiagonallyRightTop(true, board, ref result);
      Assert.AreEqual(900000, result);

      //___O
      //__XO___
      //_XXO___
      //XXXO___
      result = 0;
      board.PutToken(false, 3, out win);
      board.PutToken(false, 3, out win);
      board.PutToken(false, 3, out win);
      board.PutToken(false, 3, out win);
      alphaBeta.CheckSecondSituationDiagonallyRightTop(true, board, ref result);
      Assert.AreEqual(50000, result);

      //___O
      //__XO___
      //_XXO___
      //_XXO___
      result = 0;
      board.RemoveToken(0);
      alphaBeta.CheckSecondSituationDiagonallyRightTop(true, board, ref result);
      Assert.AreEqual(0, result);

      result = 0;
      //____X__
      //_______
      //__XO___
      //_XXO___
      //_XXO___
      board.RemoveToken(3);
      board.PutToken(false, 4, out win);
      board.PutToken(false, 4, out win);
      board.PutToken(false, 4, out win);
      board.PutToken(false, 4, out win);
      board.PutToken(true, 4, out win);
      alphaBeta.CheckSecondSituationDiagonallyRightTop(true, board, ref result);
      Assert.AreEqual(900000, result);

      result = 0;
      //______X
      //______X
      //____X_X
      //___XX_X
      board = new Board();
      board.PutToken(true, 3, out win);
      board.PutToken(true, 4, out win);
      board.PutToken(true, 4, out win);
      board.PutToken(true, 6, out win);
      board.PutToken(true, 6, out win);
      board.PutToken(true, 6, out win);
      board.PutToken(true, 6, out win);
      alphaBeta.CheckSecondSituationDiagonallyRightTop(true, board, ref result);
      Assert.AreEqual(900000, result);


      result = 0;
      //__X____
      //_XO____
      //XOO____
      //OXO____
      board = new Board();
      board.PutToken(false, 0, out win);
      board.PutToken(true, 0, out win);
      board.PutToken(true, 1, out win);
      board.PutToken(false, 1, out win);
      board.PutToken(true, 1, out win);
      board.PutToken(false, 2, out win);
      board.PutToken(false, 2, out win);
      board.PutToken(false, 2, out win);
      board.PutToken(true, 2, out win);
      alphaBeta.CheckSecondSituationDiagonallyRightTop(true, board, ref result);
      Assert.AreEqual(900000, result);

      result = 0;
      //_______
      //___X___
      //__XO___
      //__OX___
      //XOOO___
      //OXOO___
      board.RemoveToken(1);
      board.PutToken(false, 3, out win);
      board.PutToken(false, 3, out win);
      board.PutToken(true, 3, out win);
      board.PutToken(false, 3, out win);
      board.PutToken(true, 3, out win);
      alphaBeta.CheckSecondSituationDiagonallyRightTop(true, board, ref result);
      Assert.AreEqual(900000, result);

      result = 0;
      //_______
      //___XX__
      //__XOO__
      //__OXO__
      //XOOOX__
      //OXOOX__
      board.PutToken(true, 4, out win);
      board.PutToken(true, 4, out win);
      board.PutToken(false, 4, out win);
      board.PutToken(false, 4, out win);
      board.PutToken(true, 4, out win);
      alphaBeta.CheckSecondSituationDiagonallyRightTop(true, board, ref result);
      Assert.AreEqual(900000, result);

      //_______
      //___XX__
      //___OO__
      //_XOXO__
      //XOOOX__
      //OXOOX__
      result = 0;
      board.RemoveToken(2);
      board.PutToken(true, 1, out win);
      alphaBeta.CheckSecondSituationDiagonallyRightTop(true, board, ref result);
      Assert.AreEqual(900000, result);

      //____O__
      //___XX__
      //__XOX__
      //_XOOX__
      //_OXOO__
      result = 0;
      board = new Board();
      board.PutToken(false, 1, out win);
      board.PutToken(true, 1, out win);
      board.PutToken(true, 2, out win);
      board.PutToken(false, 2, out win);
      board.PutToken(true, 2, out win);
      board.PutToken(false, 3, out win);
      board.PutToken(false, 3, out win);
      board.PutToken(false, 3, out win);
      board.PutToken(true, 3, out win);
      board.PutToken(false, 4, out win);
      board.PutToken(true, 4, out win);
      board.PutToken(true, 4, out win);
      board.PutToken(true, 4, out win);
      board.PutToken(false, 4, out win);
      alphaBeta.CheckSecondSituationDiagonallyRightTop(true, board, ref result);
      Assert.AreEqual(900000, result);

      result = 0;
      board.RemoveToken(4);
      alphaBeta.CheckSecondSituationDiagonallyRightTop(true, board, ref result);
      Assert.AreEqual(int.MaxValue, result);

    }
    [TestMethod]
    public void CheckSecondSituationDiagonallyLeftTopTest()
    {
      Board board = new Board();
      bool? win;
      AlphaBeta alphaBeta = new AlphaBeta();
      int result = 0;

      //O
      //OX______
      //OXX_____
      //OXXX____
      board.PutToken(false, 0, out win);
      board.PutToken(false, 0, out win);
      board.PutToken(false, 0, out win);
      board.PutToken(false, 0, out win);
      board.PutToken(true, 1, out win);
      board.PutToken(true, 1, out win);
      board.PutToken(true, 1, out win);
      board.PutToken(true, 2, out win);
      board.PutToken(true, 2, out win);
      board.PutToken(true, 3, out win);
      alphaBeta.CheckSecondSituationDiagonallyLeftTop(true, board, ref result);
      Assert.AreEqual(50000, result);

      //OX
      //OXX
      //OXXX
      result = 0;
      board.RemoveToken(0);
      alphaBeta.CheckSecondSituationDiagonallyLeftTop(true, board, ref result);
      Assert.AreEqual(900000, result);

      //_
      //_X
      //_XX
      //_XXX
      result = 0;
      board.RemoveToken(0);
      board.RemoveToken(0);
      board.RemoveToken(0);
      board.RemoveToken(0);
      alphaBeta.CheckSecondSituationDiagonallyLeftTop(true, board, ref result);
      Assert.AreEqual(900000, result);

      //_______
      //__XX___
      //__OOX__          
      //OXOXOX_
      //OXOXOX_
      board = new Board();
      result = 0;
      board.PutToken(false, 0, out win);
      board.PutToken(false, 0, out win);
      board.PutToken(true, 1, out win);
      board.PutToken(true, 1, out win);
      board.PutToken(false, 2, out win);
      board.PutToken(false, 2, out win);
      board.PutToken(false, 2, out win);
      board.PutToken(true, 2, out win);
      board.PutToken(true, 3, out win);
      board.PutToken(true, 3, out win);
      board.PutToken(false, 3, out win);
      board.PutToken(true, 3, out win);
      board.PutToken(false, 4, out win);
      board.PutToken(false, 4, out win);
      board.PutToken(true, 4, out win);
      board.PutToken(true, 5, out win);
      board.PutToken(true, 5, out win);
      alphaBeta.CheckSecondSituationDiagonallyLeftTop(true, board, ref result);
      Assert.AreEqual(int.MaxValue, result);
    }

    [TestMethod]
    public void CheckSecondSituation()
    {
      Board board = new Board();
      bool? win;
      AlphaBeta alphaBeta = new AlphaBeta();
      int result = 0;

      //LEFT TOP
      //_______
      //__XX___
      //__OOX__          
      //OXOXOX_
      //OXOXOX_
      board.PutToken(false, 0, out win);
      board.PutToken(false, 0, out win);
      board.PutToken(true, 1, out win);
      board.PutToken(true, 1, out win);
      board.PutToken(false, 2, out win);
      board.PutToken(false, 2, out win);
      board.PutToken(false, 2, out win);
      board.PutToken(true, 2, out win);
      board.PutToken(true, 3, out win);
      board.PutToken(true, 3, out win);
      board.PutToken(false, 3, out win);
      board.PutToken(true, 3, out win);
      board.PutToken(false, 4, out win);
      board.PutToken(false, 4, out win);
      board.PutToken(true, 4, out win);
      board.PutToken(true, 5, out win);
      board.PutToken(true, 5, out win);
      alphaBeta.CheckSecondSituation(true, board, ref result);
      Assert.AreEqual(int.MaxValue, result);

      //RIGHT TOP
      //_______
      //___XX__
      //__XOX__
      //_XOOX__
      //_OXOO__
      result = 0;
      board = new Board();
      board.PutToken(false, 1, out win);
      board.PutToken(true, 1, out win);
      board.PutToken(true, 2, out win);
      board.PutToken(false, 2, out win);
      board.PutToken(true, 2, out win);
      board.PutToken(false, 3, out win);
      board.PutToken(false, 3, out win);
      board.PutToken(false, 3, out win);
      board.PutToken(true, 3, out win);
      board.PutToken(false, 4, out win);
      board.PutToken(true, 4, out win);
      board.PutToken(true, 4, out win);
      board.PutToken(true, 4, out win);
      alphaBeta.CheckSecondSituation(true, board, ref result);
      Assert.AreEqual(int.MaxValue, result);


      //VERTICALLY
      //_______
      //____X__
      //__XOXX_
      //_XOOXX_
      //_OXOOX_
      board.RemoveToken(3);
      result = 0;
      board.PutToken(true, 5, out win);
      board.PutToken(true, 5, out win);
      board.PutToken(true, 5, out win);
      alphaBeta.CheckSecondSituation(true, board, ref result);
      Assert.AreEqual(int.MaxValue, result);

      //HORIZONTALLY
      //_XXX_
      //OOXOX
      //OOOXO
      board = new Board();
      result = 0;
      board.PutToken(false, 0, out win);
      board.PutToken(false, 0, out win);
      board.PutToken(false, 1, out win);
      board.PutToken(false, 1, out win);
      board.PutToken(true, 1, out win);
      board.PutToken(false, 2, out win);
      board.PutToken(true, 2, out win);
      board.PutToken(true, 2, out win);
      board.PutToken(true, 3, out win);
      board.PutToken(false, 3, out win);
      board.PutToken(true, 3, out win);
      board.PutToken(false, 4, out win);
      board.PutToken(true, 4, out win);
      alphaBeta.CheckSecondSituation(true, board, ref result);
      Assert.AreEqual(int.MaxValue, result);
    }
  }
}
