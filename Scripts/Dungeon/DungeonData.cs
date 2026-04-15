using Godot;

public partial class DungeonData : Resource
{
    public int Width { get; set; }
    public int Height { get; set; }
    public int Seed { get; set; }

    public DungeonTileType[,] Tiles { get; set; }

    public Vector2I StartPosition { get; set; }
    public Vector2I ExitPosition { get; set; }

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
}