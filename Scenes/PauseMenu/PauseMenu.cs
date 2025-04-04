using Godot;
using System;

public partial class PauseMenu : Control
{
	private Sprite2D sprite;
	private float timeAccumulator = 0.0f;
	private HBoxContainer back;
	private HBoxContainer save;
	private HBoxContainer startMenu;
	private HBoxContainer target ;
	
	public override void _Ready()
	{
		back = GetNode<HBoxContainer>("CanvasLayer/VBoxContainer/Continue");
		save = GetNode<HBoxContainer>("CanvasLayer/VBoxContainer/Save");
		startMenu = GetNode<HBoxContainer>("CanvasLayer/VBoxContainer/MainMenu");
		
		back.GrabFocus();
	}

	public override void _Process(double delta)
	{
		target = GetTree().Root.GetViewport().GuiGetFocusOwner() as HBoxContainer;
		ModulateColor(target, delta);
		
		if(Input.IsActionJustPressed("menu"))
			ReturnToGame();
		else{
			if(Input.IsActionJustPressed("accept"))
				HandlePauseAction(target.Name);
		}
	}
	
	private void ModulateColor(HBoxContainer target, double delta)
	{
		sprite = target.GetNode<Sprite2D>("Sprite2D");
		timeAccumulator += (float)delta;
		float t = timeAccumulator % 2.0f;
		
		// Calcola la luminosità: quando t=0 o t=2 il valore è 1 (bianco), quando t=1 il valore è 0 (nero)
		float brightness = Math.Abs(t - 1.0f);
		sprite.Modulate = new Color(brightness, brightness, brightness);
	}
	
	private void onContinueFocusExited()
	{
		sprite = GetNode<Sprite2D>("CanvasLayer/VBoxContainer/Continue/Sprite2D");
		sprite.Modulate = new Color(1, 1, 1);
	}

	private void ReturnToGame()
	{
		GetTree().Paused = false;		
		this.QueueFree();
	}

	private void HandlePauseAction(string targetName)
	{
		switch(targetName)
		{
			case "Continue":
				ReturnToGame();
				break;
			case "Save":
				GD.Print("Save");
				break;
			case "MainMenu":
				GD.Print("MainMenu");
				break;
			default:
				break;
		}
	}

	private void onSaveFocusExited()
	{
		sprite = GetNode<Sprite2D>("CanvasLayer/VBoxContainer/Save/Sprite2D");
		sprite.Modulate = new Color(1, 1, 1);
	}


	private void onStartFocusExited()
	{
		sprite = GetNode<Sprite2D>("CanvasLayer/VBoxContainer/MainMenu/Sprite2D");
		sprite.Modulate = new Color(1, 1, 1);
	}
	
}


