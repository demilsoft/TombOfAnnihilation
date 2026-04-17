using Godot;
using System.Collections.Generic;

public class DungeonSaveData
{
    public int Width { get; set; }
    public int Height { get; set; }
    public int Seed { get; set; }

    public int StartX { get; set; }
    public int StartY { get; set; }

    public int ExitX { get; set; }
    public int ExitY { get; set; }

    public List<int> Tiles { get; set; } = new List<int>();
}