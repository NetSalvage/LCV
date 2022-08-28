using Godot;
using System;
using System.Collections.Generic;

public class hexMap : Node //Tracks all of the hexagons. Does hexagon-map math to determine how the hexagons are interacting.
{
	PackedScene hexRef;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		hexRef = GD.Load<PackedScene>("res://HexAgon.tscn");
		var thisLayout = GetNode<hexLayout>("LayoutNode");
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
		thisLayout.DrawMap(hexList);
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

//hex map math goes here
//only need q and r for position, but we need s for math
//q+r+s = 0, that's all

}