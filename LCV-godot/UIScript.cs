using Godot;
using System;

public class UIScript : Node2D
{
	Camera2D thisCam;
	Label hexCoordsLabel;
	CanvasLayer helpWindow;
	
	[Signal]
	public delegate void MapClicked (Vector2 mousePos);
	
	public override void _Ready()
	{
		thisCam = GetNode<Camera2D>("UserCam");
		thisCam.MakeCurrent();
		hexCoordsLabel = GetNode<Label>("UserCam/UserCanvasLayer/BottomRightWindow/BottomRightLabel"); //whew, lol.
		helpWindow = GetNode<CanvasLayer>("UserCam/UserCanvasLayer/HelpWindowScene");
	}

	//things that should happen only on press
	public override void _Input(InputEvent inputEvent)
	{
		if (inputEvent.IsActionPressed("choose_0"))
		{
			//check for UI elements being pressed, first. If not:
			this.EmitSignal("MapClicked", GetGlobalMousePosition());
		}
		
		if (inputEvent.IsActionPressed("ui_help"))
		{
			helpWindow.SetVisible(!helpWindow.IsVisible());
		}

	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		InputCheck(delta);
	}
	
	void InputCheck(float delta)
	{
		//camera movement
		if (Input.IsActionPressed("ui_left"))
		{
			thisCam.Position += Vector2.Left * 1000 * delta;
		}
		else if (Input.IsActionPressed("ui_right"))
		{
			thisCam.Position += Vector2.Right * 1000 * delta;
		}
		
		if (Input.IsActionPressed("ui_down"))
		{
			thisCam.Position += Vector2.Down * 1000 * delta;
		}
		else if (Input.IsActionPressed("ui_up"))
		{
			thisCam.Position += Vector2.Up * 1000 * delta;
		}
	}

	void OnCoordsReceived (Vector2 hexCoords)
	{
		hexCoordsLabel.SetText("(" + hexCoords.x + "," + hexCoords.y + ")");
	}

}
