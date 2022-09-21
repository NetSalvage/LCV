using Godot;
using System;
using System.Collections.Generic;

public class UIScript : Node2D
{
	Camera2D thisCam;
	CanvasLayer helpWindow;
	Label offsetCoordsLabel;
	List<Vector2> selectedHex;
	bool mouseInUI;
	bool windowsOpen;
	
	[Signal]
	public delegate void MapClicked (Vector2 mousePos);
		
	public override void _Ready()
	{
		thisCam = GetNode<Camera2D>("UserCam");
		thisCam.MakeCurrent();
		offsetCoordsLabel = GetNode<Label>("UserCam/UserCanvasLayer/BottomRightWindow/BottomRightLabel"); //whew, lol.
		helpWindow = GetNode<CanvasLayer>("UserCam/UserCanvasLayer/Windows/HelpWindowScene");
		selectedHex = new List<Vector2>();
	}

	//things that should happen only on press
	public override void _Input(InputEvent inputEvent)
	{
		if (inputEvent.IsActionPressed("choose_0"))
		{
			if (mouseInUI==false && windowsOpen == false)
			{
				this.EmitSignal("MapClicked", GetGlobalMousePosition());
			}
		}
		
		if (inputEvent.IsActionPressed("ui_help"))
		{
			helpWindow.SetVisible(!helpWindow.IsVisible());
			if (helpWindow.IsVisible())
			{
				windowsOpen = true;
			}
		}
		
		if (inputEvent.IsActionPressed("ui_cancel"))
		{
			BackOneLevel();
		}

	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		LongInputCheck(delta);
	}
	
	void LongInputCheck(float delta)
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

	void OnCoordsReceived (Vector2 offsetCoords)
	{
		if (selectedHex.Count < 1)
		{
			selectedHex.Add(offsetCoords);
			offsetCoordsLabel.SetText	("(" + selectedHex[0].x + "," + selectedHex[0].y + ")");
		}
		else if (selectedHex.Count < 2)
		{
			selectedHex.Add(offsetCoords);
			offsetCoordsLabel.SetText	("(" + selectedHex[0].x + "," + selectedHex[0].y + ")" + '\n' 
									+"(" + selectedHex[1].x + "," + selectedHex[1].y + ")");
		}
	}
	
	void BackOneLevel()
	{
		//i'm going to have to use a List to somehow record "actions taken" by the player, and then use this function to remove the latest step.
		//later I'll incorporate BackOneStep too, for specific windows. Which will do substeps one at a time instead of stepping out of procedures.
		while (selectedHex.Count != 0)
		{
			selectedHex.RemoveAt(0);
		}
		offsetCoordsLabel.SetText	("Clicked hex coordinates go here.");
	}

	//These two functions are connected to various UI elements through Godot's interface, for my own sanity:
	private void OnMouseInUI() 
	{
		mouseInUI = true;
	}

	private void OnMouseOutUI()
	{
		mouseInUI = false;
	}

}








