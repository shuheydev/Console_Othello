using Console_Othello.Extensions;

namespace Console_Othello
{
    public class Human : IPlayer
    {
        public PlayerID ID { get; set; }
        public string Name { get; } = "Human";
        public PlayerType Type { get; } = PlayerType.Human;

        public Human()
        {
        }

        public Human(string Name)
        {
            this.Name = Name;
        }

        public (int row, int column) Place(List<List<PlayerID>> board)
        {
            throw new NotImplementedException();
        }
    }

    public class Random_CPU : IPlayer
    {
        public PlayerID ID { get; set; }
        public string Name { get; } = "Shuhei_CPU";
        public PlayerType Type { get; } = PlayerType.CPU;

        public Random_CPU()
        {
        }

        public Random_CPU(string Name)
        {
            this.Name = Name;
        }

        int _rowSize;
        int _colSize;
        public (int row, int column) Place(List<List<PlayerID>> board)
        {
            _rowSize = board.Count;
            _colSize = board[0].Count;

            var candidatePlace = new List<(int row_candidate, int col_candidate)>();
            //ランダムな指し方。
            for (int row = 0; row < _rowSize; row++)
            {
                for (int col = 0; col < _colSize; col++)
                {
                    if (board[row][col] != PlayerID.None)
                    {
                        continue;
                    }

                    if (CheckReverseStones(row, col, board).Any())
                    {
                        candidatePlace.Add((row, col));
                    }
                }
            }

            return candidatePlace.Random();
        }


        List<(int row, int col)> CheckReverseStones(int row, int col, List<List<PlayerID>> board)
        {
            //ひっくり返す石をチェックする
            List<(int row, int col)> reverseStones = new List<(int row, int col)>();

            foreach (var direction in Enum.GetValues(typeof(CheckDirection)).Cast<CheckDirection>().ToList())
            {
                reverseStones.AddRange(CheckReverse(row, col, direction, board));
            }

            return reverseStones;
        }
        List<(int row, int col)> CheckReverse(int row, int col, CheckDirection direction, List<List<PlayerID>> board)
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
                if (!(row >= 0 && row < _rowSize) || !(col >= 0 && col < _colSize))
                {
                    break;
                }

                //自分の石に当たった場合
                if (board[row][col] == this.ID)
                {
                    return result;
                }
                result.Add((row, col));
            } while (board[row][col] != PlayerID.None);

            result.Clear();
            return result;
        }
    }
}
