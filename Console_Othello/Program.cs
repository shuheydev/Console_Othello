// See https://aka.ms/new-console-template for more information

using Console_Othello;

const int PLAYERNAME_LINE_NUM = 0;
const int MESSAGE_LINE_NUM = 1;
const int BOARD_LINE_NUM = 2;



var gameManager = new GameManager();

gameManager.InitBoard();

gameManager.Players.Add(new Human("Shuhei"));
gameManager.Players.Add(new Human("Emi"));
gameManager.Players.Add(new Human("Haru"));
gameManager.Players.Add(new Human("Natsu"));

gameManager.SetPlayerOrder(0);


Console.Clear();
Play();


void Play()
{
    while (true)
    {
        Console.SetCursorPosition(0, PLAYERNAME_LINE_NUM);
        Console.Write($"{gameManager.CurrentPlayer.Name} : {gameManager.GetPlayerMark(gameManager.CurrentPlayer.ID)}");

        Console.SetCursorPosition(0, BOARD_LINE_NUM);
        DrawBoard(gameManager.Board);

        Console.Write("Input[row,column]:");
        string input = Console.ReadLine();

        //入力の形式が正しいかどうか
        if (!CheckInput(input, out int row, out int col))
        {
            Console.Clear();
            Console.SetCursorPosition(0, MESSAGE_LINE_NUM);
            Console.Write($"入力が正しくありません: {input}");

            continue;
        }


        //石を置く
        if (!gameManager.PlaceStone(row, col))
        {
            Console.Clear();
            Console.SetCursorPosition(0, MESSAGE_LINE_NUM);
            Console.Write($"そこには置けません: {row}, {col}");

            continue;
        }

        gameManager.NextPlayer();

        Console.Clear();
    }
}

bool CheckInput(string input, out int row, out int col)
{
    row = -1;
    col = -1;

    if (string.IsNullOrEmpty(input))
    {
        return false;
    }
    var splitted = input.Split(',');
    if (splitted.Length != 2)
    {
        return false;
    }

    if (!int.TryParse(splitted[0], out int intRow))
    {
        return false;
    }
    if (!int.TryParse(splitted[1], out int intCol))
    {
        return false;
    }

    row = intRow;
    col = intCol;
    return true;
}
void DrawBoard(List<List<ID>> board)
{
    Console.Write("  ");
    Console.WriteLine(String.Join(" ", Enumerable.Range(0, board.Count)));
    for (int i = 0; i < board.Count; i++)
    {
        var row = board[i].Select(x => gameManager.GetPlayerMark(x));
        Console.Write($"{i,-2}");
        Console.WriteLine(string.Join("", row));
    }
}








