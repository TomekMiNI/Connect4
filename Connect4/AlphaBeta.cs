using Connect4.Algorithm_base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4
{

	public class AlphaBeta : IAlgorithmInterface
	{

		public bool IsFirstPlayer;
		private int depth;

		public AlphaBeta()
		{
		}

		public AlphaBeta(bool isFirstPlayer, int depth = 5)
		{
			this.IsFirstPlayer = isFirstPlayer;
			this.depth = depth;
		}

		

		public int SelectMove(Board board)
		{
			var min = int.MinValue;
			var max = int.MaxValue;
			return CalculateNextMove(IsFirstPlayer, board.ActivePlayer, 0, depth, ref min, ref max, board);
		}

		public void MakeMove(bool firstPlayer, bool turnPlayer, Board board, int maxLevel = 5)
		{
			int alpha = int.MinValue;
			int beta = int.MaxValue;
			int bestRes = alpha;
			int bestMove = 0;
			bestMove = CalculateNextMove(firstPlayer, turnPlayer, 0, maxLevel, ref alpha, ref beta, board);

			board.PutToken(bestMove);
		}

		//return move
		private int CalculateNextMove(bool firstPlayer, bool turnPlayer, int level, int maxLevel, ref int alpha, ref int beta, Board board)
		{
			//its just on max level
			if (level == maxLevel)
				return CalculateCurrentBoard(turnPlayer, board);

			int localAlpha = alpha;
			int localBeta = beta;

			int bestResult = firstPlayer == turnPlayer ? int.MinValue : int.MaxValue;
			int result = 0;
			int bestMove = 0;

			for (int i = 0; i < board.NumberOfColumns; i++)
			{
				Board locBoard = board.Clone();
				bool? win = false;
				bool put = false;
				if (locBoard.PutToken(i))
				{
					if (locBoard.Result == Result.Draw)
						win = null;
					else if (locBoard.Result == Result.None)
						win = false;
					else
						win = true;
					put = true;
					//time to go back in tree
					if (win != false || level == maxLevel)
					{
						if (win == true)
							result = firstPlayer == turnPlayer ? int.MaxValue : int.MinValue;

						//draw, so calculate result for turnPlayer
						else if (win == null)
							result = 0;
					}
					//go deeper
					else
					{
						//increase in case of next starting (turn) player
						if (firstPlayer == turnPlayer) level++;
						result = CalculateNextMove(!firstPlayer, turnPlayer, level, maxLevel, ref alpha, ref beta, locBoard);
					}

				}
				if (!put) continue;
				//there was minimize, so beta is new
				if (firstPlayer == turnPlayer)
				{
					//now we want to maximize
					if (result > bestResult)
					{
						bestResult = result;
						if (alpha < bestResult)
							alpha = bestResult;
						bestMove = i;
					}
					if (win == false)
						level--;
					if (alpha == int.MaxValue || alpha >= localBeta)
						break;
					beta = localBeta;
				}
				else
				{
					//minimize 
					if (result < bestResult)
					{
						bestResult = result;
						if (beta > bestResult)
							beta = bestResult;
						bestMove = i;
					}
					if (beta == int.MinValue || beta <= localAlpha)
						break;
					alpha = localAlpha;
				}
			}
			return (level == 0 && firstPlayer == turnPlayer) ? bestMove : bestResult;
		}

		public int CalculateCurrentBoard(bool firstPlayer, Board board)
		{
			int result = 0;
			if (CheckSecondSituation(firstPlayer, board, ref result)) return result;

			//sum second (50 000), third, fourth
			CheckThirdSituation(firstPlayer, board, ref result);
			CheckFourthSituation(firstPlayer, board, ref result);
			return result;
		}

		public void CheckFourthSituation(bool firstPlayer, Board board, ref int result)
		{
			for (int c = 0; c < board.NumberOfColumns; c++)
			{
				for (int r = 0; r < board.NumberOfRows; r++)
				{
					if (board[c, r] != firstPlayer) continue;
					if (c > 0)
					{
						if (board[c - 1, r] == firstPlayer) { continue; }
						if (r > 0 && board[c - 1, r - 1] == firstPlayer) { continue; }
						if (r < board.NumberOfRows - 1 && board[c - 1, r + 1] == firstPlayer) { continue; }
					}
					if (c < board.NumberOfColumns - 1)
					{
						if (board[c + 1, r] == firstPlayer) { continue; }
						if (r > 0 && board[c + 1, r - 1] == firstPlayer) { continue; }
						if (r < board.NumberOfRows - 1 && board[c + 1, r + 1] == firstPlayer) { continue; }
					}
					if (r > 0 && board[c, r - 1] == firstPlayer) { continue; }
					if (r < board.NumberOfRows - 1 && board[c, r + 1] == firstPlayer) { continue; }
					result += GetValueFromColumn(c);
				}
			}
		}

		private int GetValueFromColumn(int col)
		{
			if (col == 0 || col == 6)
				return 40;
			else if (col == 1 || col == 5)
				return 70;
			else if (col == 2 || col == 4)
				return 120;
			else
				return 200;
		}

		public void CheckThirdSituation(bool firstPlayer, Board board, ref int result)
		{
			CheckThirdSituationHorizontally(firstPlayer, board, ref result);
			CheckThirdSituationVertically(firstPlayer, board, ref result);
			CheckThirdSituationDiagonally(firstPlayer, board, ref result);
		}

		public void CheckThirdSituationDiagonally(bool firstPlayer, Board board, ref int result)
		{
			CheckThirdSituationDiagonallyRightTop(firstPlayer, board, ref result);
			CheckThirdSituationDiagonallyLeftTop(firstPlayer, board, ref result);
		}
		public void CheckThirdSituationDiagonallyRightTop(bool firstPlayer, Board board, ref int result)
		{
			int c = 3;
			int r = 0;
			while (r <= 2)
			{
				var starts = FindStartsOfTwoTokensInRowInDiagonalRightTop(firstPlayer, board, c, r, true);
				foreach (var (col, row) in starts)
					result += GetValueFromThirdSituation(CountLacksInRowInRowInDiagonalRightTop(board, col, row));
				if (c > 0) c--;
				else
					r++;
			}
		}
		public void CheckThirdSituationDiagonallyLeftTop(bool firstPlayer, Board board, ref int result)
		{
			int c = 3;
			int r = 0;
			while (r <= 2)
			{
				var starts = FindStartsOfTwoTokensInRowInDiagonalLeftTop(firstPlayer, board, c, r, true);
				foreach (var (col, row) in starts)
					result += GetValueFromThirdSituation(CountLacksInRowInRowInDiagonalLeftTop(board, col, row));
				if (c < board.NumberOfColumns - 1) c++;
				else r++;
			}
		}


		public void CheckThirdSituationVertically(bool firstPlayer, Board board, ref int result)
		{
			for (int c = 0; c < board.NumberOfColumns; c++)
			{
				int r = board.GetFirstFreeRow(c);
				if (r >= board.NumberOfRows - 1 || r < 2 || (r - 3 >= 0 && board[c, r - 3] == firstPlayer)) continue;

				if (board[c, r - 1] == firstPlayer && board[c, r - 2] == firstPlayer)
					result += GetValueFromThirdSituation(board.NumberOfRows - r);
			}
		}

		public void CheckThirdSituationHorizontally(bool firstPlayer, Board board, ref int result)
		{
			for (int r = 0; r < board.NumberOfRows; r++)
			{
				List<int> starts = FindStartsOfTwoTokensInRowInRow(board, r, firstPlayer, true);
				foreach (var start in starts)
				{
					int count = CountLacksInRowInRow(board, r, start);
					result += GetValueFromThirdSituation(count);
				}
			}
		}

		private int GetValueFromThirdSituation(int count)
		{
			switch (count)
			{
				case 2:
					return 10000;
				case 3:
					return 20000;
				case 4:
					return 30000;
				case 5:
					return 40000;
			}
			return 0;
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
			for (int c = 0; c < currentBoard.NumberOfColumns; c++)
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
				if (found)
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
			for (int rr = 0; rr <= 2; rr++)
				for (int cc = 4 + rr; cc <= 6; cc++)
					if (currentBoard[cc, rr] == null)
						lacks[cc] = true;

			int c = 3;
			int r = 0;
			//start from bottom to fill lacks
			while (r <= 2)
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
		private List<(int, int)> FindStartsOfTwoTokensInRowInDiagonalRightTop(bool firstPlayer, Board currentBoard, int c, int r, bool maxTwo = false)
		{
			bool found = false;
			List<(int, int)> starts = new List<(int, int)>();
			while (c < currentBoard.NumberOfColumns && r < currentBoard.NumberOfRows)
			{
				if (currentBoard[c, r] == firstPlayer)
				{
					if (found)
					{
						bool add = true;
						if (maxTwo)
						{
							//3 in row
							if (c + 1 < currentBoard.NumberOfColumns && r + 1 < currentBoard.NumberOfRows
							  && currentBoard[c + 1, r + 1] == firstPlayer)
							{
								add = false;
								//omit those 3
								found = false;
							}
						}
						if (add)
						{
							starts.Add((c - 1, r - 1));
							found = false;
						}
						if (!maxTwo)
						{
							c--;
							r--;
						}
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
		private List<(int, int)> FindStartsOfTwoTokensInRowInDiagonalLeftTop(bool firstPlayer, Board currentBoard, int c, int r, bool maxTwo = false)
		{
			bool found = false;
			List<(int, int)> starts = new List<(int, int)>();
			while (c >= 0 && r < currentBoard.NumberOfRows)
			{
				if (currentBoard[c, r] == firstPlayer)
				{
					if (found)
					{
						bool add = true;
						if (maxTwo)
						{
							//3 in row
							if (c > 0 && r + 1 < currentBoard.NumberOfRows
							  && currentBoard[c - 1, r + 1] == firstPlayer)
							{
								add = false;
								//omit those 3
								found = false;
							}
						}
						if (add)
						{
							starts.Add((c + 1, r - 1));
							found = false;
						}
						if (!maxTwo)
						{
							c++;
							r--;
						}
					}
					else
						found = true;
				}
				else
					found = false;
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

		private List<int> FindStartsOfTwoTokensInRowInRow(Board currentBoard, int r, bool player, bool maxTwo = false)
		{
			List<int> starts = new List<int>();
			bool start = false;
			for (int i = 0; i < currentBoard.NumberOfColumns; i++)
			{
				if (currentBoard[i, r] == player)
				{
					//two in row
					if (start)
					{
						bool add = true;
						if (maxTwo)
						{
							//3 in row
							if (i + 1 < currentBoard.NumberOfColumns && currentBoard[i + 1, r] == player)
							{
								add = false;
								//omit those 3
								start = false;
							}
						}

						if (add)
						{
							starts.Add(i - 1);
							start = false;
						}
						if (!maxTwo)
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


		private int CountLacksInRowInRow(Board currentBoard, int r, int startC)
		{
			//startC last Index of tokens
			int rightCount = 0;
			int leftCount = 0;
			int col = startC + 1;
			while (++col < currentBoard.NumberOfColumns && currentBoard[col, r] == null)
				rightCount++;
			col = startC;
			while (--col >= 0 && currentBoard[col, r] == null)
				leftCount++;

			return Math.Max(rightCount, leftCount);
		}
		//get list of counts (more than 1) of tokens in row in diagonal
		private int CountLacksInRowInRowInDiagonalRightTop(Board currentBoard, int c, int r)
		{
			int leftCount = 0;
			int rightCount = 0;
			int cc = c + 1;
			int rr = r + 1;
			while (++cc < currentBoard.NumberOfColumns && ++rr < currentBoard.NumberOfRows &&
			  currentBoard[cc, rr] == null) rightCount++;
			cc = c;
			rr = r;
			while (--cc >= 0 && --rr >= 0 &&
			  currentBoard[cc, rr] == null) leftCount++;
			return Math.Max(leftCount, rightCount);
		}
		private int CountLacksInRowInRowInDiagonalLeftTop(Board currentBoard, int c, int r)
		{
			int leftCount = 0;
			int rightCount = 0;
			int cc = c - 2;
			int rr = r + 1;
			//left
			while (--cc >= 0 && ++rr < currentBoard.NumberOfRows &&
			  currentBoard[cc, rr] == null) leftCount++;
			cc = c;
			rr = r;
			//right
			while (++cc < currentBoard.NumberOfColumns && --rr >= 0 &&
			  currentBoard[cc, rr] == null) rightCount++;
			return Math.Max(leftCount, rightCount);
		}

		public override string ToString()
		{
			return "AlphaBeta";
		}

		public void SetSeed(int seed)
		{

		}
	}
}
