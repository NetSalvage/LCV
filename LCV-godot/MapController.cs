using Godot;
using System;
using System.Collections.Generic;

public class MapController : Node
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	PackedScene hexRef;
	Node[,] mapCell;
	Vector2 gridStart;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		hexRef = GD.Load<PackedScene>("res://Hexagon.tscn");
		//map size/etc info will go here
		int q = 5;
		int r = 5;
		mapCell = new Node[q,r];
		
		GD.Print("Printing a hex map that is " + q + " units along the q axis, and " + r + " units along the r axis.");
		
		
		gridStart.x = 200;
		gridStart.y = 200;
		DrawMap(gridStart,q,r);
		GD.Print("Done"); //TODO: make this message play via a Signal saying that the map is done
	}
	
	public void DrawMap(Vector2 origin, int q, int r)
	{
		for (int a = 0; a < q; a++)
		{
			for (int b = 0; b < r; b++)
			{ 
				var thisHex = hexRef.Instance<Hex>();
				AddChild(thisHex);
				mapCell[a,b] = thisHex;
				// "res://" means "look for a RESOURCE inside the project file."
				thisHex.DrawHex(origin);
				GD.Print(a+","+b);
			}
		}
		
	}
}
