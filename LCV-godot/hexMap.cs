using Godot;
using System;
using System.Collections.Generic;

public class hexMap : Node //Tracks all of the hexagons. Does hexagon-map math to determine how the hexagons are interacting. All calculations use Cube Coordinates.
{
	/*NOTE: for now, hex maps will be saved and loaded as .tscn files, as per the intended Godot architecture.
	Because of this, any needed hexLayout code will be added to this script.*/
	PackedScene hexRef;
	//[Signal] delegate void mapPressed

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		hexRef = GD.Load<PackedScene>("res://HexAgon.tscn");
				//gonna test everything i've written so far
		var hexagon1 = hexRef.Instance<hexAgon>();
		hexagon1.Setup (1,2,-3);
		AddChild(hexagon1);
		var hexagon2 = hexRef.Instance<hexAgon>();
		hexagon2.Setup (2,3,-5);
		AddChild(hexagon2);
		var hexagon3 = hexRef.Instance<hexAgon>();
		hexagon3.Setup (1,3,-4);
		AddChild(hexagon3);
		GD.Print(IsEqual(hexagon1,hexagon2));
		var hexagonX = AddHexes(hexagon1,hexagon2);
		GD.Print(hexagonX.GetQ() + "," + hexagonX.GetR() + "," + hexagonX.GetS());

		
		List<hexAgon> hexList = new List<hexAgon>();
		hexList.Add(hexagon1);
		hexList.Add(hexagon2);
		hexList.Add(hexagon3);
		
	}
	
	public bool IsEqual(hexAgon hex1, hexAgon hex2)
	{
		if ( hex1.GetQ() == hex2.GetQ() && hex1.GetR() == hex2.GetR() && hex1.GetS() == hex2.GetS() ) //"s" is superfluous but still
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
	public hexAgon AddHexes(hexAgon hex1, hexAgon hex2)
	{
		hexAgon hexResult = new hexAgon();
		hexResult.Setup( hex1.GetQ()+hex2.GetQ(), hex1.GetR()+hex2.GetR(), hex1.GetS()+hex2.GetS() );
		
		return (hexResult);
	}

	public hexAgon SubtractHexes(hexAgon hex1, hexAgon hex2) //hex1 - hex2
	{
		hexAgon hexResult = new hexAgon();
		hexResult.Setup( hex1.GetQ()-hex2.GetQ(), hex1.GetR()-hex2.GetR(), hex1.GetS()-hex2.GetS() );
		
		return (hexResult);
	}
	
	public int hexDistance(hexAgon hex1, hexAgon hex2)
	{
		hexAgon hexResult = SubtractHexes(hex1,hex2);
		int distance = Math.Max(hexResult.GetQ(),hexResult.GetR());
		return (Math.Max(distance,hexResult.GetS()));
	}
	
	
	
	//public char hexFacing(hexAgon hex1, hexAgon hex2)
}
