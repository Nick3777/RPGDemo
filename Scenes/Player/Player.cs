using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	public int Speed { get; set; } = 100;
	[Export]
	public float KnockbackStrength = 200f;
	
	private AnimationTree animTree;
	private AnimationNodeStateMachinePlayback stateMachine;
	private Vector2 previousDirection = Vector2.Zero;
	private string previousState = "";
	private bool isAttacking = false;
	private bool isInvincible = false;
	private Timer invincibilityTimer;
	
	public override void _Ready()
	{
		animTree = GetNode<AnimationTree>("AnimationTree");
		stateMachine = (AnimationNodeStateMachinePlayback)animTree.Get("parameters/playback");
		invincibilityTimer = GetNode<Timer>("InvincibilityTimer");
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if(!isInvincible)
			GetInput();
		MoveAndSlide();
	}
	
//region MoveAndAnimation
	public void GetInput()
	{
		Vector2 inputDirection = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		inputDirection = inputDirection.Normalized();
		isAttacking = Input.IsActionJustPressed("attack");
		SetAnimation(inputDirection);
		Velocity = stateMachine.GetCurrentNode() == "Attack" ? Vector2.Zero : inputDirection * Speed;
	}
	
	public void SetAnimation(Vector2 inputDirection)
	{
		string targetState = DetermineState(inputDirection);
		string parameterPath = $"parameters/{targetState}/blend_position";
		Vector2 blendPosition = inputDirection;
		
		if(targetState == "Idle" || targetState == "Attack")
			blendPosition = previousDirection;
			
		animTree.Set(parameterPath, blendPosition);
		stateMachine.Travel(targetState);
	}

	public string DetermineState(Vector2 inputDirection)
	{
		if (isAttacking)
			return "Attack";
		if (inputDirection != Vector2.Zero)
			{
				previousDirection = inputDirection;
				return "Walk";
			}			 
		return "Idle";
	}
//endregion

	private void TakeEnemyDmg(string enemy, Vector2 enemyPosition)
	{
		if (isInvincible) return;
			
		PlayerStats.currentHealth -= EnemiesDamage.enemyDamage[enemy];
		GD.Print(PlayerStats.currentHealth);		
		Vector2 knockbackDirection = (GlobalPosition - enemyPosition).Normalized();
		Velocity = knockbackDirection * KnockbackStrength;
		
		if(PlayerStats.currentHealth == 0) this.QueueFree();
		
		isInvincible = true;
		invincibilityTimer.Start();
	}
	
	private void onHurtboxAreaEntered(Area2D area)
	{
		if(area.IsInGroup("EnemyHitbox"))
		{
			string enemy = area.GetParent().Name;
			Vector2 enemyPosition = area.GetParent<Node2D>().GlobalPosition;;
			TakeEnemyDmg(enemy, enemyPosition);
		}
	}
	
	private void onInvincibilityTimerTimeout()
	{
		isInvincible = false;
	}
}



