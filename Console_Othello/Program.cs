// See https://aka.ms/new-console-template for more information

const int ROWSIZE = 10;
const int COLSIZE = 10;


const int PLAYERNAME_LINE_NUM = 0;
const int MESSAGE_LINE_NUM = 1;
const int BOARD_LINE_NUM = 2;


List<IPlayer> Players = new List<IPlayer>();

Console.Clear();

List<List<ID>> board;

board = InitBoard();

Players.Add(new Shuhei());
Players.Add(new Shuhei());
Players.Add(new Shuhei());
Players.Add(new Shuhei());

SetPlayerId(Players);

int count = 0;
//ID currentPlayerID = ID.None;

IPlayer currentPlayer = null;

while (true)
{
    currentPlayer = Players[(count % 4)];
    //currentPlayerID = currentPlayer.ID;


    Console.SetCursorPosition(0, PLAYERNAME_LINE_NUM);
    Console.Write($"{currentPlayer.Name} : {GetPlayerMark(currentPlayer.ID)}");

    Console.SetCursorPosition(0, BOARD_LINE_NUM);
    DrawBoard(board);

    Console.Write("Input[row,column]:");
    string input = Console.ReadLine();

    //入力が正しいかどうか
    if (!CheckInput(input))
    {
        Console.Clear();
        Console.SetCursorPosition(0, MESSAGE_LINE_NUM);
        Console.Write("入力が正しくありません");

        continue;
    }

    var splittedInput = input.Split(',');


    int.TryParse(splittedInput[0], out int row);
    int.TryParse(splittedInput[1], out int col);


    //ひっくり返せる石をピックアップする
    var reverseStones = CheckReverseStones(row, col);

    if (!reverseStones.Any())
    {
        continue;
    }

    //石を置く
    board[row][col] = currentPlayer.ID;
    //ひっくり返す
    Reverse(reverseStones);

    count++;
    Console.Clear();
}

void SetPlayerId(List<IPlayer> players)
{
    players[0].ID = ID.Player1;
    players[1].ID = ID.Player2;
    players[2].ID = ID.Player3;
    players[3].ID = ID.Player4;
}

void Reverse(List<(int row, int col)> reverseStones)
{
    foreach (var stone in reverseStones)
    {
        board[stone.row][stone.col] = currentPlayer.ID;
    }
}

List<(int row, int col)> CheckReverseStones(int row, int col)
{
    //ひっくり返す石をチェックする
    List<(int row, int col)> reverseStones = new List<(int row, int col)>();

    foreach (var direction in Enum.GetValues(typeof(CheckDirection)).Cast<CheckDirection>().ToList())
    {
        reverseStones.AddRange(CheckReverse(row, col, direction));
    }

    return reverseStones;
}

List<(int row, int col)> CheckReverse(int row, int col, CheckDirection direction)
{
    List<(int row, int col)> rl = new List<(int row, int col)>();
    do
    {
        switch (direction)
        {
            case CheckDirection.Top:
                row--;
                break;
            case CheckDirection.TopRight:
                row--;
                col++;
                break;
            case CheckDirection.Right:
                col++;
                break;
            case CheckDirection.BottomRight:
                row++;
                col++;
                break;
            case CheckDirection.Bottom:
                row++;
                break;
            case CheckDirection.BottomLeft:
                row++;
                col--;
                break;
            case CheckDirection.Left:
                col--;
                break;
            case CheckDirection.TopLeft:
                row--;
                col--;
                break;
        }

        if (!(row is >= 0 and < ROWSIZE) || !(col is >= 0 and < COLSIZE))
        {
            break;
        }

        if (board[row][col] == currentPlayer.ID)
        {
            return rl;
        }
        rl.Add((row, col));
    } while (row >= 0 && board[row][col] != ID.None);

    //ひっくり返す石が見つからなかった
    rl.Clear();
    return rl;
}

bool CheckInput(string input)
{
    if (string.IsNullOrEmpty(input))
    {
        return false;
    }
    var splitted = input.Split(',');
    if (splitted.Length != 2)
    {
        return false;
    }

    if (!int.TryParse(splitted[0], out int row))
    {
        return false;
    }
    if (!int.TryParse(splitted[1], out int col))
    {
        return false;
    }

    if (row >= ROWSIZE)
    {
        return false;
    }
    if (col >= COLSIZE)
    {
        return false;
    }

    if (board[row][col] != ID.None)
    {
        return false;
    }

    return true;
}

void DrawBoard(List<List<ID>> board)
{
    Console.Write("  ");
    Console.WriteLine(String.Join(" ", Enumerable.Range(0, ROWSIZE)));
    for (int i = 0; i < board.Count; i++)
    {
        var row = board[i].Select(x => GetPlayerMark(x));
        Console.Write($"{i,-2}");
        Console.WriteLine(string.Join("", row));
    }
}

string GetPlayerMark(ID id)
{
    return id switch
    {
        ID.None => "□",
        ID.Player1 => "●",
        ID.Player2 => "★",
        ID.Player3 => "▲",
        ID.Player4 => "■",
        _ => throw new NotImplementedException(),
    };
}

List<List<ID>> InitBoard()
{
    var board = new List<List<ID>>();
    for (int row = 0; row < ROWSIZE; row++)
    {
        board.Add(new List<ID>());
        for (int col = 0; col < COLSIZE; col++)
        {
            board[row].Add(ID.None);
        }
    }

    board[5][5] = ID.Player1;
    board[4][3] = ID.Player1;
    board[3][4] = ID.Player1;
    board[6][6] = ID.Player1;

    board[5][4] = ID.Player2;
    board[3][5] = ID.Player2;
    board[4][6] = ID.Player2;
    board[6][3] = ID.Player2;

    board[4][5] = ID.Player3;
    board[5][3] = ID.Player3;
    board[6][4] = ID.Player3;
    board[3][6] = ID.Player3;

    board[4][4] = ID.Player4;
    board[5][6] = ID.Player4;
    board[6][5] = ID.Player4;
    board[3][3] = ID.Player4;

    return board;
}

public enum CheckDirection
{
    Top,
    TopRight,
    Right,
    BottomRight,
    Bottom,
    BottomLeft,
    Left,
    TopLeft,
}

public enum ID
{
    None,
    Player1,
    Player2,
    Player3,
    Player4,
}


public enum PlayerType
{
    Human,
    CPU,
}
public class Shuhei : IPlayer
{
    public ID ID { get; set; }
    public string Name { get; } = "Shuhei";
    public PlayerType Type { get; } = PlayerType.Human;

    public (int row, int column) PlaceStone(List<List<ID>> board)
    {
        throw new NotImplementedException();
    }
}




