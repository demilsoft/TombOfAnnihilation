using Godot;

public partial class TestSwitch : Node2D, IInteractable
{
    private bool _activated = false;

    public void Interact(PlayerController player)
    {
        if (_activated)
        {
            GD.Print("Switch already activated.");
            return;
        }

        _activated = true;

        GD.Print("You hear stone grinding in the distance...");
    }
}