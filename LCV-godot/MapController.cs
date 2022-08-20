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
		DrawHex(0,0);
		GD.Print("Done");
	}
	
	public void DrawHex(int xPos, int yPos)
	{
		GD.Print("Drew hex at ("+ xPos +","+ yPos +")");
	}
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
