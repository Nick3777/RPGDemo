using Godot;
using System;



public partial class PlayerActions : Node
{
	public void Attack(Area2D area)
	{
		Skeleton target = (Skeleton)area.GetParent();
		target.health -= 1;
	}
	
	private void Chop()
	{
		
	}
}
