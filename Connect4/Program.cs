using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4
{
  enum Result
  {
    None,
    FirstWon,
    SecondWon,
    Draw
  }
  class Program
  {
    static void Main(string[] args)
    {
      Game game = new Game();
      game.Start();
    }
  }
  class Game
  {
    Result result { get; set; }
    Board board { get; set; }
    public Game()
    {
      result = Result.None;
      board = new Board();
    }
    public Result Start()
    {
      while (true)
      {
        if ((result = MakeMove(firstPlayer: true)) != Result.None)
          break;
        if ((result = MakeMove(firstPlayer: false)) != Result.None)
          break;
      }
      switch(result)
      {
        case Result.Draw:
          Console.WriteLine("DRAW!");
          break;
        case Result.FirstWon:
          Console.WriteLine("GRACZ 1 WYGRAŁ!");
          break;
        case Result.SecondWon:
          Console.WriteLine("GRACZ 2 WYGRAŁ!");
          break;
        default:
          Console.WriteLine("QUE ESTA PASANDO!?");
          break;
      }
      Console.WriteLine(board);
      return result;
    }
    private Result MakeMove(bool firstPlayer)
    {
      while (true)
      {
        Console.WriteLine(board);
        Console.WriteLine(string.Format("Wybierz ruch {0}:", firstPlayer ? "X" : "0"));
        int column = -1;
        while (true)
        {
          try
          {
            column = Convert.ToInt32(Console.ReadLine());
            break;
          }
          catch
          {
            Console.WriteLine("Zła wartość kolumny!");
          }
        }
        bool? win;
        if (board.PutToken(firstPlayer, column, out win))
        {
          if (win == true)
          {
            if(firstPlayer)
              return Result.FirstWon;
            else
              return Result.SecondWon;
          }
          else if (win == null)
            return Result.Draw;
          break;
        }
      }
      return Result.None;
    }

    private Result MakeABMove(bool firstPlayer)
    {
      return Result.None;
    }
  }
}
