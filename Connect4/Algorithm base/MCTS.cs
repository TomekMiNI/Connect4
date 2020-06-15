using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4.Algorithm_base
{
	using static Constants;

	public abstract class MCTS : IAlgorithmInterface
	{
		private Random Generator { get; set; }
		private TreeNode Root { get; set; }
		private int RolloutLimit { get; set; }
		internal int Iterations { get; set; }

		public MCTS(int seed, int rolloutLimit)
		{
			Generator = new Random(seed);
			RolloutLimit = rolloutLimit;
		}

		private void Expand()
		{
			//find leaf node in tree
			var currNode = Root;
			while (currNode.AllActionsTested())
			{
				currNode = SelectNextNode(currNode);
			}

			if(currNode.Board.Result == Result.None)
			{
				//create new child node
				var board = currNode.Board.Clone();
				while (!currNode.AllActionsTested() && !board.PutToken((2 + currNode.ActionsTaken) % ncols)) //neat trick, test mid first ;)
					currNode.ActionsTaken++;

				//rollout and propagate outcome
				if (!currNode.AllActionsTested())
				{
					var child = currNode.CreateChild(board);
					var score = Rollout(child);
					child.PropagadeScoreUp(score);
					currNode.Children[currNode.ActionsTaken] = child;
					currNode.ActionsTaken++;
				}
			}
			else
			{
				if(currNode.Board.Score == null)
				{
					currNode.Board.Score = -currNode.Board.CalculateScore();
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
				MakeRandomMove(board);
			}
			var score = 0.0;
			if (board.Result != Result.Draw)
			{
				var loser = board.ActivePlayer;
				score = board.CalculateScore();
				if (childPlayer == loser)
					score = -score;
			}
			return score;
		}

		

		private void MakeRandomMove(Board board)
		{
			//dirty, mb keep in board list of full columns
			while (!board.PutToken(Generator.Next(ncols)))
				continue;
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
			var maxScore = 0.0;
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
	}
}
