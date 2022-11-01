using Godot;
using System.Linq; //necessary for 			Godot.Collections.Array godotHexes = thisTileMap.GetUsedCells();
using System;
using System.Collections.Generic;

public class MapMgr : Node2D //Manages the map, also provides all math functions related to hex maps. TileMap provides offset coordinates, but all calculations use Cube Coordinates.
{
	TileMap thisTileMap;
	UIMgr thisUINode;
	
	[Signal]
	public delegate void ClickedOffsetCoords (Vector2 offsetCoords);
	[Signal]
	public delegate void CubeDistance (int distance);

	Vector2[] usedHexes;
	
	public override void _Ready()
	{		
		thisTileMap = this.GetNode<TileMap>("ExampleMap"); //TODO: figure out a way to get the map's name ahead of time. That probably goes in the scene loading the game.
		usedHexes= new Vector2[0];
	}

	private void _Ready2(GameMgr mgr)
	{
		thisUINode = mgr.UINodeGet();
	}
	
	public Vector2 WorldToMap(Vector2 mousePos)
	{
		return (thisTileMap.WorldToMap(mousePos));
	}

	public Vector2 MapToWorld(Vector2 cellPos)
	{
		return (thisTileMap.MapToWorld(cellPos));
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

	public int GetCubeDistance(Vector2 hexStart, Vector2 hexEnd)
	{
		Vector3 cubeStart = GetCubeCoords(hexStart);
		Vector3 cubeEnd = GetCubeCoords(hexEnd);
		Vector3 subtracted = CubeSubtract(cubeStart, cubeEnd);
		int theDistance = (int)( (Mathf.Abs(subtracted.x) + Mathf.Abs(subtracted.y) + Mathf.Abs(subtracted.z))/2 );

		this.EmitSignal("CubeDistance", theDistance);
		return theDistance;
	}

	public Vector2[] GetUsedHexes()
	{
		if (usedHexes.Length < 1) 			//not sure that usedhexes will return 0 if it's just "new Vector2[]", but here's hoping
		{
			Vector2[] usedHexes = thisTileMap.GetUsedCells().Cast<Vector2>().ToArray();			
		}
		return (usedHexes);
	}
}
