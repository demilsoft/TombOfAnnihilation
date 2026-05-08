using Godot;

public partial class DungeonMain : Node2D
{
    private const int TileSize = 32;
    private DungeonData _currentDungeon;
    private Vector2I _hiddenWallTile;

    [Export] public DungeonRenderer Renderer;
	[Export] public Camera2D Camera;
    [Export] public PackedScene SpawnMarkerScene;
    [Export] public PackedScene PlayerScene;
    [Export] public PackedScene TestSwitchScene;
    [Export] public PackedScene TestDoorScene;

    public override void _Ready()
    {
        // Generate and load dungeon
        var generator = new DungeonGenerator();
        var serializer = new DungeonSerializer();

        // Set dungeon criteria
        DungeonData generatedDungeon = generator.Generate(
            width: 50,
            height: 32,
            seed: 12345,
            roomCount: 8,
            roomMinSize: 4,
            roomMaxSize: 8
        );

        string savePath = "user://generated_dungeon.json";

        // Debug print to console
        GD.Print(ProjectSettings.GlobalizePath(savePath));

        serializer.SaveDungeon(generatedDungeon, savePath);
        DungeonData loadedDungeon = serializer.LoadDungeon(savePath);

        // Render Dungeon
        if (loadedDungeon != null)
        {
            _currentDungeon = loadedDungeon;

            // Debug print to console
            GD.Print($"Start: {loadedDungeon.StartPosition}");
            GD.Print($"Exit: {loadedDungeon.ExitPosition}");

            // Add player to screen
            Vector2 startWorldPos = new Vector2(
                loadedDungeon.StartPosition.X * TileSize + TileSize / 2,
                loadedDungeon.StartPosition.Y * TileSize + TileSize / 2
            );

            // RENDER OBJECTS AND PLAYER 
            // Add Hidden Wall tile for testing
            _hiddenWallTile = FindHiddenWallCandidate(loadedDungeon);
            if (_hiddenWallTile != new Vector2I(-1, -1))
            {
                loadedDungeon.SetTile(_hiddenWallTile, DungeonTileType.HiddenWall);
                GD.Print($"Hidden wall placed at {_hiddenWallTile}");

                Vector2 hiddenWallWorldPos = new Vector2(
                    _hiddenWallTile.X * TileSize + TileSize / 2,
                    _hiddenWallTile.Y * TileSize + TileSize / 2
                );

                GD.Print($"Hidden wall world position: {hiddenWallWorldPos}");
            }
            else
            {
                GD.PrintErr("No valid hidden wall candidate found.");
            }
            // End add hidden wall


            // Re-render after changing tile to HiddenWall
            Renderer.Render(loadedDungeon);
            // End add hidden wall

            // Add Player
            var player = PlayerScene.Instantiate<PlayerController>();
            player.Position = startWorldPos;
            player.SetDungeon(loadedDungeon);
            AddChild(player);
            // End add player

            // Add Test Switch for interaction testing
            var testSwitch = TestSwitchScene.Instantiate<TestSwitch>();
            testSwitch.Position = startWorldPos + new Vector2(TileSize, 0);
            AddChild(testSwitch);
            testSwitch.DungeonMain = this;
            // End add test switch

            // Add Test Door for interaction testing
            var testDoor = TestDoorScene.Instantiate<TestDoor>();
            testDoor.Position = startWorldPos + new Vector2(TileSize * -3, 0);
            AddChild(testDoor);
            testSwitch.ConnectedDoor = testDoor;
            // End add test door

            // Debug print to console
            GD.Print($"startWorldPos: {startWorldPos}");
            // END RENDER OBJECTS AND PLAYER

            // CAMERA SETUP
            GD.Print($"Camera is null? {Camera == null}");
            // Add camera and functionality
            if (Camera != null)
            {
                Camera.Enabled = true;
                Camera.MakeCurrent();
                Camera.GlobalPosition = startWorldPos;
                Camera.Zoom = new Vector2(.75f, .75f);

                // Move camera under player so it follows automatically
                Camera.GetParent()?.RemoveChild(Camera);
                player.AddChild(Camera);

                // Center camera on player
                Camera.Position = Vector2.Zero;
                Camera.ForceUpdateScroll();

                GD.Print($"Camera node global position: {Camera.GlobalPosition}");
                GD.Print($"Camera screen center: {Camera.GetScreenCenterPosition()}");
                GD.Print($"Camera current: {Camera.IsCurrent()}");

                var spawnMarker = SpawnMarkerScene.Instantiate<Node2D>();
                spawnMarker.Position = startWorldPos;
                AddChild(spawnMarker);
            }
            // End add camera
        }

    }

    public void RevealHiddenWall()
    {
        if (_currentDungeon == null)
            return;

        GD.Print($"Revealing hidden wall at {_hiddenWallTile}");

        _currentDungeon.SetTile(_hiddenWallTile, DungeonTileType.Floor);
        Renderer.Render(_currentDungeon);
    }

    private Vector2I FindHiddenWallCandidate(DungeonData dungeon)
    {
        for (int x = 1; x < dungeon.Width - 1; x++)
        {
            for (int y = 1; y < dungeon.Height - 1; y++)
            {
                Vector2I cell = new Vector2I(x, y);

                if (dungeon.GetTile(cell) != DungeonTileType.Wall)
                    continue;

                bool floorNorth = IsFloorLike(dungeon, cell + new Vector2I(0, -1));
                bool floorSouth = IsFloorLike(dungeon, cell + new Vector2I(0, 1));
                bool floorEast = IsFloorLike(dungeon, cell + new Vector2I(1, 0));
                bool floorWest = IsFloorLike(dungeon, cell + new Vector2I(-1, 0));

                if (floorNorth || floorSouth || floorEast || floorWest)
                    return cell;
            }
        }

        return new Vector2I(-1, -1);
    }

    private bool IsFloorLike(DungeonData dungeon, Vector2I cell)
    {
        if (!dungeon.IsInBounds(cell))
            return false;

        DungeonTileType tile = dungeon.GetTile(cell);

        return tile == DungeonTileType.Floor ||
            tile == DungeonTileType.Start ||
            tile == DungeonTileType.Exit;
    }
}