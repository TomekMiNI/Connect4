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
			//TimeMCTSRandom();
			//TuneParameterUCT();
			//UCTvsTUNED();
			//IsMoreBetter();
			//IsBetterMoveChoiceImpactful();
			//UCTvsPUCT();
			TestAlgorithms();
		}


		static void TestAlgorithms()
		{
			var rollouts = 16000;
			var gameCount = 100;
			var algorithms = new List<IAlgorithmInterface>()
			{
				new PUCT(Math.Sqrt(2), 0, rollouts),
				new UCT(Math.Sqrt(2), 0, rollouts),
				new UCB1TUNED(1, 0, rollouts),
				new AlphaBeta()
			};
			var ResultDict = new Dictionary<string, int>();
			var UsedMovesSum = new Dictionary<string, int>();
			for (int i = 0; i < algorithms.Count; i++)
			{
				for (int j = i + 1; j < algorithms.Count; j++)
				{
					//A vs B, A is first player
					ResultDict[algorithms[i].ToString() + " vs " + algorithms[j].ToString()] = 0;
					ResultDict[algorithms[j].ToString() + " vs " + algorithms[i].ToString()] = 0;
					UsedMovesSum[algorithms[i].ToString() + " vs " + algorithms[j].ToString()] = 0;
					UsedMovesSum[algorithms[j].ToString() + " vs " + algorithms[i].ToString()] = 0;

					IAlgorithmInterface firstPlayer = algorithms[i];
					IAlgorithmInterface secondPlayer = algorithms[j];
					for (int k = 0; k < gameCount; k++)
					{
						if(k < gameCount/2)
							PlayGame(ResultDict, UsedMovesSum, firstPlayer, secondPlayer, k);
						else
							PlayGame(ResultDict, UsedMovesSum, secondPlayer, firstPlayer, k);
						Console.Write(".");
					}
					Console.WriteLine();
					Console.WriteLine($"Progess: {i} vs {j} done");
				}
			}
			foreach (var kvp in ResultDict)
			{
				Console.WriteLine($"{kvp.Key}: {kvp.Value}");
			}
			var csv = string.Join(
				Environment.NewLine,
				ResultDict.Select(d => $"{d.Key};{d.Value};{UsedMovesSum[d.Key]};")
			);
			System.IO.File.WriteAllText("./result.csv", csv);
		}

		private static void PlayGame(Dictionary<string, int> ResultDict, Dictionary<string, int> UsedMovesSum, IAlgorithmInterface firstPlayer, IAlgorithmInterface secondPlayer, int k)
		{
			if(firstPlayer is AlphaBeta st)
			{
				st.IsFirstPlayer = true;
			}
			if(secondPlayer is AlphaBeta nd)
			{
				nd.IsFirstPlayer = false;
			}

			firstPlayer.SetSeed(k);
			secondPlayer.SetSeed(k);
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

			if (board.Result == Result.Draw)
			{

			}
			else
			{
				if (board.Result == Result.FirstWon)
				{
					ResultDict[firstPlayer.ToString() + " vs " + secondPlayer.ToString()]++;
					UsedMovesSum[firstPlayer.ToString() + " vs " + secondPlayer.ToString()]+= board.MovesByWinner();
				}
				else
				{
					ResultDict[secondPlayer.ToString() + " vs " + firstPlayer.ToString()]++;
					UsedMovesSum[secondPlayer.ToString() + " vs " + firstPlayer.ToString()]+= board.MovesByWinner();
				}
			}
		}

		static void TimeMCTS()
		{
			var uct = new UCB1TUNED(1, 123, 16000);
			var board = new Board();
			while(board.Result == Result.None)
				TimeOneMove(uct, board);
		}

		private static void TimeOneMove(IAlgorithmInterface uct, Board board)
		{
			var stopwatch = Stopwatch.StartNew();
			var move = uct.SelectMove(board);
			stopwatch.Stop();
			board.PutToken(move);
			Console.WriteLine($"Selected move {move}");
			Console.WriteLine($"Time UCT {stopwatch.ElapsedMilliseconds / (double)1000}");
		}

		static double TimeMCTSRandom()
		{
			var uct = new UCT(Math.Sqrt(2), 123, 25000, MoveEvaluation.Random);
			var board = new Board();
			var stopwatch = Stopwatch.StartNew();
			var move = uct.SelectMove(board);
			stopwatch.Stop();
			Console.WriteLine($"Selected move {move}");
			Console.WriteLine($"Time UCT Random {stopwatch.ElapsedMilliseconds / (double)1000}");
			return stopwatch.ElapsedMilliseconds;
		}

		static void TimeAB()
		{
			var uct = new AlphaBeta(true,5);
			var board = new Board();
			while (board.Result == Result.None)
				TimeOneMove(uct, board);
		}

		static void TuneParameterUCT()
		{
			var gameCount = 30;
			var abDepth = 5;
			var rolloutCount = 200000;
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
				var tuned = new UCB1TUNED(Math.Sqrt(2), i, rolloutCount);
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
				var uct = new UCB1TUNED(1, i,5 * rolloutCount, MoveEvaluation.Random);
				var oneAhead = new UCB1TUNED(1, i, rolloutCount, MoveEvaluation.OneAhead);
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
