using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class Constants
{
    public const byte FieldWidth = 13;
    public const byte FieldHeight = 13;
    public const char FloorCellSymbol = '.';
    public const char BoxCellSymbol = '0';
    public const byte MinBoxCount = 30;
    public const byte MaxBoxCount = 65;
    public const byte BombsPerTern = 1;
    public const byte BombTimerRounds = 8;
    public const byte BombRange = 3;
    public const byte NumberOfRounds = 200;
}

public class GameContext
{
    public int MyId { get; set; }

    public int Turn { get; set; }

    public List<Player> Players { get; set; }

    public Player MyPlayer
    {
        get
        {
            return this.Players.First(p => p.Id == this.MyId);
        }
    }

    public List<Entity> Boxes { get; set; }

    public List<Bomb> Bombs { get; set; }

    public CellType[,] Field { get; set; }
}

public class Entity
{
    public Point Point { get; set; }
}

public class Player : Entity
{
    public int Id { get; set; }

    public int BombsLeft { get; set; }

    public int ExplosionRange { get; set; }
}

public class Bomb : Entity
{
    public int OwnerId { get; set; }

    public int RoundsUntilExplosion { get; set; }

    public int ExplosionRange { get; set; }
}

public class Point
{
    public int X { get; set; }
    public int Y { get; set; }
}

public enum CellType
{
    EmptyCell = 0,
    Box = 1
}

public class Program
{
    static void Main(string[] args)
    {
        string[] inputs;
        inputs = Console.ReadLine().Split(' ');
        int width = int.Parse(inputs[0]);
        int height = int.Parse(inputs[1]);
        int myId = int.Parse(inputs[2]);
        var players = new List<Player>();
        var bombs = new List<Bomb>();
        var boxes = new List<Entity>();
        var field = new CellType[Constants.FieldWidth, Constants.FieldHeight];
        var context = new GameContext() { MyId = myId, Turn = Constants.NumberOfRounds };

        // game loop
        while (true)
        {
            for (int i = 0; i < height; i++)
            {
                string row = Console.ReadLine();
                for (var j = 0; j < row.Length; j++)
                {
                    switch (row[j])
                    {
                        case '.':
                            field[i, j] = CellType.EmptyCell;
                            break;
                        case '0':
                            field[i, j] = CellType.Box;
                            boxes.Add(new Entity()
                            {
                                Point = new Point() { X = i, Y = j }
                            });
                            break;
                        default:
                            break;
                    }
                }
            }

            int entitiesCount = int.Parse(Console.ReadLine());
            for (int i = 0; i < entitiesCount; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                int entityType = int.Parse(inputs[0]);
                int owner = int.Parse(inputs[1]);
                int x = int.Parse(inputs[2]);
                int y = int.Parse(inputs[3]);
                int param1 = int.Parse(inputs[4]);
                int param2 = int.Parse(inputs[5]);

                if (entityType == 0)
                {
                    players.Add(new Player()
                    {
                        Id = owner,
                        Point = new Point() { X = x, Y = y },
                        BombsLeft = param1,
                        ExplosionRange = param2
                    });
                }
                else if (entityType == 1)
                {
                    bombs.Add(new Bomb()
                    {
                        OwnerId = owner,
                        Point = new Point() { X = x, Y = y },
                        RoundsUntilExplosion = param1,
                        ExplosionRange = param2
                    });
                }
            }

            context.Boxes = boxes;
            context.Bombs = bombs;
            context.Players = players;
            context.Field = field;

            ProcessTurn(context);

            context.Turn--;
        }
    }

    private static void ProcessTurn(GameContext context)
    {
        foreach (var box in context.Boxes)
            if (IsCloseEnoughToBomb(context.MyPlayer.Point, box.Point))
            {
                Bomb(box.Point.X, box.Point.Y, string.Format($"BOMB {box.Point.X} {box.Point.Y}"));
                return;
            }

        Point closestBomb = GetClosestBox(context.MyPlayer, context.Boxes);
        Bomb(closestBomb.X, closestBomb.Y, string.Format($"MOVE {closestBomb.X} {closestBomb.Y}"));
        return;
    }

    private static Point GetClosestBox(Player player, IEnumerable<Entity> boxes)
    {
        var minDistance = int.MaxValue;
        Point closestPoint = null;
        foreach (var box in boxes)
        {
            if (GetDistance(player.Point, box.Point) < minDistance)
            {
                closestPoint = box.Point;
                minDistance = box.Point.X + box.Point.Y;
            }
        }

        return closestPoint;
    }

    private static int GetDistance(Point p1, Point p2)
    {
        return Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
    }

    private static bool IsCloseEnoughToBomb(Point playerPoint, Point boxPoint)
    {
        var distanceX = Math.Abs(playerPoint.X - boxPoint.X);
        var distanceY = Math.Abs(playerPoint.Y - boxPoint.Y);
        if ((distanceY == 0 && distanceX <= Constants.BombRange - 1) ||
            (distanceX == 0 && distanceY <= Constants.BombRange - 1))
        {
            return true;
        }

        return false;
    }

    public static void Bomb(int x, int y, params string[] messages)
    {
        Console.WriteLine($"BOMB {x} {y} {messages}");
    }

    public static void Move(int x, int y, params string[] messages)
    {
        Console.WriteLine($"MOVE {x} {y} {messages}");
    }
}