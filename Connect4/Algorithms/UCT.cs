using Connect4.Algorithm_base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4.Algorithms
{
	using static Constants;

	public class UCT : MCTS
	{
		private double ExpC { get; set; }
		public UCT(double expConst, int seed, int rolloutLimit, MoveEvaluation moveEvaluation = MoveEvaluation.OneAhead) : base(seed, rolloutLimit, moveEvaluation)
		{
			ExpC = expConst;
		}

		public override TreeNode SelectNextNode(TreeNode treeNode)
		{
			var childScores = new double[ncols];
			for (int i = 0; i < ncols; i++)
			{
				var child = treeNode.Children[i];
				if (child == null)
					childScores[i] = double.MinValue;
				else
				{
					//ucb1 formula
					childScores[i] = child.MeanScore + ExpC * Math.Sqrt(Math.Log(Iterations) / child.VisitedCount);
				}
			}

			//find index with max value
			var max = double.MinValue;
			var indexOfMax = 0;
			for (int i = 0; i < ncols; i++)
			{
				if(childScores[i] > max)
				{
					max = childScores[i];
					indexOfMax = i;
				}
			}
			return treeNode.Children[indexOfMax];
		}

		public override string ToString()
		{
			return "UCT";
		}
	}
}
