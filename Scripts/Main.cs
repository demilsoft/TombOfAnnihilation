using Godot;
using System;

public partial class Main : Node2D
{

    [Export] public PackedScene DungeonMainScene;

    public override void _Ready()
    {
        // var dungeon = new DungeonData(20, 20, 1234);

        // dungeon.SetTile(new Vector2I(5, 5), DungeonTileType.Floor);
        // dungeon.StartPosition = new Vector2I(1, 1);
        // dungeon.ExitPosition = new Vector2I(18, 18);

        // GD.Print($"Tile at 5,5 = {dungeon.GetTile(new Vector2I(5, 5))}");
        // GD.Print($"Start = {dungeon.StartPosition}, Exit = {dungeon.ExitPosition}");
    
        var dungeonInstance = DungeonMainScene.Instantiate();
        AddChild(dungeonInstance);
    }
}