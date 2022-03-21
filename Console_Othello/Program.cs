// See https://aka.ms/new-console-template for more information

using Console_Othello;

const int PLAYERNAME_LINE_NUM = 0;
const int MESSAGE_LINE_NUM = 1;
const int BOARD_LINE_NUM = 2;



var gameManager = new GameManager();

gameManager.InitBoard();

gameManager.Players.Add(new Random_CPU("スズキ"));
gameManager.Players.Add(new Random_CPU("タカハシ"));
gameManager.Players.Add(new Random_CPU("ヤマダ"));
gameManager.Players.Add(new Random_CPU("サトウ"));

gameManager.SetPlayerOrder(0);
gameManager.InitPlayerLifeList();

Console.Clear();
Console.WriteLine("何かキーを押すと始まります");
Console.ReadKey();

Play();


void Play()
{
    while (true)
    {
        Console.Write($"{gameManager.CurrentPlayer.Name} : {gameManager.GetPlayerMark(gameManager.CurrentPlayer.ID)} : {gameManager.GetPlayerLife(gameManager.CurrentPlayer.ID)}");

        Console.SetCursorPosition(0, BOARD_LINE_NUM);
        DrawBoard(gameManager.Board);

        var score = gameManager.GetScore();
        DisplayScore(score);

        if (gameManager.IsFinish())
        {
            break;
        }

        if (gameManager.GetPlayerLife(gameManager.CurrentPlayer.ID) <= 0)
        {
            gameManager.NextPlayer();
            continue;
        }

        Console.SetCursorPosition(0, PLAYERNAME_LINE_NUM);

        int row;
        int col;

        //石を置く場所を取得する
        if (gameManager.CurrentPlayer.Type == PlayerType.Human)
        {
            Console.Write("Input[row,column]:");
            string input = Console.ReadLine();

            //入力の形式が正しいかどうか
            if (!CheckUserInput(input, out row, out col))
            {
                Console.Clear();
                Console.SetCursorPosition(0, MESSAGE_LINE_NUM);
                Console.Write($"入力が正しくありません: {input}");

                //置けなかったらライフが1減って次のプレイヤーへ
                gameManager.DecreaseLife(gameManager.CurrentPlayer.ID);
                gameManager.NextPlayer();

                continue;
            }
        }
        else
        {
            //CPUの場合
            (row, col) = gameManager.CurrentPlayer.Place(gameManager.Board);

            Task.Delay(100).Wait();
        }


        //石を置く
        if (!gameManager.PlaceStone(row, col))
        {
            Console.Clear();
            Console.SetCursorPosition(0, MESSAGE_LINE_NUM);
            Console.Write($"そこには置けません: {row}, {col}");

            //置けなかったらライフが1減って次のプレイヤーへ
            gameManager.DecreaseLife(gameManager.CurrentPlayer.ID);
            gameManager.NextPlayer();

            continue;
        }

        gameManager.NextPlayer();

        Console.Clear();
    }

    var winnerId = gameManager.GetScore().MaxBy(x => x.Value).Key;
    var winner = gameManager.GetPlayerById(winnerId);
    Console.SetCursorPosition(0, MESSAGE_LINE_NUM);
    Console.WriteLine($"{winner.Name} Win!!");

    Console.ReadKey();
}

void DisplayScore(Dictionary<PlayerID, int> score)
{
    Console.SetCursorPosition(0, 15);

    int maxNameLength = gameManager.Players.Max(x => x.Name.Length);
    foreach (var id in Enum.GetValues(typeof(PlayerID)).Cast<PlayerID>().ToList())
    {
        if (id == PlayerID.None)
        {
            continue;
        }

        Console.Write($"{gameManager.GetPlayerById(id).Name,-10} : HP {gameManager.GetPlayerLife(id)} : {score[id]}個 ");
        for (int i = 0; i < score[id]; i++)
        {
            Console.Write("*");
        }
        Console.WriteLine();
    }
    Console.WriteLine($"空き : {score[PlayerID.None]}個");
}

bool CheckUserInput(string input, out int row, out int col)
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
void DrawBoard(List<List<PlayerID>> board)
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








