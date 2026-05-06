using Godot;

public partial class DebugCamera : Camera2D
{
    [Export] public float PanSpeed = 300.0f;

    public override void _Process(double delta)
    {
		if (Input.IsActionPressed("camera_left"))
            GD.Print("camera_left");

        if (Input.IsActionPressed("camera_right"))
            GD.Print("camera_right");

        if (Input.IsActionPressed("camera_up"))
            GD.Print("camera_up");
			
        if (Input.IsActionPressed("camera_down"))
            GD.Print("camera_down");

        Vector2 move = Vector2.Zero;

        if (Input.IsActionPressed("camera_left"))
            move.X -= 1;
        if (Input.IsActionPressed("camera_right"))
            move.X += 1;
        if (Input.IsActionPressed("camera_up"))
            move.Y -= 1;
        if (Input.IsActionPressed("camera_down"))
            move.Y += 1;

        Position += move.Normalized() * PanSpeed * (float)delta;
    }
}
