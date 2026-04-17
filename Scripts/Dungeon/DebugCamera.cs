using Godot;

public partial class DebugCamera : Camera2D
{
    [Export] public float PanSpeed = 300.0f;

    public override void _Process(double delta)
    {
		if (Input.IsActionPressed("move_left"))
            GD.Print("move_left");

        if (Input.IsActionPressed("move_right"))
            GD.Print("move_right");

        if (Input.IsActionPressed("move_up"))
            GD.Print("move_up");
			
        if (Input.IsActionPressed("move_down"))
            GD.Print("move_down");

        Vector2 move = Vector2.Zero;

        if (Input.IsActionPressed("move_left"))
            move.X -= 1;
        if (Input.IsActionPressed("move_right"))
            move.X += 1;
        if (Input.IsActionPressed("move_up"))
            move.Y -= 1;
        if (Input.IsActionPressed("move_down"))
            move.Y += 1;

        Position += move.Normalized() * PanSpeed * (float)delta;
    }
}
