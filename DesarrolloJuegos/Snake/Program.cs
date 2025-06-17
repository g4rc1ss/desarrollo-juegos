using Snake;

Console.CursorVisible = false;
const char snakeSymbol = '8';
const char boardSymbol = '#';
const char ballSymbol = '0';

int score = 0;
int width = Console.WindowWidth;
int height = Console.WindowHeight;
char[,] board = new char[height, width];
List<Point> snakeParts = new List<Point> { new(height >> 2, width >> 1, Directions.UP) };
(int x, int y) ball = (snakeParts.First().X - 3, snakeParts.First().Y);
Dictionary<Directions, (int, int)> directions = new Dictionary<Directions, (int, int)>
{
    { Directions.UP, (-1, 0) },
    { Directions.DOWN, (1, 0) },
    { Directions.LEFT, (0, -1) },
    { Directions.RIGHT, (0, 1) },
};

Console.Clear();
_ = Task.Run(() =>
{
    while (true)
    {
        ConsoleKeyInfo key = Console.ReadKey(true);
        Directions headDirection = snakeParts.First().Direction;

        if (
            headDirection == Directions.UP && key.Key == ConsoleKey.DownArrow
            || headDirection == Directions.DOWN && key.Key == ConsoleKey.UpArrow
            || headDirection == Directions.LEFT && key.Key == ConsoleKey.RightArrow
            || headDirection == Directions.RIGHT && key.Key == ConsoleKey.LeftArrow
        )
        {
            continue;
        }

        snakeParts.First().Direction = key.Key switch
        {
            ConsoleKey.UpArrow => Directions.UP,
            ConsoleKey.LeftArrow => Directions.LEFT,
            ConsoleKey.RightArrow => Directions.RIGHT,
            ConsoleKey.DownArrow => Directions.DOWN,
            _ => headDirection,
        };
    }
});

while (true)
{
    MoveSnake();
    PrintBoard();

    bool isColision = CheckColision();
    if (isColision)
    {
        break;
    }

    bool isEat = CheckEatBall();
    if (isEat)
    {
        AddPartToSnake();
        CreateBall();
        score++;
    }

    board = new char[height, width];
    AddSnakeToBoard();
    AddBall();

    await Task.Delay((int)TimeSpan.FromSeconds(1).TotalMilliseconds / 20);
}

Console.Clear();
Console.ForegroundColor = ConsoleColor.DarkYellow;
Console.WriteLine($"Game Over: {score}");

void MoveSnake()
{
    for (int i = snakeParts.Count - 1; i >= 0; i--)
    {
        Point snake = snakeParts[i];

        (int dx, int dy) = directions[snake.Direction];
        snake.X += dx;
        snake.Y += dy;
        if (i > 0)
        {
            snake.Direction = snakeParts[i - 1].Direction;
        }
    }
}

bool CheckColision()
{
    Point snakeHead = snakeParts.First();
    bool isBoardLimit =
        (snakeHead.X <= 0 || snakeHead.Y <= 0)
        || (snakeHead.X >= height || snakeHead.Y >= width)
        || board[snakeHead.X, snakeHead.Y] == boardSymbol;

    bool isCollapseWithSnakePart = !isBoardLimit && board[snakeHead.X, snakeHead.Y] == snakeSymbol;

    return isBoardLimit || isCollapseWithSnakePart;
}

void AddSnakeToBoard()
{
    foreach (Point snake in snakeParts)
    {
        board[snake.X, snake.Y] = snakeSymbol;
    }
}

void AddPartToSnake()
{
    Point lastIndex = snakeParts.Last();
    (int dx, int dy) = directions[lastIndex.Direction];

    // ~dx + 1 es lo mismo que dx * -1, pero usando operadores bitwise
    snakeParts.Add(
        new Point(lastIndex.X + (~dx + 1), lastIndex.Y + (~dy + 1), lastIndex.Direction)
    );
}

bool CheckEatBall()
{
    Point? snakeCoord = snakeParts.FirstOrDefault();
    ArgumentNullException.ThrowIfNull(snakeCoord);

    return snakeCoord.X == ball.x && snakeCoord.Y == ball.y;
}

void CreateBall()
{
    int xBall;
    int yBall;

    do
    {
        xBall = new Random().Next(3, height);
        yBall = new Random().Next(3, width);
    } while (snakeParts.Any(x => x.X == xBall && x.Y == yBall));

    ball = (xBall, yBall);
}

void AddBall()
{
    board[ball.x, ball.y] = ballSymbol;
}

void PrintBoard()
{
    Console.Clear();
    for (int x = 0; x < height; x++)
    {
        for (int y = 0; y < width; y++)
        {
            char symbol = board[x, y];
            if (x == 0 || x + 1 == height || y == 0 || y + 1 == width)
            {
                symbol = boardSymbol;
            }

            Console.SetCursorPosition(y, x);
            Console.Write(symbol);
        }
    }
}
