using Connect4;
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
  public partial class Connect4: Form
  {
    private Board board;
    private bool start = false;
    private bool play = false;
    private bool firstPlayer = true;
    private VS vs;
    AlphaBeta ab;
   
    public Connect4()
    {
      InitializeComponent();
    }

    private void mainPanel_Paint(object sender, PaintEventArgs e)
    {
      if (!start) return;
      int cellWidth = MainPanel.Width / MainPanel.ColumnCount;
      int cellHeight = MainPanel.Height / MainPanel.RowCount;

      for(int r = 0; r < board.NumberOfRows; r++)
        for(int c = 0; c < board.NumberOfColumns; c++)
        {
          Brush brush = board[c, r] == null ? Brushes.White : board[c, r] == true ? Brushes.Red : Brushes.Yellow;
          int x = c * cellWidth;
          int y = (MainPanel.RowCount - r - 1) * cellHeight;
          int radius = (cellWidth + cellHeight)/ 4;
          e.Graphics.FillRectangle(Brushes.Blue, new Rectangle(x, y, cellWidth - 1, cellHeight - 1));
          e.Graphics.FillEllipse(brush, new Rectangle(x + cellWidth/4, y + cellHeight/4, radius, radius));
        }
      TurnLabel.BackColor = firstPlayer ? Color.Red : Color.Yellow;
    }

    private void mainPanel_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
    {
      if (!play) return;

      var table = (sender as TableLayoutPanel);
      var posX = e.Location.X;

      var colWidth = table.Width / table.ColumnCount;
      var colIndex = 0;
      while (posX > (colIndex + 1) * colWidth)
        colIndex++;
      if (colIndex == table.ColumnCount - 1) return;
      bool? win;
      board.PutToken(firstPlayer, colIndex, out win);
      table.Invalidate();
      if (win != false)
      {
        if (win == true)
        {
          MessageBox.Show(string.Format("{0} player won!", firstPlayer ? "First" : "Second"));
        }
        else if (win == null)
        {
          MessageBox.Show("DRAW!");
        }
        play = start = false;
      }
      firstPlayer = !firstPlayer;
      var abMove = ab.MakeMove(firstPlayer, firstPlayer, board);
      if(abMove != false)
      {
        if (abMove == true)
        {
          MessageBox.Show(string.Format("{0} player won!", firstPlayer ? "First" : "Second"));
        }
        else if (win == null)
        {
          MessageBox.Show("DRAW!");
        }
        play = start = false;
      }
      firstPlayer = !firstPlayer;

    }

    

    private void StartButton_Click(object sender, EventArgs e)
    {
      ab = new AlphaBeta();
      if (playerRB.Checked)
        vs = VS.Player;
      else if (abRB.Checked)
        vs = VS.AlphaBeta;
      else
        vs = VS.MCTS;
      board = new Board();
      firstPlayer = play = start = true;
      MainPanel.Invalidate();
    }
  }
}
