using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4.Algorithm_base
{
	public static class Constants
	{
		public const int ncols = 7;
		public const bool normaliseBoardScore = true;
		public const MoveSelection moveSelection = MoveSelection.MostVisited;
	}

	public enum MoveSelection
	{
		MostVisited,
		BestScore,
	}
}
