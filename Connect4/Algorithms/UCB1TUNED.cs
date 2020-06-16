using Connect4.Algorithm_base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4.Algorithms
{
	using static Constants;

	public class UCB1TUNED : MCTS
	{
		private double ExpC { get; set; }
		public UCB1TUNED(double expConst, int seed, int rolloutLimit) : base(seed, rolloutLimit)
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
					//UCB1TUNED formula
					var V = child.Var - child.MeanScore * child.MeanScore + Math.Sqrt(2 * Math.Log(Iterations) / child.VisitedCount);
					childScores[i] = child.MeanScore + ExpC * Math.Sqrt(Math.Min(0.25, V) * Math.Log(Iterations) / child.VisitedCount);
				}
			}

			//find index with max value
			var max = double.MinValue;
			var indexOfMax = 0;
			for (int i = 0; i < ncols; i++)
			{
				if (childScores[i] > max)
				{
					max = childScores[i];
					indexOfMax = i;
				}
			}
			return treeNode.Children[indexOfMax];
		}
	}
}
