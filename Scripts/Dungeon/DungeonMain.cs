using Godot;

public partial class DungeonMain : Node2D
{
    private const int TileSize = 32;

    [Export] public DungeonRenderer Renderer;
	[Export] public Camera2D Camera;
    [Export] public PackedScene SpawnMarkerScene;
    [Export] public PackedScene PlayerScene;
    [Export] public PackedScene TestSwitchScene;

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
            Renderer.Render(loadedDungeon);

            // Debug print to console
            GD.Print($"Start: {loadedDungeon.StartPosition}");
            GD.Print($"Exit: {loadedDungeon.ExitPosition}");

            // Add player to screen
            Vector2 startWorldPos = new Vector2(
                loadedDungeon.StartPosition.X * TileSize + TileSize / 2,
                loadedDungeon.StartPosition.Y * TileSize + TileSize / 2
            );

            var player = PlayerScene.Instantiate<PlayerController>();
            player.Position = startWorldPos;
            player.SetDungeon(loadedDungeon);
            AddChild(player);
            // End add player

            // Add Test Switch for interation tesint
            var testSwitch = TestSwitchScene.Instantiate<Node2D>();
            // Place switch 1 tile east of player spawn
            testSwitch.Position = startWorldPos + new Vector2(TileSize, 0);
            AddChild(testSwitch);
            // End add test switch
            
            // Debug print to console
            GD.Print($"startWorldPos: {startWorldPos}");
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
}