using Godot;
using System;
using System.Collections.Generic;

public class UIMgr : Node2D
{
	MapMgr thisMapMgr;
	Camera2D thisCam;
	CanvasLayer helpWindow;
	public CanvasLayer mapOverlay { get; private set; }
	Label offsetCoordsLabel;
	List<Vector2> selectedHex;
	bool mouseInUI;
	bool windowsOpen;
	bool mapOverlayInitialized = false;
	
	Vector2 hexCoords; //map coords and world coords both use vector2, so...

	[Signal]
	public delegate void MapClicked (Vector2 mousePos);
	[Signal]
	public delegate void GetDistance (Vector2 hexStart, Vector2 hexEnd);

	//engine-facing stuff
	public override void _Ready()
	{
		thisCam = GetNode<Camera2D>("UserCam");
		thisCam.MakeCurrent();
		offsetCoordsLabel = GetNode<Label>("UserCam/UserCanvasLayer/BottomRightWindow/BottomRightLabel"); //whew, lol.
		helpWindow = GetNode<CanvasLayer>("UserCam/UserCanvasLayer/Windows/HelpWindowScene");
		mapOverlay = GetNode<CanvasLayer>("MapOverlay");
		mapOverlay.Visible=false;
		selectedHex = new List<Vector2>();
	}

	private void _Ready2(GameMgr mgr)
	{
		thisMapMgr = mgr.MapNodeGet();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		LongInputCheck(delta);
	}


	//input-checking

	//things that should happen only on press
	public override void _Input(InputEvent inputEvent)
	{
		if (inputEvent.IsActionPressed("choose_0"))
		{
			if (mouseInUI==false && windowsOpen == false)
			{
				ClickedOffsetCoords( thisMapMgr.WorldToMap(GetGlobalMousePosition()) );
			}
		}
		
		if (inputEvent.IsActionPressed("ui_help"))
		{
			helpWindow.Visible = !helpWindow.Visible;
			if (helpWindow.Visible)
			{
				windowsOpen = true;
			}
		}
		
		if (inputEvent.IsActionPressed("ui_cancel"))
		{
			BackOneLevel();
		}
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


	//Everything else

	void BackOneLevel()
	{
		//TODO: use a List to somehow record "actions taken" by the player, and then use this function to remove the latest step.
		//later I'll incorporate BackOneStep too, for specific windows. Which will do substeps one at a time instead of stepping
		//out of procedures.
		while (selectedHex.Count != 0)
		{
			selectedHex.RemoveAt(0);
		}
		offsetCoordsLabel.Text = "Clicked hex coordinates go here.";
	}

	void ClickedOffsetCoords (Vector2 offsetCoords)
	{
		if (selectedHex.Count < 1)
		{
			selectedHex.Add(offsetCoords);
			offsetCoordsLabel.Text = ("(" + selectedHex[0].x + "," + selectedHex[0].y + ")");
		}
		else if (selectedHex.Count < 2)
		{
			selectedHex.Add(offsetCoords);
			int cubeDistance = thisMapMgr.GetCubeDistance(selectedHex[0], selectedHex[1]);
			if (cubeDistance == 1)
			{
				offsetCoordsLabel.Text += '\n' + "(" + selectedHex[1].x + "," + selectedHex[1].y + ")"
										+ '\n' + "Distance: "+ cubeDistance +" hex";
			}
			else
			{
				offsetCoordsLabel.Text += '\n' + "(" + selectedHex[1].x + "," + selectedHex[1].y + ")"
										+ '\n' + "Distance: "+ cubeDistance +" hexes";
			}
		}
	}

	public void MapOverlayVisible (bool state)
	{
		mapOverlay.Visible = state;
		if (state == true && mapOverlayInitialized == false) //wow! dynamic loading!
		{
			foreach (Vector2 hex in thisMapMgr.GetUsedHexes())
			{
				//instantiate label at The Right Spot, center the label text, 
				//TODO: if these labels need to get modified in the future, I'll have to reconfigure this to, like...
				//create a dictionary that uses the hex coordinates as a key.
				Label hexLabel = new Label(); //What can I even put in those brackets...?
				hexLabel.SetPosition(thisMapMgr.MapToWorld(hex));
				//hexLabel.SetAlign((enum)VALIGN_CENTER); continue from here
			}
			mapOverlayInitialized = true;
		}
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