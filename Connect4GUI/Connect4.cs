using Connect4;
using Connect4.Algorithm_base;
using Connect4.Algorithms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Connect4GUI
{
	enum VS
	{
		Player,
		AlphaBeta,
		MCTS
	}
	public partial class Connect4 : Form
	{
		private Board board;
		private bool start = false;
		private bool play = false;
		private bool firstPlayer = true;
		private VS vs;
		AlphaBeta ab;
		UCT uct;
		PUCT puct;
		UCB1TUNED ucb1tuned;
		private bool yourTurn = true;
		IAlgorithmInterface algorithm = null;

		public Connect4()
		{
			InitializeComponent();
		}

		private void mainPanel_Paint(object sender, PaintEventArgs e)
		{
			if (!start) return;
			int cellWidth = MainPanel.Width / MainPanel.ColumnCount;
			int cellHeight = MainPanel.Height / MainPanel.RowCount;

			for (int r = 0; r < board.NumberOfRows; r++)
				for (int c = 0; c < board.NumberOfColumns; c++)
				{
					Brush brush = board[c, r] == null ? Brushes.White : board[c, r] == true ? Brushes.Red : Brushes.Yellow;
					int x = c * cellWidth;
					int y = (MainPanel.RowCount - r - 1) * cellHeight;
					int radius = (cellWidth + cellHeight) / 4;
					e.Graphics.FillRectangle(Brushes.Blue, new Rectangle(x, y, cellWidth - 1, cellHeight - 1));
					e.Graphics.FillEllipse(brush, new Rectangle(x + cellWidth / 4, y + cellHeight / 4, radius, radius));
				}
			TurnLabel.BackColor = firstPlayer ? Color.Red : Color.Yellow;
		}

		private void mainPanel_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (!play) return;

			//player move
			var table = (sender as TableLayoutPanel);
			var posX = e.Location.X;
			var colWidth = table.Width / table.ColumnCount;
			var colIndex = 0;
			while (posX > (colIndex + 1) * colWidth)
				colIndex++;
			if (colIndex == table.ColumnCount - 1) return;
			board.PutToken(colIndex);
			table.Invalidate();

			if (CheckResult())
				return;
			firstPlayer = !firstPlayer;
			PlayMove();
			CheckResult();
			firstPlayer = !firstPlayer;
		}

		private bool CheckResult()
		{
			if (board.Result != Result.None)
			{
				switch (board.Result)
				{
					case Result.FirstWon:
						MessageBox.Show(string.Format("First player won!"));
						break;
					case Result.SecondWon:
						MessageBox.Show(string.Format("Second player won!"));
						break;
					case Result.Draw:
						MessageBox.Show("DRAW!");
						break;
				}
				play = start = false;
				return true;
			}
			return false;
		}

		private void StartButton_Click(object sender, EventArgs e)
		{
			ab = new AlphaBeta();
			uct = new UCT(Math.Sqrt(2), 123, 100000);
			puct = new PUCT(Math.Sqrt(2), 123, 100000);
			ucb1tuned = new UCB1TUNED(1, 123, 100000);
			if (playerRB.Checked)
				vs = VS.Player;
			else if (abRB.Checked)
				vs = VS.AlphaBeta;
			else
				vs = VS.MCTS;
			
			if (puctRB.Checked)
				algorithm = puct;
			if (uctRB.Checked)
				algorithm = uct;
			if (tunedRB.Checked)
				algorithm = ucb1tuned;

			yourTurn = youStartBox.Checked;
			board = new Board();
			firstPlayer = play = start = true;
			MainPanel.Invalidate();
			if (!yourTurn)
			{
				PlayMove();
				CheckResult();
				firstPlayer = !firstPlayer;
				MainPanel.Invalidate();
			}
		}

		private void PlayMove()
		{
			if (vs == VS.AlphaBeta)
			{
				ab.MakeMove(firstPlayer, firstPlayer, board);
			}
			else
			{
				var move = algorithm.SelectMove(board);
				board.PutToken(move);
			}
		}
	}
}
