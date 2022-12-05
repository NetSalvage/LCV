using Godot;
using System;

public class UIHex : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    bool readied = false;
    Container coordsCo;
    Label coordsLbl;

    MapMgr thisMapMgr;
    Vector2 coords;
    Polygon2D outline;
    
    public override void _Ready() {
        coordsCo = this.GetNode<Container>("CoordsCo");
        coordsLbl = this.GetNode<Label>("CoordsCo/CoordsLbl");
    }

    public void Prep (MapMgr mgr, Vector2 hex) {
        if (readied == false) {
            thisMapMgr = mgr;
            coords = hex;
            coordsLbl.Text = "("+coords.x+","+coords.y+")";
            //setting sizes by code is a fun trick but not really worth it when I can make scenes ahead of time (and these controls are inscrutable)
        }

    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
