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

	bool mouseInUI;
	bool windowsOpen;

	//information stored in the UI
	List<Vector2> selectedHex;
	List<Vector2> path = new List<Vector2>();
	List<Vector2> area = new List<Vector2>();
	bool areaVisible = false;

		

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
		//"_Input" is called by the engine, once per frame, for each InputEvent.
		//InputEvents are created by pressing a button for the first time in that frame.

		if (inputEvent.IsActionPressed("choose_0")) {
			if (mouseInUI==false && windowsOpen == false && selectedHex.Count <2) {
				Vector2 coords = thisMapMgr.thisTileMap.WorldToMap(GetGlobalMousePosition());
				if (HexExists(coords)) {
					selectedHex.Add(coords);
					updateSelectionUI();
					return;
				}
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
			return;
		}
		
		if (inputEvent.IsActionPressed("ui_cancel")) {
			BackOneLevel();
			return;
		}

		if (inputEvent.IsActionPressed("ui_maplabels")) {
			foreach (KeyValuePair<Vector2, UIHex> i in hexOverlay) {
				i.Value.coordsLbl.Visible=!i.Value.coordsLbl.Visible;
			}
			return;
		}

		if (selectedHex.Count > 0) {
			Vector2 testOffsetCoords=selectedHex[selectedHex.Count-1];
			if (inputEvent.IsActionPressed("mapU")) {
				testOffsetCoords = OddQ.Neighbor((testOffsetCoords),0);
			}
			else if (inputEvent.IsActionPressed("mapUR")) {
				testOffsetCoords = OddQ.Neighbor((testOffsetCoords),1);			
			}
			else if (inputEvent.IsActionPressed("mapDR")) {
				testOffsetCoords = OddQ.Neighbor((testOffsetCoords),2);	
			}
			else if (inputEvent.IsActionPressed("mapD")) {
				testOffsetCoords = OddQ.Neighbor((testOffsetCoords),3);	
			}
			else if (inputEvent.IsActionPressed("mapDL")) {
				testOffsetCoords = OddQ.Neighbor((testOffsetCoords),4);	
			}
			else if (inputEvent.IsActionPressed("mapUL")) {
				testOffsetCoords = OddQ.Neighbor((testOffsetCoords),5);	
			}

			if (testOffsetCoords != selectedHex[selectedHex.Count-1]) {
				bool testCrashed = false;
				try {
					UIHex testHex = hexOverlay[testOffsetCoords];
				}
				catch {
					testCrashed = true;
				}
				if (!testCrashed) {
					if (selectedHex.Count == 1) {
						Deselect(selectedHex[0]);
						selectedHex[0] = testOffsetCoords;
						updateSelectionUI();
					}
					else {
						Deselect(selectedHex[1]); 
						selectedHex[1] = testOffsetCoords;
						updateSelectionUI();
					}
				}
				//else if (selectedHex.Count == 2) {
			}

			if (inputEvent.IsActionPressed("selection_area")) {
				areaVisible = !areaVisible;
				updateSelectionUI();
			}
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
			Deselect(selectedHex[0]);
			selectedHex.RemoveAt(0);
		}
		foreach (Vector2 i in path) {
			Deselect(i);
		}
		updateSelectionUI();
	}

	void Deselect(Vector2 hex) {
		hexOverlay[hex].OutlineVisible(false);
	}
	
	void updateSelectionUI () {
		//updates the appearance of hexes based on whether or not they're selected.
		//Also (currently) updates the "area" shown.
		if (selectedHex.Count == 0) {
			offsetCoordsLabel.Text = "Clicked hex coordinates go here.";
		}
		else {
			offsetCoordsLabel.Text = ("(" + selectedHex[0].x + "," + selectedHex[0].y + ")");
			hexOverlay[selectedHex[0]].OutlineColour(Colors.Yellow);
			hexOverlay[selectedHex[0]].OutlineVisible(true);
			if (selectedHex.Count > 1) {
				foreach (Vector2 i in path) {
					Deselect(i);
				}
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
				path = OddQ.Line(selectedHex[0],selectedHex[1],thisMapMgr);
				//"path" is not filling up with entries.
				//Cube.Line works, but OddQ.Line is not returning the list.
				foreach (Vector2 i in path) {
					if (i != selectedHex[0] && i != selectedHex[1]) {
						hexOverlay[i].OutlineColour(Colors.White);
						hexOverlay[i].OutlineVisible(true);
					}
				}
			}
		}

		//handle Area display
		foreach (Vector2 i in area) {
			if (!selectedHex.Contains(i)) {
				bool remove = true;
				foreach (Vector2 j in path) {
					if (j == i) {
						remove = false;
					}
				}
				if (remove) {
					hexOverlay[i].OutlineVisible(false);
				}
			}
			//TODO: Convert hexmath functions to use lists; use lists everywhere; then you can just do "if !selectedHex.Contains(i) && !path.Contains(i)".
		}

		area.Clear();
		if (selectedHex.Count == 2) {
			//Displays all hexes within [distance] radius of the starting hex.
			if (areaVisible) {
				foreach (Vector2 i in HexMath.OddQ.Area(selectedHex[0], OddQ.Distance(selectedHex[0], selectedHex[1]))) {
					if (i != selectedHex[0] && i != selectedHex[1]) {
						bool valid = true;
						foreach (Vector2 j in path) {
							if (i == j || HexExists(i) == false) {
								valid = false;
								break;
							}
						}
						if (valid) {
							area.Add(i);
						}
					}
				}
				foreach (Vector2 i in area) {
					hexOverlay[i].OutlineColour(Colors.Gray);
					hexOverlay[i].OutlineVisible(true);
				}
			}
		}
	}

	//These two functions are connected to various UI elements through Godot's interface, for my own sanity:
	private void OnMouseInUI() {
		mouseInUI = true;
	}

	private void OnMouseOutUI() {
		mouseInUI = false;
	}

	bool HexExists(Vector2 hex) {
		//Not sure how I feel about my error-checking being dependent on the UIHex list.
		//However, it's much faster than checking the entire hex list for the hex in question.
		bool result = true;
		try {
			UIHex testHex = hexOverlay[hex];
		}
		catch {
			result = false;
		}		
		return result;
	}
}