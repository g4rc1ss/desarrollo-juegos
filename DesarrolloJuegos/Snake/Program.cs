using Snake;


Console.CursorVisible = false;
const char SNAKE_SYMBOL = '8';
const char BOARD_SYMBOL = '#';
const char BALL_SYMBOL = '0';

var score = 0;
var width = Console.WindowWidth;
var height = Console.WindowHeight;
var board = new char[height, width];
var snakeParts = new List<Point>
{
    new(height >> 2, width >> 1, Directions.Up),
};
(int x, int y) ball = (snakeParts.First().X - 3, snakeParts.First().Y);
var directions = new Dictionary<Directions, (int, int)>
{
    { Directions.Up, (-1, 0) },
    { Directions.Down, (1, 0) },
    { Directions.Left, (0, -1) },
    { Directions.Right, (0, 1) },
};

Console.Clear();
_ = Task.Run(() =>
{
    while (true)
    {
        var key = Console.ReadKey(true);
        var headDirection = snakeParts.First().Direction;

        if (headDirection == Directions.Up && key.Key == ConsoleKey.DownArrow
            || headDirection == Directions.Down && key.Key == ConsoleKey.UpArrow
            || headDirection == Directions.Left && key.Key == ConsoleKey.RightArrow
            || headDirection == Directions.Right && key.Key == ConsoleKey.LeftArrow)
        {
            continue;
        }

        snakeParts.First().Direction = key.Key switch
        {
            ConsoleKey.UpArrow => Directions.Up,
            ConsoleKey.LeftArrow => Directions.Left,
            ConsoleKey.RightArrow => Directions.Right,
            ConsoleKey.DownArrow => Directions.Down,
            _ => headDirection
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
        var snake = snakeParts[i];

        var (dx, dy) = directions[snake.Direction];
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
    var snakeHead = snakeParts.First();
    var isBoardLimit = (snakeHead.X <= 0 || snakeHead.Y <= 0)
                       || (snakeHead.X >= height || snakeHead.Y >= width)
                       || board[snakeHead.X, snakeHead.Y] == BOARD_SYMBOL;

    var isCollapseWithSnakePart = !isBoardLimit && board[snakeHead.X, snakeHead.Y] == SNAKE_SYMBOL;

    return isBoardLimit || isCollapseWithSnakePart;
}

void AddSnakeToBoard()
{
    foreach (var snake in snakeParts)
    {
        board[snake.X, snake.Y] = SNAKE_SYMBOL;
    }
}

void AddPartToSnake()
{
    var lastIndex = snakeParts.Last();
    var (dx, dy) = directions[lastIndex.Direction];

    // ~dx + 1 es lo mismo que dx * -1, pero usando operadores bitwise
    snakeParts.Add(new Point(lastIndex.X + (~dx + 1), lastIndex.Y + (~dy + 1), lastIndex.Direction));
}

bool CheckEatBall()
{
    var snakeCoord = snakeParts.FirstOrDefault();
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
    board[ball.x, ball.y] = BALL_SYMBOL;
}

void PrintBoard()
{
    Console.Clear();
    for (int x = 0; x < height; x++)
    {
        for (int y = 0; y < width; y++)
        {
            var symbol = board[x, y];
            if (x == 0 || x + 1 == height
                       || y == 0 || y + 1 == width)
            {
                symbol = BOARD_SYMBOL;
            }

            Console.SetCursorPosition(y, x);
            Console.Write(symbol);
        }
    }
}