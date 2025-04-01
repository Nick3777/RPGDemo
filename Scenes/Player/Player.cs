using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	public int Speed { get; set; } = 100;
	
	private AnimationTree animTree;
	private AnimationNodeStateMachinePlayback stateMachine;
	private Vector2 previousDirection = Vector2.Zero;
	private string previousState = "";
	private bool isAttacking = false;
	
	public override void _Ready()
	{
		animTree = GetNode<AnimationTree>("AnimationTree");
		stateMachine = (AnimationNodeStateMachinePlayback)animTree.Get("parameters/playback");
	}
	
	public override void _PhysicsProcess(double delta)
	{
		GetInput();
		MoveAndSlide();
	}
	
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
}
