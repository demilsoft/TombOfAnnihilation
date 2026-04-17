using Godot;

public partial class DungeonRenderer : Node
{
    [Export] public TileMapLayer FloorLayer;
    [Export] public TileMapLayer WallLayer;

    [Export] public int FloorSourceId = 0;
    [Export] public Vector2I FloorAtlasCoords = new Vector2I(0, 0);

    [Export] public int WallSourceId = 0;
    [Export] public Vector2I WallAtlasCoords = new Vector2I(1, 0);

	[Export] public int StartSourceId = 0;
    [Export] public Vector2I StartAtlasCoords = new Vector2I(2, 0);

    [Export] public int ExitSourceId = 0;
    [Export] public Vector2I ExitAtlasCoords = new Vector2I(3, 0);


    public void Render(DungeonData dungeon)
    {
        FloorLayer.Clear();
        WallLayer.Clear();

        for (int x = 0; x < dungeon.Width; x++)
        {
            for (int y = 0; y < dungeon.Height; y++)
            {
                Vector2I cell = new Vector2I(x, y);
                DungeonTileType tile = dungeon.GetTile(cell);

                switch (tile)
                {
                    case DungeonTileType.Floor:
                        FloorLayer.SetCell(cell, FloorSourceId, FloorAtlasCoords);
                        break;

                    case DungeonTileType.Start:
    					GD.Print($"RENDER START at {cell}");
                        FloorLayer.SetCell(cell, StartSourceId, StartAtlasCoords);
                        break;

                    case DungeonTileType.Exit:
    					GD.Print($"RENDER EXIT at {cell}");
                        FloorLayer.SetCell(cell, ExitSourceId, ExitAtlasCoords);
                        break;

                    case DungeonTileType.Wall:
                        WallLayer.SetCell(cell, WallSourceId, WallAtlasCoords);
                        break;
                }
            }
        }
    }
}