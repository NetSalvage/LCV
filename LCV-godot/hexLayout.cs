using Godot;
using System;
using System.Collections.Generic;

public class hexLayout : Node //Renders the hexagons. Does the visual math. Should get all of the hex's info from hexMap.
{	

	public void DrawMap(List<hexAgon> hexList) //TODO: as stuff gets implemented, we'll pass some sort of data type to hexLayout, and then hexLayout will draw from there. For now we'll just pass a starting point.
	{
		foreach(hexAgon hex in hexList)
		{
			DrawHex(hex);
		}
	}

	
	void DrawHex(hexAgon hex)
	{	
		var lineDrawer = hex.GetNode<Line2D>("cellEdge"); //RE line6: like here
		float circumradius = 20f; //Magic Number but UI stuff will handle any real scaling
		Vector2 center;
		center.x = 100 + (circumradius * 3 / 2) * hex.GetQ(); //moves 3*width/4 "right" for each +q, which is 3*circumradius*2/4, which is 3*circumradius/2.
		GD.Print(hex.GetQ());
		GD.Print(hex.GetR());
		GD.Print(hex.GetS());
		GD.Print(Mathf.Cos(Mathf.Pi/3));
		center.y = 200 	+ hex.GetR() * circumradius * Mathf.Sin((Mathf.Pi/3))
						- hex.GetS() * circumradius * Mathf.Sin((Mathf.Pi/3))	;
		/*	Easiest way to get y offset is to add all 3 axes' contributions to the "y" axis together. 
			+q is horizontal. 0 on the y axis.
			+r is W 60deg S, so r * -(sin60).
			+s is W 60deg N, so r *  (sin60).	
			Multiplying both of these by -1, because +y is downwards.	*/
		
		for (int i = 0; i <= 6; i++)
		{
			int angleDeg; 
			float angleRad;
			Vector2 vertex;
			angleDeg = 60 * i;
			angleRad = Mathf.Pi * angleDeg / 180;
			vertex.x = center.x + circumradius * Mathf.Cos(angleRad);
			vertex.y = center.y + circumradius * Mathf.Sin(angleRad);
			lineDrawer.AddPoint(vertex, i);
		}
		GD.Print("Drew hex at ("+ center.x +","+ center.y +")");
	}

}
