using Godot;
using System;

public partial class Pause : Node
{
	
	private bool isPaused = false;
	private PackedScene pauseMenuScene;
	private Node pauseMenuInstance;
	
	public override void _Ready()
	{
		pauseMenuScene =  ResourceLoader.Load<PackedScene>("res://Scenes/PauseMenu/PauseMenu.tscn");
	}
	
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("menu"))
			TogglePause();
	}
	
	private void TogglePause(){
		isPaused = !isPaused;
		GetTree().Paused = isPaused;

		if (isPaused)
		{
			pauseMenuInstance = pauseMenuScene.Instantiate();
			GetTree().CurrentScene.AddChild(pauseMenuInstance);
		}	
	}
}
