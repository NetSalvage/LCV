using Godot;
using System;

public class GameMgr : Node2D
{
    hexMap thisMapNode;
    UIScript thisUINode;
    [Signal]
    public delegate void Ready2(GameMgr mgrRef);

    public override void _Ready()
    {
        thisUINode = GetNode<UIScript>("UIRN");
        thisMapNode = GetNode<hexMap>("MapMgr");
        this.Connect("Ready2", thisUINode, "_Ready2");
        this.Connect("Ready2", thisMapNode, "_Ready2");
        this.EmitSignal("Ready2", this);
    }

    public hexMap MapNodeGet()
    {
        return thisMapNode;
    }

    public UIScript UINodeGet()
    {
        return thisUINode;
    }


//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
