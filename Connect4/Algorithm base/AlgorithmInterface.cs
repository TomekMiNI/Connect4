using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4.Algorithm_base
{
	public interface IAlgorithmInterface
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="board">game state</param>
		/// <returns>column to play next chip, -1 if move is impossible</returns>
		int SelectMove(Board board);

		void SetSeed(int seed);

		string ToString();
	}
}
