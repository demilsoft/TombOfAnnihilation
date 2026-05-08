using Godot;

// public partial class TestSwitch : Node2D, IInteractable
// {
//     [Export] public TestDoor ConnectedDoor;

//     public void Interact(PlayerController player)
//     {
//         GD.Print("Switch activated.");
//         ConnectedDoor?.Toggle();
//     }
// }

public partial class TestSwitch : Node2D, IInteractable
{
    [Export] public TestDoor ConnectedDoor;
    [Export] public DungeonMain DungeonMain;

    public void Interact(PlayerController player)
    {
        GD.Print("Switch activated.");

        ConnectedDoor?.Toggle();
        DungeonMain?.RevealHiddenWall();
    }
}