// See https://aka.ms/new-console-template for more information


public interface IPlayer
{
    public ID ID { get; set; }
    public string Name { get;}
    public PlayerType Type { get;}

    (int row, int column) PlaceStone(List<List<ID>> board);
}