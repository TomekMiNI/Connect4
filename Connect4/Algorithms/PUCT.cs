using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connect4.Algorithm_base;

namespace Connect4.Algorithms
{
	using static Algorithm_base.Constants;


	public class PUCT : MCTS
	{
		private double ExpC { get; set; }
		public PUCT(double expConst, int seed, int rolloutLimit, MoveEvaluation moveEvaluation = MoveEvaluation.Random) : base(seed, rolloutLimit, moveEvaluation)
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
					//puct
					childScores[i] = child.MeanScore + ExpC * Math.Sqrt(Iterations) / child.VisitedCount;
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

		public override string ToString()
		{
			return "PUCT" + ExpC.ToString();
		}
	}
}
