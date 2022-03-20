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

    public class Shuhei_CPU : IPlayer
    {
        public PlayerID ID { get; set; }
        public string Name { get; } = "Shuhei_CPU";
        public PlayerType Type { get; } = PlayerType.CPU;

        public Shuhei_CPU()
        {
        }

        public Shuhei_CPU(string Name)
        {
            this.Name = Name;
        }

        public (int row, int column) Place(List<List<PlayerID>> board)
        {
            throw new NotImplementedException();
        }
    }
}
