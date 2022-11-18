using Godot;
using System;
using System.Collections.Generic;

public class UIMgr : Node2D {
	public MapMgr thisMapMgr { get; private set; }
	Camera2D thisCam;
	CanvasLayer helpWindow;
	Label offsetCoordsLabel;

	CanvasLayer mapOverlay;
	Dictionary<Vector2, UIHex> hexOverlay = new Dictionary<Vector2, UIHex>();

	List<Vector2> selectedHex;
	bool mouseInUI;
	bool windowsOpen;
	
	Vector2 hexCoords; //map coords and world coords both use vector2, so...

	[Signal]
	public delegate void MapClicked (Vector2 mousePos);
	[Signal]
	public delegate void GetDistance (Vector2 hexStart, Vector2 hexEnd);

	//engine-facing stuff
	public override void _Ready() {
		thisCam = GetNode<Camera2D>("UserCam");
		thisCam.MakeCurrent();
		offsetCoordsLabel = GetNode<Label>("UserCam/UserCanvasLayer/BottomRightWindow/BottomRightLabel"); //whew, lol.
		helpWindow = GetNode<CanvasLayer>("UserCam/UserCanvasLayer/Windows/HelpWindowScene");
		
		mapOverlay = GetNode<CanvasLayer>("MapOverlay");
		mapOverlay.Visible=false;

		selectedHex = new List<Vector2>();
	}

	private void _Ready2(GameMgr mgr) {
		thisMapMgr = mgr.thisMapNode;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta) {
		LongInputCheck(delta);
	}

	//input-checking

	//things that should happen only on press
	public override void _Input(InputEvent inputEvent) {
		if (inputEvent.IsActionPressed("choose_0")) {
			if (mouseInUI==false && windowsOpen == false && selectedHex.Count <2) {
				selectedHex.Add(thisMapMgr.thisTileMap.WorldToMap(GetGlobalMousePosition()));
				updateSelectionUI();
			}
		}
		
		if (inputEvent.IsActionPressed("ui_help")) {
			helpWindow.Visible = !helpWindow.Visible;
			if (helpWindow.Visible) {
				windowsOpen = true;
			} else
			{
				windowsOpen = false;
			}
		}
		
		if (inputEvent.IsActionPressed("ui_cancel")) {
			BackOneLevel();
		}

		if (inputEvent.IsActionPressed("ui_maplabels")) {
			MapOverlayVisible(!mapOverlay.Visible);
		}

		if (selectedHex.Count == 1) {
			//dooon't think I can turn this into a switch statement. also good lord this is ugly...but it's well-structured? I think?
			if (inputEvent.IsActionPressed("mapN")) {
				selectedHex[0] = thisMapMgr.GetOffsetCoords( thisMapMgr.MoveHex( thisMapMgr.GetCubeCoords( selectedHex[0]),0));
			}
			else if (inputEvent.IsActionPressed("mapNE")) {
				selectedHex[0] = thisMapMgr.GetOffsetCoords( thisMapMgr.MoveHex( thisMapMgr.GetCubeCoords( selectedHex[0]),1));			
			}
			else if (inputEvent.IsActionPressed("mapSE")) {
				selectedHex[0] = thisMapMgr.GetOffsetCoords( thisMapMgr.MoveHex( thisMapMgr.GetCubeCoords( selectedHex[0]),2));
			}
			else if (inputEvent.IsActionPressed("mapS")) {
				selectedHex[0] = thisMapMgr.GetOffsetCoords( thisMapMgr.MoveHex( thisMapMgr.GetCubeCoords( selectedHex[0]),3));
			}
			else if (inputEvent.IsActionPressed("mapSW")) {
				selectedHex[0] = thisMapMgr.GetOffsetCoords( thisMapMgr.MoveHex( thisMapMgr.GetCubeCoords( selectedHex[0]),4));
			}
			else if (inputEvent.IsActionPressed("mapNW")) {
				selectedHex[0] = thisMapMgr.GetOffsetCoords( thisMapMgr.MoveHex( thisMapMgr.GetCubeCoords( selectedHex[0]),5));
			}
			updateSelectionUI();
		}

	}
	
	void LongInputCheck(float delta) {
		//camera movement
		if (Input.IsActionPressed("ui_left")) {
			thisCam.Position += Vector2.Left * 1000 * delta;
		}
		else if (Input.IsActionPressed("ui_right")) {
			thisCam.Position += Vector2.Right * 1000 * delta;
		}
		
		if (Input.IsActionPressed("ui_down")) {
			thisCam.Position += Vector2.Down * 1000 * delta;
		}
		else if (Input.IsActionPressed("ui_up")) {
			thisCam.Position += Vector2.Up * 1000 * delta;
		}
	}


	//Everything else

	void BackOneLevel()
	{
		//TODO: use a List to somehow record "actions taken" by the player, and then use this function to remove the latest step.
		//later I'll incorporate BackOneStep too, for specific windows. Which will do substeps one at a time instead of stepping
		//out of procedures.
		while (selectedHex.Count != 0) {
			selectedHex.RemoveAt(0);
		}
		offsetCoordsLabel.Text = "Clicked hex coordinates go here.";
	}

	void updateSelectionUI () {
		if (selectedHex.Count < 2) {
			offsetCoordsLabel.Text = ("(" + selectedHex[0].x + "," + selectedHex[0].y + ")");
		}
		else if (selectedHex.Count < 3) {
			int cubeDistance = thisMapMgr.GetCubeDistance(selectedHex[0], selectedHex[1]);
			if (cubeDistance == 1) {
				offsetCoordsLabel.Text += '\n' + "(" + selectedHex[1].x + "," + selectedHex[1].y + ")"
										+ '\n' + "Distance: "+ cubeDistance +" hex";
			}
			else {
				offsetCoordsLabel.Text += '\n' + "(" + selectedHex[1].x + "," + selectedHex[1].y + ")"
										+ '\n' + "Distance: "+ cubeDistance +" hexes";
			}
		}
	}

	public void MapOverlayVisible (bool state) {
		mapOverlay.Visible = state;
		if (state == true && hexOverlay.Count == 0) { //wow! dynamic loading!
			PackedScene uIHex = GD.Load<PackedScene>("res://UIHex.tscn");
			foreach (Vector2 hex in thisMapMgr.GetUsedHexes()) {
				hexOverlay.Add (hex, (UIHex)uIHex.Instance());
				mapOverlay.AddChild(hexOverlay[hex]);
				hexOverlay[hex].Position = thisMapMgr.thisTileMap.MapToWorld(hex);
				hexOverlay[hex].Prep(thisMapMgr,hex);
			} //HELL YES it works!!
			//TODO: center the labels.
			//FAR FUTURE TODO: draw hexagons over these hexagons
		}
	}




	//These two functions are connected to various UI elements through Godot's interface, for my own sanity:
	private void OnMouseInUI() {
		mouseInUI = true;
	}

	private void OnMouseOutUI() {
		mouseInUI = false;
	}
}