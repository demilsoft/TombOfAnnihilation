using Godot;

public partial class PlayerController : CharacterBody2D
{
    [Export] public int TileSize = 16;

    private bool _isMoving = false;

    public override void _Process(double delta)
    {
        if (_isMoving)
            return;

        Vector2I direction = Vector2I.Zero;

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
            TryMove(direction);
        }
    }

    private void TryMove(Vector2I direction)
    {
        Vector2 targetPosition = Position + (Vector2)(direction * TileSize);

        // For now: no collision check yet
        Position = targetPosition;
    }
}