using Godot;

public partial class PlayerController : CharacterBody2D
{
    [Export] public int TileSize = 32;
    public Vector2I FacingDirection { get; private set; } = new Vector2I(0, 1);

    private DungeonData _dungeon;
    private Area2D _interactionArea;

    public void SetDungeon(DungeonData dungeon)
    {
        _dungeon = dungeon;
    }

        public override void _Ready()
    {
        _interactionArea = GetNode<Area2D>("InteractionArea");
    }

    public override void _Process(double delta)
    {
        Vector2I direction = Vector2I.Zero;

        // Get player input for movement 
        if (Input.IsActionJustPressed("move_up"))
            direction = new Vector2I(0, -1);
        else if (Input.IsActionJustPressed("move_down"))
            direction = new Vector2I(0, 1);
        else if (Input.IsActionJustPressed("move_left"))
            direction = new Vector2I(-1, 0);
        else if (Input.IsActionJustPressed("move_right"))
            direction = new Vector2I(1, 0);

        if (direction != Vector2I.Zero)
        {
            FacingDirection = direction;
            TryMove(direction);
        }

        // Get player input for interaction 
        if (Input.IsActionJustPressed("interact"))
        {
            TryInteract();
        }
    }

    private void TryMove(Vector2I direction)
    {
        if (_dungeon == null)
        {
            // Debug print to console
            GD.PrintErr("Player has no DungeonData assigned.");
            return;
        }

        Vector2I currentTile = WorldToTile(GlobalPosition);
        Vector2I targetTile = currentTile + direction;

        if (!CanMoveTo(targetTile))
        {
            // Debug print to console
            GD.Print($"Blocked by tile: {_dungeon.GetTile(targetTile)} at {targetTile}");
            return;
        }

        GlobalPosition = TileToWorld(targetTile);
    }

    private bool CanMoveTo(Vector2I tile)
    {
        if (!_dungeon.IsInBounds(tile))
            return false;

        DungeonTileType tileType = _dungeon.GetTile(tile);

        return tileType == DungeonTileType.Floor ||
               tileType == DungeonTileType.Start ||
               tileType == DungeonTileType.Exit;
    }

    // Get & Set Facing direction of player
    //public Vector2I FacingDirection { get; private set; } = new Vector2I(0, 1);

    // Interaction method
    private void TryInteract()
    {
        if (_interactionArea == null)
        {
            GD.PrintErr("InteractionArea is not assigned.");
            return;
        }

        var overlappingAreas = _interactionArea.GetOverlappingAreas();

        foreach (Area2D area in overlappingAreas)
        {
            Node parent = area.GetParent();

            if (parent is IInteractable interactable)
            {
                GD.Print($"Interacting with: {parent.Name}");
                interactable.Interact(this);
                return;
            }
        }

        GD.Print("Nothing to interact with.");
    }

    private Vector2I WorldToTile(Vector2 worldPosition)
    {
        return new Vector2I(
            Mathf.FloorToInt(worldPosition.X / TileSize),
            Mathf.FloorToInt(worldPosition.Y / TileSize)
        );
    }

    private Vector2 TileToWorld(Vector2I tilePosition)
    {
        return new Vector2(
            tilePosition.X * TileSize + TileSize / 2,
            tilePosition.Y * TileSize + TileSize / 2
        );
    }
    
}