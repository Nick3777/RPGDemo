using Godot;
using System;

public partial class World : Node2D
{
	private Camera2D camera;
	private Player player;
	
	public override void _Ready()
	{
		camera = GetNode<Camera2D>("Camera2D");
		player = GetNode<Player>("Player");
	}
	
	public override void _Process(double delta)
	{
		camera.GlobalPosition = player.GlobalPosition;
	}
}
