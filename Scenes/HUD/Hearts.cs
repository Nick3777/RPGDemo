using Godot;
using System;

public partial class Hearts : PanelContainer
{
	private Texture heartTexture = GD.Load<Texture>("res://Assets/Player/PixelHeart.png");
	private int currentHealth;
	private int maxHealth;
	private HBoxContainer heartsContainer;

	public override void _Ready()
	{
		heartsContainer = GetNode<HBoxContainer>("HBoxContainer");
		currentHealth = PlayerStats.currentHealth;
		maxHealth = PlayerStats.maxHealth;
		UpdateHearts();
	}

	public override void _Process(double delta)
	{
		GD.Print(currentHealth);
		if(currentHealth != PlayerStats.currentHealth)
		{
			currentHealth = PlayerStats.currentHealth;
			UpdateHearts();
		}
	}
	
	private void UpdateHearts()
	{
		
		GD.Print("currentHealthDamn");
		foreach (Node child in heartsContainer.GetChildren())
		{
			heartsContainer.RemoveChild(child);
			child.QueueFree();
		}
		
		for (int i = 0; i < maxHealth; i++)
		{
			TextureRect heart = new TextureRect();
			heart.Texture = (Texture2D)heartTexture;
			if (i >= currentHealth)
			{
				heart.Modulate = new Color(1, 1, 1, 0.3f); 
			}
			heartsContainer.AddChild(heart);
		}
	}
}
