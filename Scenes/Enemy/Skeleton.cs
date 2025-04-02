using Godot;
using System;

public partial class Skeleton : CharacterBody2D
{
	[Export]
	public int Speed { get; set; } = 20;
	
	private Area2D wanderArea;
	private Vector2 wanderTarget;
	private Timer pauseTimer;
	private Player target;
	CircleShape2D wanderCircle;
	
	private bool isPaused = false;
	private bool isChasing = false;
	
	public override void _Ready()
	{
		wanderArea = GetNode<Area2D>("WanderArea");
		pauseTimer = GetNode<Timer>("PauseTimer");
		wanderArea.GlobalPosition = GlobalPosition;
		wanderTarget  = GlobalPosition;
		wanderCircle = wanderArea.GetNode<CollisionShape2D>("CollisionShape2D").Shape as CircleShape2D;
	}

	public override void _PhysicsProcess(double delta)
	{			
		if(GetChasingStatus())
			HandleChasingState();
		else
			HandleWanderingState();
		
		MoveAndSlide();
	}
	
//region Movement
	private bool GetChasingStatus()
	{
		if (GlobalPosition.DistanceTo(wanderArea.GlobalPosition) > wanderCircle.Radius)
			isChasing = false;
		
		return isChasing;
	}
	
	private void HandleChasingState()
	{
		onPauseTimeout();
		pauseTimer.Stop();
		Vector2 targetDirection = (target.GlobalPosition - GlobalPosition).Normalized();
		Velocity = targetDirection * Speed;
	}
	
	private void HandleWanderingState()
	{
		Vector2 direction = (wanderTarget - GlobalPosition).Normalized();
		if (isPaused) 
			return;

		if(GlobalPosition.DistanceTo(wanderTarget) <= 0.2f)
		{
			isPaused = true;
			pauseTimer.WaitTime = (float)GD.RandRange(1.0, 6.0);
			pauseTimer.Start();
			Velocity = Vector2.Zero;
			return;
		}

		Velocity = direction * Speed;
	}
	
	private Vector2 RandPosWander()
	{
		float radius = wanderCircle.Radius;
		float angle = GD.Randf() * Mathf.Tau; // Mathf.Tau equivale a 2*PI
		Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * GD.Randf() * radius;
		return wanderArea.GlobalPosition + offset;
	}
//endregion
	
//region Signals
	private void onPauseTimeout()
	{
		isPaused = false;
		wanderTarget = RandPosWander();
	}
	
	private void onChasingAreaBodyEntered(Node2D body)
	{
		if(body.IsInGroup("Player"))
		{
			isChasing = true;
			target = (Player)body;
		}
	}

	private void onChasingAreaBodyExited(Node2D body)
	{
		if(body.IsInGroup("Player"))
			isChasing = false;
	}
//endregion

}
