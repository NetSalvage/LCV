using Godot;
using System.Linq; //necessary for 			Godot.Collections.Array godotHexes = thisTileMap.GetUsedCells();
using System;
using System.Collections.Generic;
using HexMath;

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

	public Vector2[] GetUsedHexes() {
		if (usedHexes.Length < 1) {			
			usedHexes = thisTileMap.GetUsedCells().Cast<Vector2>().ToArray(); //C# exceptionalism
		}
		return usedHexes;
	}


}
