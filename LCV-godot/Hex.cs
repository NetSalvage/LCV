using Godot;
using System;

public class Hex : Node2D
{
	private Line2D lineDrawer; //can't call Functions in this space; it has to be within another function
	float circumradius = 30;
	
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
}
