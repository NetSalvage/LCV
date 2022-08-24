using Godot;
using System;

public class hexAgon : Node //Is the hexagons.
{
	protected int q; //q coordinate
	protected int r; //r coordinate
	protected int s; //s coordinate
	public void setup (int a, int b, int c)
	{
		q = a;
		r = b;
		s = c;
	}
	public int getQ()
	{
		return q;
	}
	public int getR()
	{
		return r;
	}
	public int getS()
	{
		return s;
	}
}
