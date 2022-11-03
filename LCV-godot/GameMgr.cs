using Godot;
using System;

public class GameMgr : Node2D {
    public MapMgr thisMapNode {get; private set;}
    public UIMgr thisUINode {get; private set;}

    [Signal] //try not putting a newline
    public delegate void Ready2(GameMgr mgrRef);

    public override void _Ready() {
        thisUINode = GetNode<UIMgr>("UIRN");
        thisMapNode = GetNode<MapMgr>("MapMgr");
        this.Connect("Ready2", thisUINode, "_Ready2");
        this.Connect("Ready2", thisMapNode, "_Ready2");
        this.EmitSignal("Ready2", this);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
