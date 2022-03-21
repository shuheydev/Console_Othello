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

        const int LIFE_COUNT = 3;

        public List<List<PlayerID>> Board { get; private set; }

        public bool CheckGameFinished()
        {
            //置く場所がない
            if (!Board.SelectMany(x => x).Any(x => x == PlayerID.None))
            {
                return true;
            }

            //ターンの最後でライフが残っている人が1人
            if (Count % Players.Count == Players.Count - 1 && PlayerLifeList.Where(x => x.Value > 0).Count() == 1)
            {
                return true;
            }



            return true;
        }

        public IPlayer GetPlayerById(PlayerID id)
        {
            return Players.FirstOrDefault(x => x.ID == id);
        }


        public int Count { get; private set; } = 0;

        public List<IPlayer> Players = new List<IPlayer>();

        public IPlayer CurrentPlayer { get; set; }

        public GameManager()
        {
        }

        public List<List<PlayerID>> InitBoard()
        {
            Board = new List<List<PlayerID>>();
            for (int row = 0; row < ROWSIZE; row++)
            {
                Board.Add(new List<PlayerID>());
                for (int col = 0; col < COLSIZE; col++)
                {
                    Board[row].Add(PlayerID.None);
                }
            }

            Board[5][5] = PlayerID.Player1;
            Board[4][3] = PlayerID.Player1;
            Board[3][4] = PlayerID.Player1;
            Board[6][6] = PlayerID.Player1;

            Board[5][4] = PlayerID.Player2;
            Board[3][5] = PlayerID.Player2;
            Board[4][6] = PlayerID.Player2;
            Board[6][3] = PlayerID.Player2;

            Board[4][5] = PlayerID.Player3;
            Board[5][3] = PlayerID.Player3;
            Board[6][4] = PlayerID.Player3;
            Board[3][6] = PlayerID.Player3;

            Board[4][4] = PlayerID.Player4;
            Board[5][6] = PlayerID.Player4;
            Board[6][5] = PlayerID.Player4;
            Board[3][3] = PlayerID.Player4;

            return Board;
        }

        internal void DecreaseLife(PlayerID id)
        {
            PlayerLifeList[id]--;
        }

        public bool IsFinish()
        {
            //置くところがない
            return !Board.SelectMany(x => x).Any(x => x == PlayerID.None);
        }

        public Dictionary<PlayerID, int> GetScore()
        {
            var score = new Dictionary<PlayerID, int>();
            foreach (var id in Enum.GetValues(typeof(PlayerID)).Cast<PlayerID>().ToList())
            {
                score.Add(id, 0);
            }

            foreach (var g in Board.SelectMany(x => x).GroupBy(x => x))
            {
                score[g.Key] = g.Count();
            }

            return score;
        }

        public void SetPlayerOrder(int startIndex)
        {
            Players[(startIndex + 0) % 4].ID = PlayerID.Player1;
            Players[(startIndex + 1) % 4].ID = PlayerID.Player2;
            Players[(startIndex + 2) % 4].ID = PlayerID.Player3;
            Players[(startIndex + 3) % 4].ID = PlayerID.Player4;

            CurrentPlayer = Players[0];
        }

        public Dictionary<PlayerID, int> PlayerLifeList = new Dictionary<PlayerID, int>();
        public void InitPlayerLifeList()
        {
            foreach (var id in Enum.GetValues(typeof(PlayerID)).Cast<PlayerID>().ToList())
            {
                PlayerLifeList[id] = LIFE_COUNT;
            }
        }
        public int GetPlayerLife(PlayerID id)
        {
            return PlayerLifeList[id];
        }


        public bool PlaceStone(int row, int col)
        {
            if (!CheckPlaceStone(row, col))
            {
                return false;
            }

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

        public string GetPlayerMark(PlayerID id)
        {
            return id switch
            {
                PlayerID.None => "□",
                PlayerID.Player1 => "●",
                PlayerID.Player2 => "★",
                PlayerID.Player3 => "▲",
                PlayerID.Player4 => "■",
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
            List<(int row, int col)> result = new List<(int row, int col)>();

            //1.自分の石に当たるか
            //2.盤の外に出るか
            //3.何も置かれていないマスに当たるか
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

                //盤の外に出た場合
                if (!(row is >= 0 and < ROWSIZE) || !(col is >= 0 and < COLSIZE))
                {
                    break;
                }

                //自分の石に当たった場合
                if (Board[row][col] == CurrentPlayer.ID)
                {
                    return result;
                }
                result.Add((row, col));
            } while (Board[row][col] != PlayerID.None);

            result.Clear();
            return result;
        }

        bool CheckPlaceStone(int row, int col)
        {
            if (row >= ROWSIZE)
            {
                return false;
            }
            if (col >= COLSIZE)
            {
                return false;
            }

            if (Board[row][col] != PlayerID.None)
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

    public enum PlayerID
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
