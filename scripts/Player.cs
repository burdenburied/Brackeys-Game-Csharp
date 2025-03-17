using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public partial class Player : CharacterBody2D
{
	public const float Speed = 130.0f;
	public const float JumpVelocity = -300.0f;
	private AnimatedSprite2D animated_sprite;
	private AudioStreamPlayer2D jump_sfx;

    public override void _Ready()
    {
        animated_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		jump_sfx = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
    }

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
			jump_sfx.Play();
		}


		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("move_left", "move_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		// Flip sprites
		if (direction > new Vector2(0, 0))
		{
			animated_sprite.FlipH = false;
		}
		if (direction < new Vector2(0, 0))
		{
			animated_sprite.FlipH = true;
		}

		//Run animation
		if (IsOnFloor())
		{
			if (direction != Vector2.Zero)
			{
				animated_sprite.Play("run");
			}
			else
			{
				animated_sprite.Play("idle");
			}
		}
		else 
		{
			animated_sprite.Play("jump");
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
