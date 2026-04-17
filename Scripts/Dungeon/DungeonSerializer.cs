using Godot;
using System;
using System.Text.Json;

public partial class DungeonSerializer : GodotObject
{
    public void SaveDungeon(DungeonData dungeon, string filePath)
    {
        DungeonSaveData saveData = new DungeonSaveData
        {
            Width = dungeon.Width,
            Height = dungeon.Height,
            Seed = dungeon.Seed,
            StartX = dungeon.StartPosition.X,
            StartY = dungeon.StartPosition.Y,
            ExitX = dungeon.ExitPosition.X,
            ExitY = dungeon.ExitPosition.Y,
            Tiles = dungeon.GetFlatTileList()
        };

        string json = JsonSerializer.Serialize(saveData, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        using FileAccess file = FileAccess.Open(filePath, FileAccess.ModeFlags.Write);
        file.StoreString(json);

        GD.Print($"Dungeon saved to: {filePath}");
    }

    public DungeonData LoadDungeon(string filePath)
    {
        if (!FileAccess.FileExists(filePath))
        {
            GD.PrintErr($"Save file not found: {filePath}");
            return null;
        }

        using FileAccess file = FileAccess.Open(filePath, FileAccess.ModeFlags.Read);
        string json = file.GetAsText();

        DungeonSaveData saveData = JsonSerializer.Deserialize<DungeonSaveData>(json);

        if (saveData == null)
        {
            GD.PrintErr("Failed to deserialize dungeon save data.");
            return null;
        }

        DungeonData dungeon = new DungeonData(saveData.Width, saveData.Height, saveData.Seed);

        dungeon.StartPosition = new Vector2I(saveData.StartX, saveData.StartY);
        dungeon.ExitPosition = new Vector2I(saveData.ExitX, saveData.ExitY);
        dungeon.SetFlatTileList(saveData.Tiles);

        GD.Print($"Dungeon loaded from: {filePath}");

        return dungeon;
    }
}