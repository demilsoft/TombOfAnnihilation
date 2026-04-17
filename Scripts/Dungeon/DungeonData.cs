using Godot;
using System.Collections.Generic;

public partial class DungeonData : Resource
{
    public int Width { get; set; }
    public int Height { get; set; }
    public int Seed { get; set; }

    public DungeonTileType[,] Tiles { get; set; }

    public Vector2I StartPosition { get; set; }
    public Vector2I ExitPosition { get; set; }

    public DungeonData()
    {
    }

    public DungeonData(int width, int height, int seed)
    {
        Width = width;
        Height = height;
        Seed = seed;
        Tiles = new DungeonTileType[width, height];
    }

    public bool IsInBounds(Vector2I pos)
    {
        return pos.X >= 0 && pos.X < Width && pos.Y >= 0 && pos.Y < Height;
    }

    public DungeonTileType GetTile(Vector2I pos)
    {
        if (!IsInBounds(pos))
            return DungeonTileType.Empty;

        return Tiles[pos.X, pos.Y];
    }

    public void SetTile(Vector2I pos, DungeonTileType tileType)
    {
        if (!IsInBounds(pos))
            return;

        Tiles[pos.X, pos.Y] = tileType;
    }

    public List<int> GetFlatTileList()
    {
        List<int> flatTiles = new List<int>();

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                flatTiles.Add((int)Tiles[x, y]);
            }
        }

        return flatTiles;
    }

    public void SetFlatTileList(List<int> flatTiles)
    {
        Tiles = new DungeonTileType[Width, Height];

        int index = 0;

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Tiles[x, y] = (DungeonTileType)flatTiles[index];
                index++;
            }
        }
    }
}