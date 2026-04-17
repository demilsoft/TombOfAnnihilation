using Godot;

public partial class DungeonMain : Node2D
{
    [Export] public DungeonRenderer Renderer;
	[Export] public Camera2D Camera;
    [Export] public PackedScene SpawnMarkerScene;

    public override void _Ready()
    {
        var generator = new DungeonGenerator();
        var serializer = new DungeonSerializer();

        DungeonData generatedDungeon = generator.Generate(
            width: 50,
            height: 32,
            seed: 12345,
            roomCount: 8,
            roomMinSize: 4,
            roomMaxSize: 8
        );

        string savePath = "user://generated_dungeon.json";
        GD.Print(ProjectSettings.GlobalizePath(savePath));

        serializer.SaveDungeon(generatedDungeon, savePath);

        DungeonData loadedDungeon = serializer.LoadDungeon(savePath);

        if (loadedDungeon != null)
        {
            Renderer.Render(loadedDungeon);

            GD.Print($"Start: {loadedDungeon.StartPosition}");
            GD.Print($"Exit: {loadedDungeon.ExitPosition}");

            Vector2 startWorldPos = new Vector2(
                loadedDungeon.StartPosition.X * 16 + 8,
                loadedDungeon.StartPosition.Y * 16 + 8
            );

            GD.Print($"startWorldPos: {startWorldPos}");
            GD.Print($"Camera is null? {Camera == null}");

            if (Camera != null)
            {
                Camera.Enabled = true;
                Camera.MakeCurrent();
                Camera.GlobalPosition = startWorldPos;
                Camera.Zoom = new Vector2(1.25f, 1.25f);
                Camera.ForceUpdateScroll();

                GD.Print($"Camera node global position: {Camera.GlobalPosition}");
                GD.Print($"Camera screen center: {Camera.GetScreenCenterPosition()}");
                GD.Print($"Camera current: {Camera.IsCurrent()}");

                var spawnMarker = SpawnMarkerScene.Instantiate<Node2D>();
                spawnMarker.Position = startWorldPos;
                AddChild(spawnMarker);
            }
        }
    }
}