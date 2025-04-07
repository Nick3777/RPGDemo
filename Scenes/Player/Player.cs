using Godot;
using System;
using System.Linq;

public partial class Player : CharacterBody2D
{
	[Export]
	public int Speed { get; set; } = 100;
	[Export]
	public float KnockbackStrength = 100f;
	string[] actions = { "Attack", "Chop", "Death" };
	private PlayerActions playerActions;
	
	private AnimationTree animTree;
	private AnimationNodeStateMachinePlayback stateMachine;
	private Vector2 previousDirection = Vector2.Zero;
	private string previousState = "";
	private string currentNode = "";
	private bool isAttacking = false;
	private bool isInvincible = false;
	private bool isChopping = false;
	private bool isDoingAction = false;
	private Timer invincibilityTimer;
	

	public override void _Ready()
	{
		playerActions = new PlayerActions();
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
		SetAnimation(inputDirection);
		ActionDetection(inputDirection);
		Velocity = isDoingAction ? Vector2.Zero : inputDirection * Speed;
	}
	
	public void SetAnimation(Vector2 inputDirection)
	{
		string targetState = DetermineState(inputDirection);
		string parameterPath = $"parameters/{targetState}/blend_position";
		Vector2 blendPosition = inputDirection;
		
		if(!(targetState == "Move"))
			blendPosition = previousDirection;
			
		animTree.Set(parameterPath, blendPosition);
		stateMachine.Travel(targetState);
		if(targetState == "Death")
			Death();
	}

	public string DetermineState(Vector2 inputDirection)
	{	
		if(PlayerStats.currentHealth <= 0)
			return "Death";
		if(Input.IsActionJustPressed("attack"))
			return "Attack";
		if(Input.IsActionJustPressed("chop"))
			return "Chop";

		if (inputDirection != Vector2.Zero)
			{
				previousDirection = inputDirection;
				return "Walk";
			}			 
		return "Idle";
	}
	
	private void ActionDetection(Vector2 inputDirection)
	{
		currentNode = stateMachine.GetCurrentNode();
		if(actions.Contains(currentNode))
			isDoingAction = true;
		else
			isDoingAction = false;	
	}
//endregion

//region HandleDmg&Attack
	private void TakeEnemyDmg(string enemy, Vector2 enemyPosition)
	{
		if (isInvincible) return;
			
		PlayerStats.currentHealth -= EnemiesDamage.enemyDamage[enemy];	
		Vector2 knockbackDirection = (GlobalPosition - enemyPosition).Normalized();
		Velocity = knockbackDirection * KnockbackStrength;
		
		if(PlayerStats.currentHealth <= 0) 
			Death();
		
		isInvincible = true;
		invincibilityTimer.Start();
	}
	
	private void Death()
	{	
		this.CollisionLayer = 0;
		this.CollisionMask = 0;
		PlayerStats.isDead = true;	
			
		if(stateMachine.GetCurrentNode() == "End")
			this.QueueFree();
			
	}
//endregion
	
//region Signals
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
	private void onSwordAreaEntered(Area2D enemyHurtbox)
	{
		if(enemyHurtbox.IsInGroup("EnemyHitbox"))
			playerActions.Attack(enemyHurtbox);

//endregion
}
