using Connect4.Algorithm_base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4
{
	using static Constants;

	public enum Result
	{
		None,
		FirstWon,
		SecondWon,
		Draw
	}

	public class Board
	{
		//7 columns, 6 rows
		public int NumberOfColumns { get; set; }
		public int NumberOfRows { get; set; }
		public int MaxTokensCount => NumberOfColumns * NumberOfRows;
		bool?[][] currentBoard { get; set; }
		public bool ActivePlayer { get; set; }
		public Result Result { get; set; }
		public int FreePlaces { get; set; }
		public double? Score { get; set; }

		public bool? this[int c, int r] => currentBoard[c][r];
		public Board(bool activePlayer = true, int numberOfColumns = 7, int numberOfRows = 6)
		{
			NumberOfColumns = numberOfColumns;
			NumberOfRows = numberOfRows;
			ActivePlayer = activePlayer;
			FreePlaces = MaxTokensCount;
			currentBoard = new bool?[NumberOfColumns][];
			for (int i = 0; i < NumberOfColumns; i++)
				currentBoard[i] = new bool?[NumberOfRows];
		}
		public Board(bool?[][] currentBoard, int freePlaces, Result result, bool activePlayer = true, int numberOfColumns = 7, int numberOfRows = 6)
		{
			NumberOfColumns = numberOfColumns;
			NumberOfRows = numberOfRows;
			ActivePlayer = activePlayer;
			Result = result;
			this.FreePlaces = freePlaces;

			this.currentBoard = new bool?[NumberOfColumns][];
			for (int i = 0; i < NumberOfColumns; i++)
				this.currentBoard[i] = new bool?[NumberOfRows];

			for (int i = 0; i < NumberOfColumns; i++)
				currentBoard[i].CopyTo(this.currentBoard[i], 0);
		}
		public Board Clone()
		{
			return new Board(this.currentBoard, this.FreePlaces, Result, this.ActivePlayer, this.NumberOfColumns, this.NumberOfRows);
		}

		public bool PutToken(int column)
		{
			if (column < 0 || column > 6)
			{
				return false;
			}
			int row = GetFirstFreeRow(column);
			if (row < 6)
			{
				currentBoard[column][row] = ActivePlayer;
				if (Win(column, row))
					Result = ActivePlayer ? Result.FirstWon : Result.SecondWon;
				if (--FreePlaces == 0)
					Result = Result.Draw;
				ActivePlayer = !ActivePlayer;
				return true;
			}
			return false;
		}

		public double CalculateScore()
		{
			if (FreePlaces == 0)
				return 0;
			int tokensUsedByWinner = MovesByWinner();
			double score = (MaxTokensCount / 2 + 1) - tokensUsedByWinner;
			if (normaliseBoardScore)
				score = score / (MaxTokensCount / 2 + 1);
			return score;
		}

		public int MovesByWinner()
		{
			int tokensUsed = MaxTokensCount - FreePlaces;
			int tokensUsedByWinner = (tokensUsed + 1) / 2;
			return tokensUsedByWinner;
		}

		public void RemoveToken(int column)
		{
			int r = GetFirstFreeRow(column);
			if (r > 0)
			{
				currentBoard[column][r - 1] = null;
				FreePlaces++;
				ActivePlayer = !ActivePlayer;
				Result = Result.None;
			}
		}
		public int GetFirstFreeRow(int column)
		{
			int row = 0;
			while (row < 6 && currentBoard[column][row] != null)
				row++;
			return row;
		}
		private bool Win(int column, int row)
		{
			if (HorizontalWin(column, row))
				return true;
			if (VerticalWin(column, row))
				return true;
			if (DiagonalWin(column, row))
				return true;
			return false;
		}

		private bool DiagonalWin(int column, int row)
		{
			int r = row;
			int c = column;
			int times = 1;
			//go left down
			while (r > 0 && c > 0 && currentBoard[column][row] == currentBoard[--c][--r] && ++times < 4) ;
			if (times == 4) return true;
			r = row;
			c = column;
			//go right up
			while (r < 5 && c < 6 && currentBoard[column][row] == currentBoard[++c][++r] && ++times < 4) ;
			if (times == 4) return true;

			times = 1;
			r = row;
			c = column;
			//go left up
			while (r < 5 && c > 0 && currentBoard[column][row] == currentBoard[--c][++r] && ++times < 4) ;
			if (times == 4) return true;
			r = row;
			c = column;
			//go right down
			while (r > 0 && c < 6 && currentBoard[column][row] == currentBoard[++c][--r] && ++times < 4) ;
			return times == 4;

		}

		private bool VerticalWin(int column, int row)
		{
			if (row >= 3)
			{
				int times = 1;
				//go down
				while (currentBoard[column][row] == currentBoard[column][--row] && ++times < 4) ;
				return times == 4;
			}
			return false;
		}

		private bool HorizontalWin(int column, int row)
		{
			int times = 1;
			int col = column;
			//go right
			while (col < 6 && currentBoard[column][row] == currentBoard[++col][row] && ++times < 4) ;
			if (times == 4) return true;
			col = column;
			//go left
			while (col > 0 && currentBoard[column][row] == currentBoard[--col][row] && ++times < 4) ;
			return times == 4;
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			for (int r = 5; r >= 0; r--)
			{
				for (int c = 0; c <= 6; c++)
				{
					switch (currentBoard[c][r])
					{
						case true:
							sb.Append("X ");
							break;
						case false:
							sb.Append("O ");
							break;
						default:
							sb.Append("- ");
							break;
					}
				}
				sb.AppendLine();
			}
			return sb.ToString();
		}
	}
}
