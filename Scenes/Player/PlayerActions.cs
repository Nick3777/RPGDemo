using Godot;
using System;



public partial class PlayerActions : Node
{
	public void Attack(Area2D enemyHurtbox)
	{
		Skeleton target = (Skeleton) enemyHurtbox.GetParent();
		target.health -= 1;
	}
	
	private void Chop()
	{
		
	}
}
