using Godot;

public partial class TestDoor : Node2D
{
    private bool _isOpen = false;
    private bool _isAnimating = false;

    private AnimatedSprite2D _animatedSprite;
    private CollisionShape2D _collisionShape;

    public override void _Ready()
    {
        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _collisionShape = GetNode<CollisionShape2D>("StaticBody2D/CollisionShape2D");

        _animatedSprite.AnimationFinished += OnAnimationFinished;

		// Start visually closed.
		_animatedSprite.Animation = "open";
		_animatedSprite.Frame = 0;
		_animatedSprite.Pause();

		// Start physically closed.
    	_collisionShape.Disabled = false;
    }

    public void Toggle()
    {
        if (_isAnimating)
            return;

        if (_isOpen)
            Close();
        else
            Open();
    }

    public void Open()
    {
        if (_isOpen || _isAnimating)
            return;

        _isAnimating = true;
        _isOpen = true;

        GD.Print("Door opening.");

        _animatedSprite.Play("open");
        _collisionShape.SetDeferred("disabled", true);
    }

    public void Close()
    {
        if (!_isOpen || _isAnimating)
            return;

        _isAnimating = true;
        _isOpen = false;

        GD.Print("Door closing.");

        _collisionShape.SetDeferred("disabled", false);
        _animatedSprite.Play("close");
    }

    private void OnAnimationFinished()
    {
		// Hold final frame
		_animatedSprite.Pause();

        GD.Print(_isOpen ? "Door opened." : "Door closed.");
    }
}