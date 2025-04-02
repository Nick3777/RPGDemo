using Godot;
using System;

public partial class Tree : Node2D
{
	
	private Sprite2D fullTree;
	private Sprite2D cutTree;
	private bool cut = false;
	
	public override void _Ready()
	{
		fullTree = GetNode<Sprite2D>("FullTree");
		cutTree = GetNode<Sprite2D>("CutTree");
		fullTree.Show();
		cutTree.Hide();
	}

	public override void _Process(double delta)
	{
		
	}
	
	private void onAreaToCutEntered(Area2D area)
	{
		if(cut) return;
		if(area.IsInGroup("Cutting"))
		{
			GD.Print("SSSSS");
			cut = true;
			fullTree.Hide();
			cutTree.Show();
		}
	}
}
