using Godot;
using System;



public partial class PlayerActions : Node
{
	public void Attack(Node2D body)
	{
		Skeleton target = (Skeleton)body;
		target.health -= 1;
	}
	
	private void Chop()
	{
		
	}
}
