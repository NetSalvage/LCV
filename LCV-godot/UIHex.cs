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
    
    public override void _Ready() {
        coordsCo = this.GetNode<Container>("coordsCo");
        coordsLbl = this.GetNode<Label>("coordsCo/coordsLbl");
    }

    public void Prep (MapMgr mgr, Vector2 hex) {
        if (readied == false) {
            thisMapMgr = mgr;
            coords = hex;
            coordsCo.RectSize = thisMapMgr.thisTileMap.CellSize;
            coordsLbl.Text = "("+coords.x+","+coords.y+")";
            coordsLbl.Align = Label.AlignEnum.Center; //that took long enough to figure out
            coordsLbl.Valign = Label.VAlign.Center; //improperly documented!!
            coordsLbl.AnchorLeft= 0;
            coordsLbl.AnchorRight= 1;
            coordsLbl.AnchorTop= 0;
            coordsLbl.AnchorBottom= 1;	
        }

    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
