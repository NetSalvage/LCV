using Godot;
using System;

public class hexAgon : Node //Is the hexagons. 
{
	protected int q; //q coordinate
	protected int r; //r coordinate
	protected int s; //s coordinate
	//remember 60 degrees is pi/3.
	
	public void Setup (int a, int b, int c)
	{
		q = a;
		r = b;
		s = c;
	}
	public int GetQ()
	{
		return q;
	}
	public int GetR()
	{
		return r;
	}
	public int GetS()
	{
		return s;
	}
}
