using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4.Algorithm_base
{
	using static Constants;

	public enum MoveEvaluation
	{
		Random,
		OneAhead
	}

	public abstract class MCTS : IAlgorithmInterface
	{
		private Random Generator { get; set; }
		private TreeNode Root { get; set; }
		private int RolloutLimit { get; set; }
		internal int Iterations { get; set; }
		internal MoveEvaluation MoveEvaluation { get; set; }


		public MCTS(int seed, int rolloutLimit, MoveEvaluation moveEvaluation)
		{
			Generator = new Random(seed);
			RolloutLimit = rolloutLimit;
			MoveEvaluation = moveEvaluation;
		}

		private void Expand()
		{
			//find leaf node in tree
			var currNode = Root;
			while (currNode.AllActionsTested() && currNode.Board.Result == Result.None)
			{
				currNode = SelectNextNode(currNode);
			}

			for (int i = 0; i < ncols; i++)
			{
				if (currNode.Children[i] != null
					&& (currNode.Children[i].Board.Result == Result.FirstWon || currNode.Children[i].Board.Result == Result.SecondWon))
				{
					currNode = currNode.Children[i];
					break;
				}
			}

			if (currNode.Board.Result == Result.None)
			{
				//create new child node
				var board = currNode.Board.Clone();

				var unvisitedChildren = new List<int>();
				for (int i = 0; i < ncols; i++)
				{
					if(currNode.Children[i] == null && board[i,board.NumberOfRows-1] == null)
					{
						unvisitedChildren.Add(i);
					}
				}

				int move = 0;
				if(MoveEvaluation == MoveEvaluation.OneAhead)
				{
					move = ChooseLessRandomMove(board);
					if (!unvisitedChildren.Contains(move))
					{
						move = unvisitedChildren[Generator.Next(unvisitedChildren.Count)];
					}
				}
				else
				{
					move = unvisitedChildren[Generator.Next(unvisitedChildren.Count)];
				}
				board.PutToken(move);
			
				var child = currNode.CreateChild(board);
				var score = Rollout(child);
				child.PropagadeScoreUp(score);
				currNode.Children[move] = child;
				currNode.ActionsTaken++;
			}
			else
			{
				if(currNode.Board.Score == null)
				{
					currNode.Board.Score = currNode.Board.CalculateScore();
				}
				currNode.PropagadeScoreUp(currNode.Board.Score.Value);
			}
			
		}

		private double Rollout(TreeNode node)
		{
			var board = node.Board.Clone();
			//child was created by putting token, then it's opponent's turn
			var childPlayer = !node.Board.ActivePlayer;
			while (board.Result == Result.None)
			{
				MakeMove(board);
			}

			var loser = board.ActivePlayer;
			var score = board.CalculateScore();
			if (childPlayer == loser)
				score = -score;

			return score;
		}

		private int MakeMove(Board board)
		{
			var move = 0;
			if (MoveEvaluation == MoveEvaluation.OneAhead)
				move = MakeLessRandomMove(board);
			else
				move = MakeRandomMove(board);
			return move;
		}

		private int MakeRandomMove(Board board)
		{
			//dirty, mb keep in board list of full columns
			var move = Generator.Next(ncols);
			while (!board.PutToken(move))
			{
				move = Generator.Next(ncols);
			}
			return move;
		}

		private int MakeLessRandomMove(Board board)
		{
			int move = ChooseLessRandomMove(board);
			board.PutToken(move);
			return move;
		}

		private int ChooseLessRandomMove(Board board)
		{
			var possibleMoves = new List<int>();
			var winningMoves = new List<int>(); //and draws
			var losingMoves = new List<int>();
			for (int i = 0; i < ncols; i++)
			{
				if (board.PutToken(i))
				{
					possibleMoves.Add(i);
					if (board.Result != Result.None)
					{
						winningMoves.Add(i);
					}
					board.RemoveToken(i);
					board.PutToken(i);
					if (board.Result != Result.None)
					{
						losingMoves.Add(i);
					}
					board.RemoveToken(i);
				}
			}
			var move = 0;
			if (winningMoves.Any())
				move = winningMoves[Generator.Next(winningMoves.Count)];
			else if (losingMoves.Count == 1)
				move = losingMoves.First();
			else
				move = possibleMoves[Generator.Next(possibleMoves.Count)];
			return move;
		}

		public abstract TreeNode SelectNextNode(TreeNode treeNode);

		public int SelectMove(Board board)
		{
			//clean start every time
			Root = new TreeNode(board);
			Iterations = 0;
			while(Iterations < RolloutLimit)
			{
				Iterations++;
				Expand();
			}
			switch (Constants.moveSelection)
			{
				case MoveSelection.MostVisited:
					return MaxVisitAction();
				case MoveSelection.BestScore:
					return BestScoreAction();
			}
		}

		//robust child https://ai.stackexchange.com/questions/16905/mcts-how-to-choose-the-final-action-from-the-root
		private int MaxVisitAction()
		{
			var visitMax = 0;
			var indexOfMax = 0;
			for (int i = 0; i < ncols; i++)
			{
				if (Root.Children[i] != null && Root.Children[i].VisitedCount > visitMax)
				{
					visitMax = Root.Children[i].VisitedCount;
					indexOfMax = i;
				}
			}
			return indexOfMax;
		}

		//max child
		private int BestScoreAction()
		{
			var maxScore = double.MinValue;
			var indexOfMax = 0;
			for (int i = 0; i < ncols; i++)
			{
				if (Root.Children[i] != null && Root.Children[i].MeanScore > maxScore)
				{
					maxScore = Root.Children[i].MeanScore;
					indexOfMax = i;
				}
			}
			return indexOfMax;
		}

		public void SetSeed(int seed)
		{
			Generator = new Random(seed);
		}
	}
}
