using Godot;
using System;

public class hexMap : Node //Tracks all of the hexagons. Does hexagon-map math to determine how the hexagons are interacting.
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//gonna test everything i've written so far
		var hexagon1 = new hexAgon();
		hexagon1.setup (1,2,3);
		var hexagon2 = new hexAgon();
		hexagon2.setup (2,3,4);
		GD.Print(isEqual(hexagon1,hexagon2));
		var hexagon3 = addHexes(hexagon1,hexagon2);
		GD.Print(hexagon3.getQ() + "," + hexagon3.getR() + "," + hexagon3.getS());
		//17:34 - it works!!
	}
	
	public bool isEqual(hexAgon hex1, hexAgon hex2)
	{
		if ( hex1.getQ() == hex2.getQ() && hex1.getR() == hex2.getR() && hex1.getS() == hex2.getS() ) //"s" is superfluous but still
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
	public hexAgon addHexes(hexAgon hex1, hexAgon hex2)
	{
		hexAgon hexResult = new hexAgon();
		hexResult.setup( hex1.getQ()+hex2.getQ(), hex1.getR()+hex2.getR(), hex1.getS()+hex2.getS() );
		return (hexResult);
	}

//hex map math goes here
//only need q and r for position, but we need s for math
//q+r+s = 0, that's all

}
