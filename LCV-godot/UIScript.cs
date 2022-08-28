using Godot;
using System;

public class UIScript : Node2D
{
	Camera2D thisCam;
	public override void _Ready()
	{
		thisCam = GetNode<Camera2D>("UserCam");
		thisCam.MakeCurrent();
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if (Input.IsActionPressed("ui_left"))
		{
			thisCam.Position += Vector2.Left * 100 * delta;
		}
		else if (Input.IsActionPressed("ui_right"))
		{
			thisCam.Position += Vector2.Right * 100 * delta;
		}
		
		if (Input.IsActionPressed("ui_down"))
		{
			thisCam.Position += Vector2.Down * 100 * delta;
		}
		else if (Input.IsActionPressed("ui_up"))
		{
			thisCam.Position += Vector2.Up * 100 * delta;
		}
		
	}
}
