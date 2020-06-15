using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4.Algorithm_base
{
	using static Constants;

	public class TreeNode
	{
		internal Board Board { get; set; }
		internal TreeNode Parent { get; set; }
		internal TreeNode[] Children { get; set; }
		public int ActionsTaken { get; set; }
		public int VisitedCount { get; set; }
		public double CumulativeScore { get; set; }
		public double MeanScore
		{
			get
			{
				return CumulativeScore / VisitedCount;
			}
		}

		public TreeNode(Board board)
		{
			Board = board;
			Children = new TreeNode[ncols];
		}

		public bool AllActionsTested()
		{
			return ActionsTaken == ncols;
		}

		public void PropagadeScoreUp(double score)
		{
			var node = this;
			while (node.Parent != null)
			{
				node.VisitedCount++;
				node.CumulativeScore += score;
				//node.UpdateNodeStats(score);
				score = -score;
				node = node.Parent;
			}
		}

		public TreeNode CreateChild(Board board)
		{
			var node = new TreeNode(board)
			{
				Parent = this
			};
			return node;
		}

		//public abstract void UpdateNodeStats(int outcome);
		//public abstract TreeNode CreateNode(Board board);
	}

}
