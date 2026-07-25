using Godot;
using System;

public partial class Saw : AnimatableBody2D
{
	public Area2D killing;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		killing = GetNode<Area2D >("killing") ;
		//killing.BodyEntered += OnKillingBodyEntered; 
	}

	private void OnKillingBodyEntered(Node body)
{
    

    if (body.IsInGroup("player"))
    {
        
        GetTree().ReloadCurrentScene();
    }
}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
