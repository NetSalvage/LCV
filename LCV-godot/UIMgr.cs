using Godot;
using System;
using System.Collections.Generic;
using HexMath;

public class UIMgr : Node2D {
	public MapMgr thisMapMgr { get; private set; }
	Camera2D thisCam;
	CanvasLayer helpWindow;
	Label offsetCoordsLabel;

	CanvasLayer mapOverlay;
	Dictionary<Vector2, UIHex> hexOverlay = new Dictionary<Vector2, UIHex>();

	List<Vector2> selectedHex;
	Vector2[] path = new Vector2[0];
	bool mouseInUI;
	bool windowsOpen;
	
	Vector2 hexCoords; //map coords and world coords both use vector2, so...

	//engine-facing stuff
	public override void _Ready() {
		thisCam = GetNode<Camera2D>("UserCam");
		thisCam.MakeCurrent();
		offsetCoordsLabel = GetNode<Label>("UserCam/UserCanvasLayer/BottomRightWindow/BottomRightLabel"); //whew, lol.
		helpWindow = GetNode<CanvasLayer>("UserCam/UserCanvasLayer/Windows/HelpWindowScene");
		
		mapOverlay = GetNode<CanvasLayer>("MapOverlay");
		selectedHex = new List<Vector2>();
	}

	private void _Ready2(GameMgr mgr) {
		thisMapMgr = mgr.thisMapNode;
		PackedScene uIHex = GD.Load<PackedScene>("res://UIHex.tscn");
		foreach (Vector2 hex in thisMapMgr.GetUsedHexes()) {
			hexOverlay.Add (hex, (UIHex)uIHex.Instance());
			mapOverlay.AddChild(hexOverlay[hex]);
			hexOverlay[hex].Name = "UIhex["+hex.x+","+hex.y+"]";
			hexOverlay[hex].Position = thisMapMgr.thisTileMap.MapToWorld(hex);
			hexOverlay[hex].Prep(thisMapMgr,hex);
		}
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
			}
			else {
				windowsOpen = false;
			}
		}
		
		if (inputEvent.IsActionPressed("ui_cancel")) {
			BackOneLevel();
		}

		if (inputEvent.IsActionPressed("ui_maplabels")) {
			foreach (KeyValuePair<Vector2, UIHex> i in hexOverlay) {
				i.Value.coordsLbl.Visible=!i.Value.coordsLbl.Visible;
			}
		}

		if (selectedHex.Count == 1) {
			//dooon't think I can turn this into a switch statement. also good lord this is ugly...but it's well-structured? I think?
			if (inputEvent.IsActionPressed("mapU")) {
				selectedHex[0] = OddQ.Neighbor((selectedHex[0]),0);
			}
			else if (inputEvent.IsActionPressed("mapUR")) {
				selectedHex[0] = OddQ.Neighbor((selectedHex[0]),1);			
			}
			else if (inputEvent.IsActionPressed("mapDR")) {
				selectedHex[0] = OddQ.Neighbor((selectedHex[0]),2);	
			}
			else if (inputEvent.IsActionPressed("mapD")) {
				selectedHex[0] = OddQ.Neighbor((selectedHex[0]),3);	
			}
			else if (inputEvent.IsActionPressed("mapDL")) {
				selectedHex[0] = OddQ.Neighbor((selectedHex[0]),4);	
			}
			else if (inputEvent.IsActionPressed("mapUL")) {
				selectedHex[0] = OddQ.Neighbor((selectedHex[0]),5);	
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
		//TODO: use a List to record "actions taken" by the player, and then use this function to remove the latest step.
		//later I'll incorporate BackOneStep too, for specific windows. Which will do substeps one at a time instead of stepping
		//out of procedures.
		while (selectedHex.Count != 0) {
			hexOverlay[selectedHex[0]].OutlineVisible(false);
			selectedHex.RemoveAt(0);
		}
		foreach (Vector2 i in path) {
			hexOverlay[i].OutlineVisible(false); //TODO: maybe find a way to tally all highlighted hexes in one spot
		}
		updateSelectionUI();
	}

	void updateSelectionUI () {
		switch(selectedHex.Count) {
			case 1:
				offsetCoordsLabel.Text = ("(" + selectedHex[0].x + "," + selectedHex[0].y + ")");
				hexOverlay[selectedHex[0]].OutlineColour(Colors.Yellow);
				hexOverlay[selectedHex[0]].OutlineVisible(true);
				break;
			case 2:
				hexOverlay[selectedHex[0]].OutlineColour(Colors.Yellow);
				hexOverlay[selectedHex[0]].OutlineVisible(true);
				hexOverlay[selectedHex[1]].OutlineColour(Colors.Yellow);
				hexOverlay[selectedHex[1]].OutlineVisible(true);
				int distance = OddQ.Distance(selectedHex[0], selectedHex[1]);
				offsetCoordsLabel.Text += '\n' + "(" + selectedHex[1].x + "," + selectedHex[1].y + ")"
										+ '\n' + "Distance: " + distance;			
				if (distance == 1) {
					offsetCoordsLabel.Text += " hex";
				}
				else {
					offsetCoordsLabel.Text += " hexes";
				}
				path = OddQ.Line(selectedHex[0],selectedHex[1],thisMapMgr); //minimally expensive but it does calculate distance a second time
				foreach (Vector2 i in path) {
					if (i != selectedHex[0] && i != selectedHex[1]) {
						hexOverlay[i].OutlineColour(Colors.White);
						hexOverlay[i].OutlineVisible(true);
					}
				}
				break;
			default:
				offsetCoordsLabel.Text = "Clicked hex coordinates go here.";
				break;
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