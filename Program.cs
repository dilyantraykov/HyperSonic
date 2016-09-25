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
    public int Turn { get; set; }

    public List<Player> Players { get; set; }

    public List<Bomb> Bombs { get; set; }

    public char[,] Field { get; set; }
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

    public void Bomb(int x, int y, params string[] messages)
    {
        Console.WriteLine($"BOMB {x} {y} {messages}");
    }

    public void Move(int x, int y, params string[] messages)
    {
        Console.WriteLine($"MOVE {x} {y} {messages}");
    }
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

public class Program
{
    static void Main(string[] args)
    {
        string[] inputs;
        inputs = Console.ReadLine().Split(' ');
        int width = int.Parse(inputs[0]);
        int height = int.Parse(inputs[1]);
        int myId = int.Parse(inputs[2]);
        List<Entity> entities = new List<Entity>();
        var field = new char[Constants.FieldWidth, Constants.FieldHeight];
        var context = new GameContext();

        // game loop
        while (true)
        {
            for (int i = 0; i < height; i++)
            {
                string row = Console.ReadLine();
                for (var j = 0; j < row.Length; j++)
                {
                    field[i, j] = row[j];
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

                var entity = CreateEntity(entityType, owner, x, y, param1, param2);
                entities.Add(entity);
            }

            ProcessTurn(context);
        }
    }

    private static void ProcessTurn(object context)
    {
        throw new NotImplementedException();
    }

    private static Entity CreateEntity(int entityType, int owner, int x, int y, int param1, int param2)
    {
        if (entityType == 0)
        {
            return new Player()
            {
                Id = owner,
                Point = new Point() { X = x, Y = y },
                BombsLeft = param1,
                ExplosionRange = param2
            };
        }
        else if (entityType == 1)
        {
            return new Bomb()
            {
                OwnerId = owner,
                Point = new Point() { X = x, Y = y },
                RoundsUntilExplosion = param1,
                ExplosionRange = param2
            };
        }
        else
        {
            return null;
        }
    }
}