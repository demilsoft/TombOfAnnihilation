using Godot;
using System;
using System.Collections.Generic;

public partial class DungeonGenerator : GodotObject
{
    private RandomNumberGenerator _rng = new RandomNumberGenerator();

    public DungeonData Generate(int width, int height, int seed, int roomCount, int roomMinSize, int roomMaxSize)
    {
        var dungeon = new DungeonData(width, height, seed);
        _rng.Seed = (ulong)seed;

        FillWithWalls(dungeon);

        var rooms = new List<Rect2I>();

        for (int i = 0; i < roomCount; i++)
        {
            int roomWidth = _rng.RandiRange(roomMinSize, roomMaxSize);
            int roomHeight = _rng.RandiRange(roomMinSize, roomMaxSize);

            int roomX = _rng.RandiRange(1, width - roomWidth - 2);
            int roomY = _rng.RandiRange(1, height - roomHeight - 2);

            var newRoom = new Rect2I(roomX, roomY, roomWidth, roomHeight);

            if (RoomOverlapsAny(newRoom, rooms))
                continue;

            CarveRoom(dungeon, newRoom);

            if (rooms.Count > 0)
            {
                Vector2I prevCenter = GetRoomCenter(rooms[rooms.Count - 1]);
                Vector2I newCenter = GetRoomCenter(newRoom);
                CarveCorridor(dungeon, prevCenter, newCenter);
            }

            rooms.Add(newRoom);
        }

        if (rooms.Count > 0)
        {
            dungeon.StartPosition = GetRoomCenter(rooms[0]);
            dungeon.ExitPosition = GetRoomCenter(rooms[rooms.Count - 1]);

            dungeon.SetTile(dungeon.StartPosition, DungeonTileType.Start);
            dungeon.SetTile(dungeon.ExitPosition, DungeonTileType.Exit);
        }

		// Debug output
		GD.Print($"Start: {dungeon.StartPosition}");
		GD.Print($"Start Tile: {dungeon.GetTile(dungeon.StartPosition)}");
		GD.Print($"Exit: {dungeon.ExitPosition}");
		GD.Print($"Exit Tile: {dungeon.GetTile(dungeon.ExitPosition)}");

        return dungeon;
    }

    private void FillWithWalls(DungeonData dungeon)
    {
        for (int x = 0; x < dungeon.Width; x++)
        {
            for (int y = 0; y < dungeon.Height; y++)
            {
                dungeon.SetTile(new Vector2I(x, y), DungeonTileType.Wall);
            }
        }
    }

    private void CarveRoom(DungeonData dungeon, Rect2I room)
    {
        for (int x = room.Position.X; x < room.End.X; x++)
        {
            for (int y = room.Position.Y; y < room.End.Y; y++)
            {
                dungeon.SetTile(new Vector2I(x, y), DungeonTileType.Floor);
            }
        }
    }

    private void CarveCorridor(DungeonData dungeon, Vector2I from, Vector2I to)
    {
        int x = from.X;
        int y = from.Y;

        while (x != to.X)
        {
            dungeon.SetTile(new Vector2I(x, y), DungeonTileType.Floor);
            x += Math.Sign(to.X - x);
        }

        while (y != to.Y)
        {
            dungeon.SetTile(new Vector2I(x, y), DungeonTileType.Floor);
            y += Math.Sign(to.Y - y);
        }

        dungeon.SetTile(new Vector2I(x, y), DungeonTileType.Floor);
    }

    private bool RoomOverlapsAny(Rect2I room, List<Rect2I> rooms)
    {
        foreach (var other in rooms)
        {
            if (room.Intersects(other.Grow(1)))
                return true;
        }

        return false;
    }

    private Vector2I GetRoomCenter(Rect2I room)
    {
        return new Vector2I(
            room.Position.X + room.Size.X / 2,
            room.Position.Y + room.Size.Y / 2
        );
    }
}