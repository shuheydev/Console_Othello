using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Othello
{
    internal class GameManager
    {
        const int ROWSIZE = 10;
        const int COLSIZE = 10;

        public int Count { get; private set; } = 0;

        public List<IPlayer> Players = new List<IPlayer>();

        public IPlayer CurrentPlayer { get; set; }

        public List<List<ID>> Board { get; private set; }

        public GameManager()
        {
        }

        public List<List<ID>> InitBoard()
        {
            Board = new List<List<ID>>();
            for (int row = 0; row < ROWSIZE; row++)
            {
                Board.Add(new List<ID>());
                for (int col = 0; col < COLSIZE; col++)
                {
                    Board[row].Add(ID.None);
                }
            }

            Board[5][5] = ID.Player1;
            Board[4][3] = ID.Player1;
            Board[3][4] = ID.Player1;
            Board[6][6] = ID.Player1;

            Board[5][4] = ID.Player2;
            Board[3][5] = ID.Player2;
            Board[4][6] = ID.Player2;
            Board[6][3] = ID.Player2;

            Board[4][5] = ID.Player3;
            Board[5][3] = ID.Player3;
            Board[6][4] = ID.Player3;
            Board[3][6] = ID.Player3;

            Board[4][4] = ID.Player4;
            Board[5][6] = ID.Player4;
            Board[6][5] = ID.Player4;
            Board[3][3] = ID.Player4;

            return Board;
        }
        public void SetPlayerOrder(int startIndex)
        {
            Players[(startIndex + 0) % 4].ID = ID.Player1;
            Players[(startIndex + 1) % 4].ID = ID.Player2;
            Players[(startIndex + 2) % 4].ID = ID.Player3;
            Players[(startIndex + 3) % 4].ID = ID.Player4;

            CurrentPlayer = Players[0];
        }

        public bool PlaceStone(int row, int col)
        {
            CheckInput(row, col);

            //ひっくり返せる石をピックアップする
            var reverseTargetStones = CheckReverseStones(row, col);

            if (!reverseTargetStones.Any())
            {
                return false;
            }

            //石を置く
            Board[row][col] = CurrentPlayer.ID;
            //ひっくり返す
            ReverseStones(reverseTargetStones);

            return true;
        }

        public void NextPlayer()
        {
            Count++;
            CurrentPlayer = Players[Count % Players.Count];
        }

        public string GetPlayerMark(ID id)
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

        void ReverseStones(List<(int row, int col)> reverseStones)
        {
            foreach (var stone in reverseStones)
            {
                Board[stone.row][stone.col] = CurrentPlayer.ID;
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

                if (Board[row][col] == CurrentPlayer.ID)
                {
                    return rl;
                }
                rl.Add((row, col));
            } while (row >= 0 && Board[row][col] != ID.None);

            //ひっくり返す石が見つからなかった
            rl.Clear();
            return rl;
        }

        bool CheckInput(int row, int col)
        {
            if (row >= ROWSIZE)
            {
                return false;
            }
            if (col >= COLSIZE)
            {
                return false;
            }

            if (Board[row][col] != ID.None)
            {
                return false;
            }

            return true;
        }
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
}
