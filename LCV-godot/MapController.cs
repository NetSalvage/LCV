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
		lineDrawer = GetNode<Line2D>("HexLine"); //RE line10: like here

		//in all likelihood, the rest of this function will eventually be triggered by a Signal
		//map size/etc info will go here
		GD.Print("Printing the Hexagons");
		//foreach hex etc.
		Vector2 hexPos;
		hexPos.x = 0;
		hexPos.y = 0;
		DrawHex(hexPos);
		GD.Print("Done");
	}
	
	public void DrawHex(Vector2 a)
	{
		int i = 0;
		int angle_deg = 60 * i;
		float angle_rad = Mathf.Pi * angle_deg / 180;
		/* hold on
		for (i = 0; i < 5; i++) {
			lineDrawer.set_points(i) = 
		
		}*/
		GD.Print("Drew hex at ("+ a.x +","+ a.y +")");
	}
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
