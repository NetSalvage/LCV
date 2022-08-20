using Godot;
using System;


public class MapController : Node
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	private Line2D lineDrawer; //can't call Functions in this space; it has to be within another function
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		lineDrawer = GetNode<Line2D>("CellLine"); //RE line10: like here
		//lineDrawer.EndCapMode = enum LineCapMode 1;
		
		//in all likelihood, the rest of this function will eventually be triggered by a Signal
		//map size/etc info will go here
		GD.Print("Printing the Hexagons");
		//foreach hex etc.
		Vector2 hexPos;
		hexPos.x = 200;
		hexPos.y = 200;
		DrawHex(hexPos,30);
		GD.Print("Done");
	}
	
	public void DrawHex(Vector2 center, float size)
	{
		int angleDeg; 
		float angleRad;
		Vector2 vertex;
		for (int i = 0; i <= 6; i++) {
			angleDeg = 60 * i;
			angleRad = Mathf.Pi * angleDeg / 180;
			vertex.x = center.x + size * Mathf.Cos(angleRad);
			vertex.y = center.y + size * Mathf.Sin(angleRad);
			lineDrawer.AddPoint(vertex, i);
		}
		GD.Print("Drew hex at ("+ center.x +","+ center.y +")");
	}
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
