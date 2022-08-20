using Godot;
using System;

public class HexDrawer : Line2D
{

	public void DrawHex(int xPos, int yPos)
	{
		GD.Print("Drew hex at ("+ xPos +","+ yPos +")");
	}

}
