using Connect4;
using Connect4.Algorithms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCTSTuning
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine($"Elapsed time {TimeMCTS() / 1000}");
		}

		static double TimeMCTS()
		{
			var uct = new UCT(2, 123, 100000);
			var board = new Board();
			var stopwatch = Stopwatch.StartNew();
			var move = uct.SelectMove(board);
			stopwatch.Stop();
			Console.WriteLine($"Selected move {move}");
			return stopwatch.ElapsedMilliseconds;
		}
	}
}
