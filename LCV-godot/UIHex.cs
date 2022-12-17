using Godot;
using System;

public class UIHex : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.

    Container coordsCo;
    public Label coordsLbl {get; private set;}
    MapMgr thisMapMgr;
    Vector2 coords;
    public Line2D[] outline {get; private set;}
    private bool _selected; //what a nightmare. thanks C#
                            //TODO: replace this with an "int highlighted"; 0 is "visible=false", all others are "visible=true" + a colour
    public bool selected {
        get {
            return _selected;
        }
        set {
            _selected = value;
            foreach (Line2D i in outline) {
                i.Visible = _selected;
            }
        }
    }
    
    public override void _Ready() {
        coordsCo = this.GetNode<Container>("CoordsCo");
        coordsLbl = this.GetNode<Label>("CoordsCo/CoordsLbl");
        outline = new Line2D[6];
        for (int i = 0; i < 6; i++) {
            outline[i] = this.GetNode<Line2D>("CoordsCo/Outline/Edge"+i.ToString());
        }
        selected = false;
    }

    public void Prep (MapMgr mgr, Vector2 hex) {
            thisMapMgr = mgr;
            coords = hex;
            coordsLbl.Text = "("+coords.x+","+coords.y+")";
            //setting sizes by code is a fun trick but not really worth it when I can make scenes ahead of time (and these controls are inscrutable)
    }

    public void EdgeColour (int edge, Color colour) {
        outline[edge].DefaultColor = colour;
    }

    public void EdgeVisible (int edge, bool vis) {
        outline[edge].Visible = vis;
    }
    public void OutlineColour (Color colour) {
        foreach (Line2D i in outline) {
            i.DefaultColor = colour;
        }
    }
    public void OutlineVisible (bool vis) {
        foreach (Line2D i in outline) {
            i.Visible = vis;
        }
    }


//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
