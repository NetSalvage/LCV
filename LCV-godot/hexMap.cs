using Godot;
using System;
using System.Collections.Generic;

public class hexMap : Node //Tracks all of the hexagons. Also contains all math functions related to hexagons and hex maps. All calculations use Cube Coordinates.
{
	/*NOTE: A bunch of this is deprecated.*/
	TileMap thisTileMap;
	Node2D thisUINode;
	
	[Signal]
	public delegate void ClickedOffsetCoords (Vector2 offsetCoords);
	[Signal]
	public delegate void CubeDistance (int distance);
	
	public override void _Ready()
	{		
		thisTileMap = this.GetNode<TileMap>("BGTileMap");
	}
	
	private void OnMapClicked(Vector2 mousePos)
	{
		//TODO: find a way to suss out the context in which the map is being clicked, so you can signal the right methods (???)
		this.EmitSignal("ClickedOffsetCoords", thisTileMap.WorldToMap(mousePos));
	}

	public Vector3 GetCubeCoords(Vector2 offsetCoords )
	{
		float q = offsetCoords.x;
		float r = offsetCoords.y - (offsetCoords.x - (offsetCoords.x % 2)) /2;
		//"bitwise and"; &1 returns 0 if even, and 1 if odd.
		//This also works with negative numbers, while MOD would have to check positive versus negative.
		return (new Vector3 (q, r, -q-r));
	}
	
	public Vector3 CubeSubtract(Vector3 cube1, Vector3 cube2)
	{
		return (new Vector3(cube1.x - cube2.x, cube1.y - cube2.y, cube1.z - cube2.z));
	}

	private int GetCubeDistance(Vector2 hexStart, Vector2 hexEnd)
	{
		Vector3 cubeStart = GetCubeCoords(hexStart);
		Vector3 cubeEnd = GetCubeCoords(hexEnd);
		Vector3 subtracted = CubeSubtract(cubeStart, cubeEnd);
		int theDistance = (int)( (Mathf.Abs(subtracted.x) + Mathf.Abs(subtracted.y) + Mathf.Abs(subtracted.z))/2 );

		this.EmitSignal("CubeDistance", theDistance);
		return theDistance;
	}

}
