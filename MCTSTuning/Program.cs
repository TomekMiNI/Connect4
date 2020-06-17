using Connect4;
using Connect4.Algorithm_base;
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
			//TimeAB();
			//TimeMCTS();
			TuneParameterUCT();
			UCTvsTUNED();
			//IsMoreBetter();
			IsBetterMoveChoiceImpactful();
			UCTvsPUCT();
		}

		static double TimeMCTS()
		{
			var uct = new UCT(Math.Sqrt(2), 123, 25000);
			var board = new Board();
			var stopwatch = Stopwatch.StartNew();
			var move = uct.SelectMove(board);
			stopwatch.Stop();
			Console.WriteLine($"Selected move {move}");
			Console.WriteLine($"Time UCT {stopwatch.ElapsedMilliseconds/(double)1000}");
			return stopwatch.ElapsedMilliseconds;
		}

		static double TimeAB()
		{
			var uct = new AlphaBeta(true,5);
			var board = new Board();
			var stopwatch = Stopwatch.StartNew();
			var move = uct.SelectMove(board);
			stopwatch.Stop();
			Console.WriteLine($"Selected move {move}");
			Console.WriteLine($"Time AB {stopwatch.ElapsedMilliseconds / (double)1000}");
			return stopwatch.ElapsedMilliseconds;
		}

		static void TuneParameterUCT()
		{
			var gameCount = 30;
			var abDepth = 5;
			var rolloutCount = 25000;
			var paramList = new List<double>
			{
				1
			};

			foreach (var param in paramList)
			{
				var ResultWinDict = new Dictionary<string, int>()
				{
					{ "UCTfirst" , 0},
					{ "UCTsecond", 0 },
					{ "ABfirst" , 0},
					{ "ABsecond", 0 }
				};

				var ResultDrawDict = new Dictionary<string, int>()
				{
					{ "UCTfirstDraw" , 0},
					{ "ABfirstDraw" , 0},
				};

				for (int i = 0; i < gameCount; i++)
				{
					IAlgorithmInterface firstPlayer;
					IAlgorithmInterface secondPlayer;
					var uct = new UCB1TUNED(param, i, rolloutCount);
					var abFirst = false;
					if (i < gameCount/2)
					{
						abFirst = true;
						var ab = new AlphaBeta(abFirst, abDepth);
						firstPlayer = ab;
						secondPlayer = uct;
					}
					else
					{
						abFirst = false;
						var ab = new AlphaBeta(abFirst, abDepth);
						firstPlayer = uct;
						secondPlayer = ab;
					}
					var board = new Board();
					var activePlayer = firstPlayer;
					while(board.Result == Result.None)
					{
						board.PutToken(activePlayer.SelectMove(board));
						if (activePlayer == firstPlayer)
							activePlayer = secondPlayer;
						else
							activePlayer = firstPlayer;
					}

					if(board.Result == Result.Draw)
					{

					}
					else
					{
						if(board.Result == Result.FirstWon)
						{
							if (abFirst)
								ResultWinDict["ABfirst"]++;
							else
								ResultWinDict["UCTfirst"]++;
						}
						else
						{
							if (abFirst)
								ResultWinDict["UCTsecond"]++;
							else
								ResultWinDict["ABsecond"]++;
						}
					}
				}
				Console.WriteLine($"Param {param}:");
				foreach (var kvp in ResultWinDict)
				{
					Console.WriteLine($"{kvp.Key}: {kvp.Value}");
				}
				Console.WriteLine();
			}

		

		}

		static void UCTvsTUNED()
		{
			var gameCount = 30;
			var rolloutCount = 25000;

			var ResultWinDict = new Dictionary<string, int>()
			{
				{ "UCTfirst" , 0},
				{ "UCTsecond", 0},
				{ "TUNEDfirst" , 0},
				{ "TUNEDsecond", 0}
			};

			for (int i = 0; i < gameCount; i++)
			{
				IAlgorithmInterface firstPlayer;
				IAlgorithmInterface secondPlayer;
				var uct = new UCT(Math.Sqrt(2), i, rolloutCount);
				var tuned = new UCB1TUNED(1, i, rolloutCount);
				var tunedFirst = false;
				if (i < gameCount / 2)
				{
					tunedFirst = true;
					firstPlayer = tuned;
					secondPlayer = uct;
				}
				else
				{
					tunedFirst = false;
					firstPlayer = uct;
					secondPlayer = tuned;
				}
				var board = new Board();
				var activePlayer = firstPlayer;
				while (board.Result == Result.None)
				{
					board.PutToken(activePlayer.SelectMove(board));
					if (activePlayer == firstPlayer)
						activePlayer = secondPlayer;
					else
						activePlayer = firstPlayer;
				}


				if (board.Result == Result.FirstWon)
				{
					if (tunedFirst)
						ResultWinDict["TUNEDfirst"]++;
					else
						ResultWinDict["UCTfirst"]++;
				}
				else
				{
					if (tunedFirst)
						ResultWinDict["UCTsecond"]++;
					else
						ResultWinDict["TUNEDsecond"]++;
				}

			}
			Console.WriteLine("UCT vs UCB1TUNED");
			foreach (var kvp in ResultWinDict)
			{
				Console.WriteLine($"{kvp.Key}: {kvp.Value}");
			}
			Console.WriteLine();
		}

		static void IsMoreBetter()
		{
			var gameCount = 30;
			var rolloutCount = 10000;

			var ResultWinDict = new Dictionary<string, int>()
			{
				{ "UCTfirst" , 0},
				{ "UCTsecond", 0},
				{ "Boostedfirst" , 0},
				{ "Boostedsecond", 0}
			};

			for (int i = 0; i < gameCount; i++)
			{
				IAlgorithmInterface firstPlayer;
				IAlgorithmInterface secondPlayer;
				var uct = new UCT(2, i, rolloutCount);
				var uctBoosted = new UCT(2, i, 10 * rolloutCount);
				var boostedFirst = false;
				if (i < gameCount / 2)
				{
					boostedFirst = true;
					firstPlayer = uctBoosted;
					secondPlayer = uct;
				}
				else
				{
					boostedFirst = false;
					firstPlayer = uct;
					secondPlayer = uctBoosted;
				}
				var board = new Board();
				var activePlayer = firstPlayer;
				while (board.Result == Result.None)
				{
					board.PutToken(activePlayer.SelectMove(board));
					if (activePlayer == firstPlayer)
						activePlayer = secondPlayer;
					else
						activePlayer = firstPlayer;
				}


				if (board.Result == Result.FirstWon)
				{
					if (boostedFirst)
						ResultWinDict["Boostedfirst"]++;
					else
						ResultWinDict["UCTfirst"]++;
				}
				else
				{
					if (boostedFirst)
						ResultWinDict["UCTsecond"]++;
					else
						ResultWinDict["Boostedsecond"]++;
				}

			}
			Console.WriteLine("UCT vs UCTx10");
			foreach (var kvp in ResultWinDict)
			{
				Console.WriteLine($"{kvp.Key}: {kvp.Value}");
			}
			Console.WriteLine();
		}

		static void IsBetterMoveChoiceImpactful()
		{
			var gameCount = 30;
			var rolloutCount = 25000;

			var ResultWinDict = new Dictionary<string, int>()
			{
				{ "UCTfirst" , 0},
				{ "UCTsecond", 0},
				{ "OneAheadfirst" , 0},
				{ "OneAheadsecond", 0}
			};

			for (int i = 0; i < gameCount; i++)
			{
				IAlgorithmInterface firstPlayer;
				IAlgorithmInterface secondPlayer;
				var uct = new UCT(Math.Sqrt(2), i, rolloutCount, MoveEvaluation.Random);
				var oneAhead = new UCT(Math.Sqrt(2), i, rolloutCount, MoveEvaluation.OneAhead);
				var oneAheadFirst = false;
				if (i < gameCount / 2)
				{
					oneAheadFirst = true;
					firstPlayer = oneAhead;
					secondPlayer = uct;
				}
				else
				{
					oneAheadFirst = false;
					firstPlayer = uct;
					secondPlayer = oneAhead;
				}
				var board = new Board();
				var activePlayer = firstPlayer;
				while (board.Result == Result.None)
				{
					board.PutToken(activePlayer.SelectMove(board));
					if (activePlayer == firstPlayer)
						activePlayer = secondPlayer;
					else
						activePlayer = firstPlayer;
				}


				if (board.Result == Result.FirstWon)
				{
					if (oneAheadFirst)
						ResultWinDict["OneAheadfirst"]++;
					else
						ResultWinDict["UCTfirst"]++;
				}
				else
				{
					if (oneAheadFirst)
						ResultWinDict["UCTsecond"]++;
					else
						ResultWinDict["OneAheadsecond"]++;
				}

			}
			Console.WriteLine("UCT random vs UCT OneAhead");
			foreach (var kvp in ResultWinDict)
			{
				Console.WriteLine($"{kvp.Key}: {kvp.Value}");
			}
			Console.WriteLine();
		}


		static void UCTvsPUCT()
		{
			var gameCount = 30;
			var rolloutCount = 25000;

			var ResultWinDict = new Dictionary<string, int>()
			{
				{ "UCTfirst" , 0},
				{ "UCTsecond", 0},
				{ "PUCTfirst" , 0},
				{ "PUCTsecond", 0}
			};

			for (int i = 0; i < gameCount; i++)
			{
				IAlgorithmInterface firstPlayer;
				IAlgorithmInterface secondPlayer;
				var uct = new UCT(Math.Sqrt(2), i, rolloutCount);
				var puct = new PUCT(Math.Sqrt(2), i, rolloutCount);
				var puctFirst = false;
				if (i < gameCount / 2)
				{
					puctFirst = true;
					firstPlayer = puct;
					secondPlayer = uct;
				}
				else
				{
					puctFirst = false;
					firstPlayer = uct;
					secondPlayer = puct;
				}
				var board = new Board();
				var activePlayer = firstPlayer;
				while (board.Result == Result.None)
				{
					board.PutToken(activePlayer.SelectMove(board));
					if (activePlayer == firstPlayer)
						activePlayer = secondPlayer;
					else
						activePlayer = firstPlayer;
				}


				if (board.Result == Result.FirstWon)
				{
					if (puctFirst)
						ResultWinDict["PUCTfirst"]++;
					else
						ResultWinDict["UCTfirst"]++;
				}
				else
				{
					if (puctFirst)
						ResultWinDict["UCTsecond"]++;
					else
						ResultWinDict["PUCTsecond"]++;
				}

			}
			Console.WriteLine("PUCT vs UCT");
			foreach (var kvp in ResultWinDict)
			{
				Console.WriteLine($"{kvp.Key}: {kvp.Value}");
			}
			Console.WriteLine();
		}
	}
}
