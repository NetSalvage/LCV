using Godot;
using System;

public class GameMgr : Node2D
{
    Node thisHexMap;
    Node2D thisUINode;
    TileMap thisTileMap;

    public override void _Ready()
    {
        thisUINode = GetNode<Node2D>("UIRN");
        thisHexMap = GetNode<Node>("HexMap");
        thisUINode.Connect("MapClicked", thisHexMap, "OnMapClicked"); //TheThingYouWantToConnect.Connect("SignalString", targetInstance, targetMethod)
		thisUINode.Connect("GetDistance", thisHexMap, "GetCubeDistance");   
        thisHexMap.Connect("ClickedOffsetCoords", thisUINode, "OnClickedOffsetCoords");
		thisHexMap.Connect("CubeDistance", thisUINode, "OnCubeDistance");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
