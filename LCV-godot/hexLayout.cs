using Godot;
using System;

public class hexLayout : Node //Renders the hexagons. Does the visual math. Should get all of the hex's info from hexMap.
{
	
/* 
	private Line2D lineDrawer; //can't call Functions in the class space; it has to be within a function
	float circumradius = 30; //just nice to have a default
	
	public void DrawHex(Vector2 center)
	{
		lineDrawer = GetNode<Line2D>("CellLine"); //RE line6: like here
		int angleDeg; 
		float angleRad;
		Vector2 vertex;
		
		
		for (int i = 0; i <= 6; i++)
		{
			angleDeg = 60 * i;
			angleRad = Mathf.Pi * angleDeg / 180;
			vertex.x = center.x + circumradius * Mathf.Cos(angleRad);
			vertex.y = center.y + circumradius * Mathf.Sin(angleRad);
			lineDrawer.AddPoint(vertex, i);
		}
		GD.Print("Drew hex at ("+ center.x +","+ center.y +")");
	}
*/
}
