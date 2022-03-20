using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Othello
{
    public class Human : IPlayer
    {
        public ID ID { get; set; }
        public string Name { get; } = "Human";
        public PlayerType Type { get; } = PlayerType.Human;

        public Human()
        {

        }

        public Human(string Name)
        {
            this.Name = Name;
        }


        public (int row, int column) PlaceStone(List<List<ID>> board)
        {
            throw new NotImplementedException();
        }
    }

    public class Haruki : IPlayer
    {
        public ID ID { get; set; }
        public string Name { get; } = "Haruki";
        public PlayerType Type { get; } = PlayerType.Human;

        public (int row, int column) PlaceStone(List<List<ID>> board)
        {
            throw new NotImplementedException();
        }
    }
}
