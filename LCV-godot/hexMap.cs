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
	public delegate void OffsetCoords (Vector2 offsetCoords);
	
	public override void _Ready()
	{
		thisTileMap = GetNode<TileMap>("BGTileMap");
		thisUINode = GetNode<Node2D>("UIRN");
		thisUINode.Connect("MapClicked", this, "OnMapClicked"); //TheThingYouWantToConnect.Connect("SignalString", targetInstance, targetMethod)
		this.Connect("OffsetCoords", thisUINode, "OnCoordsReceived");
		
		//use the map scene to track hex map contents etc.
	}
	
	private void OnMapClicked(Vector2 mousePos)
	{
		//TODO: find a way to suss out the context in which the map is being clicked, so you can signal the right methods (???)
		this.EmitSignal("OffsetCoords", thisTileMap.WorldToMap(mousePos));
	}

	public Vector3 GetCubeCoords(Vector3 offsetCoords )
	{
		float q = offsetCoords.x;
		float r = offsetCoords.y - (offsetCoords.x - (offsetCoords.x % 2)) /2;
		//"bitwise and"; &1 returns 0 if even, and 1 if odd.
		//This also works with negative numbers, while MOD would have to check positive versus negative.
		return (new Vector3 (q, r, -q-r));
	}
	

}
