using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 200.0f;
	public const float Dash = 600.0f;
	public const float JumpVelocity = -300.0f;
	public const float WallJumpVelocity = -300.0f;

	public float gravity = 900.0f;
	public const float wallgravity = 1000.0f;
	public float dashTime = 0.15f;
	public float dashTimer = 0f;

	// Dash cooldown
	public float dashCooldown = 1.0f;
	public float dashCooldownTimer = 0f;

	public float jumpcount = 0f;
	public float maxjump = 2f;

	public int direction;
	public bool isdashing = false;
	public Area2D killing;
	public AnimationPlayer Pogo;

	public AnimatedSprite2D Animation;

	public override void _Ready()
	{
		Animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		killing = GetNode<Area2D>("killing");
		Pogo = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	private void OnArea2dAreaEntered(Area2D area)
	{
		if (area.IsInGroup("saw"))
		{
			GD.Print("ENTEREDDDDD: ", area.Name);
			Velocity = new Vector2(Velocity.X, JumpVelocity);
			jumpcount = 1;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Input.IsActionJustPressed("restart"))
{
    GetTree().ReloadCurrentScene();
}
		Vector2 velocity = Velocity;

		// Dash cooldown timer
		if (dashCooldownTimer > 0)
			dashCooldownTimer -= (float)delta;

		if (!IsOnWall())
			velocity.Y += gravity * (float)delta;

		if (Input.IsActionPressed("ui_down") && Input.IsActionPressed("ui_attack"))
		{
			Pogo.Play("poggo");
		}

		if (IsOnFloor() || IsOnWall())
		{
			jumpcount = 0;
		}

		if (IsOnWall())
		{
			velocity.Y += wallgravity * (float)delta;
		}

		if (isdashing == false)
		{
			velocity.X = 0;

			if (Input.IsActionPressed("ui_right"))
			{
				velocity.X = Speed;
				direction = 1;
				Animation.Play("run");
				Animation.FlipH = false;
			}
			else if (Input.IsActionPressed("ui_left"))
			{
				velocity.X -= Speed;
				direction = -1;
				Animation.FlipH = true;
				Animation.Play("run");
			}
			else
			{
				Animation.Play("idle");
			}

			if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
				velocity.Y = JumpVelocity;

			if (IsOnWall() && Input.IsActionJustPressed("ui_accept"))
				velocity.Y = WallJumpVelocity;
		}

		// Dash with 1 second cooldown (works on ground and in air)
		if (Input.IsActionJustPressed("ui_dash") && dashCooldownTimer <= 0)
		{
			isdashing = true;
			gravity = 0;
			dashTimer = dashTime;
			dashCooldownTimer = dashCooldown;
		}

		if (isdashing)
		{
			velocity.X = Dash * direction;
			velocity.Y = 0;
			dashTimer -= (float)delta;

			if (dashTimer <= 0)
			{
				gravity = 900;
				isdashing = false;
			}
		}

		if (Input.IsActionJustPressed("ui_accept") && jumpcount < maxjump)
		{
			velocity.Y = JumpVelocity;
			jumpcount++;
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}