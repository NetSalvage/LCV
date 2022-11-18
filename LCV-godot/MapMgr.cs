using Godot;
using System.Linq; //necessary for 			Godot.Collections.Array godotHexes = thisTileMap.GetUsedCells();
using System;
using System.Collections.Generic;

public class MapMgr : Node2D { //Manages the map, also provides all math functions related to hex maps. TileMap provides offset coordinates, but all calculations use Cube Coordinates.
	public TileMap thisTileMap {get; private set;}
	UIMgr thisUINode;

	Vector2[] usedHexes;
	
	public override void _Ready() {		
		thisTileMap = this.GetNode<TileMap>("ExampleMap"); //TODO: figure out a way to get the map's name ahead of time. That probably goes in the scene loading the game.
		usedHexes= new Vector2[0];
	}

	private void _Ready2(GameMgr mgr) {
		thisUINode = mgr.thisUINode;
	}
	
	public Vector3 GetCubeCoords(Vector2 offsetCoords ) {
		//this hexmap is odd-Q
		float q = offsetCoords.x;
		float r = offsetCoords.y - (q - ((int)q&1))/2;
		return (new Vector3 (q, r, -q-r));
	}
	
	public Vector2 GetOffsetCoords(Vector3 cubeCoords) {
		//again, this hexmap is odd-Q
		float q = cubeCoords.x;
		float r = cubeCoords.y + (q - ((int)q&1))/2;
		return (new Vector2 (q,r));
	}

	
	public Vector3 CubeSubtract(Vector3 cube1, Vector3 cube2) {
		return (new Vector3(cube1.x - cube2.x, cube1.y - cube2.y, cube1.z - cube2.z));
	}

	public int GetCubeDistance(Vector2 hexStart, Vector2 hexEnd) {
		Vector3 cubeStart = GetCubeCoords(hexStart);
		Vector3 cubeEnd = GetCubeCoords(hexEnd);
		Vector3 subtracted = CubeSubtract(cubeStart, cubeEnd);
		int theDistance = (int)( (Mathf.Abs(subtracted.x) + Mathf.Abs(subtracted.y) + Mathf.Abs(subtracted.z))/2 );
		return theDistance;
	}

	public Vector3 MoveHex(Vector3 hex, int direction) {
		switch (direction) {
			case 0:
				return new Vector3(hex.x, hex.y-1, hex.z+1);
			case 1:
				return new Vector3(hex.x+1, hex.y-1, hex.z);
			case 2:
				return new Vector3(hex.x+1, hex.y, hex.z-1);
			case 3:
				return new Vector3(hex.x, hex.y+1, hex.z-1);
			case 4:
				return new Vector3(hex.x-1, hex.y+1, hex.z);
			case 5:
				return new Vector3(hex.x-1, hex.y, hex.z+1);
			default:
				Console.Write("ERROR: Nonexistent direction. 0 is north, 1 is northeast...");
				return hex;
		}
	}

	public Vector2[] GetUsedHexes() {
		if (usedHexes.Length < 1) {			
			usedHexes = thisTileMap.GetUsedCells().Cast<Vector2>().ToArray(); //C# exceptionalism
		}
		return usedHexes;
	}


}
