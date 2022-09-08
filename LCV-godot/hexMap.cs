using Godot;
using System;
using System.Collections.Generic;

public class hexMap : Node //Tracks all of the hexagons. Does hexagon-map math to determine how the hexagons are interacting. All calculations use Cube Coordinates.
{
	/*NOTE: A bunch of this is deprecated.*/
	PackedScene hexRef;
	TileMap thisTileMap;
	Node2D thisUINode;
	
	[Signal]
	public delegate void HexCoords (Vector2 hexCoords);
	
	public override void _Ready()
	{
		hexRef = GD.Load<PackedScene>("res://HexAgon.tscn");
		thisTileMap = GetNode<TileMap>("BGTileMap");
		thisUINode = GetNode<Node2D>("UIRN");
		thisUINode.Connect("MapClicked", this, "OnMapClicked"); //TheThingYouWantToConnect.Connect("SignalString", targetInstance, targetMethod)
		this.Connect("HexCoords", thisUINode, "OnCoordsReceived");
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
	
	private void OnMapClicked(Vector2 mousePos)
	{
		//TODO: find a way to suss out the context in which the map is being clicked, so you can signal the right methods (???)
		this.EmitSignal("HexCoords", thisTileMap.WorldToMap(mousePos));
	}

}
