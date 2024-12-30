namespace Snake;

public class Point(int x, int y, Directions direction)
{
    public int X { get; set; } = x;
    public int Y { get; set; } = y;
    public Directions Direction { get; set; } = direction;
}