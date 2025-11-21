// © 2025 Sami Kojo <sami.kojo@tuni.fi>
// License: 3-Clause BSD License (See the project root folder for details).

using System;
using GA.Common;
using GA.GArkanoid.Save;
using GA.GArkanoid.Systems;
using Godot;
using Godot.Collections;

namespace GA.GArkanoid
{
	public partial class Paddle : CharacterBody2D, ISave
	{
		private float _horizontalInput = 0.0f;
		private float _mouseInput = 0.0f;
		private int _minX = 0;
		private int _maxX = 0;
		private Sprite2D _sprite = null;
		private CollisionShape2D _collisionShape = null;
		private float _speed = 0;
		private float _width = 1;

		/// <summary>
		/// The position on top of the paddle where the ball
		/// should be attached to before it is launched.
		/// </summary>
		[Export]
		private Node2D _ballLaunchPoint = null;

		public Ball CurrentBall { get { return LevelManager.Active.CurrentBall; } }

		public float Width
		{
			get => _width;
			set
			{
				_width = value;
				UpdatePaddleWidth();
			}
		}

		#region Public Interface
		public override void _Ready()
		{
			_collisionShape = this.GetNode<CollisionShape2D>();
			_sprite = GetNode<Sprite2D>("CollisionShape2D/PaddleSprite");
			UpdateWorldBounds();

			_speed = GameManager.Instance.CurrentPlayerData.PaddleSpeed;
		}

		public override void _Process(double delta)
		{
			_horizontalInput = Input.GetAxis(Config.MoveLeftAction, Config.MoveRightAction);

			if (CurrentBall != null && !CurrentBall.IsLaunched)
			{
				// The ball should follow the paddle before it is launched.
				CurrentBall.GlobalPosition = _ballLaunchPoint.GlobalPosition;
			}
		}

		public override void _Input(InputEvent @event)
		{
			if (@event is InputEventMouseMotion mouseMotionEvent)
			{
				_mouseInput += mouseMotionEvent.Relative.X;
			}

			if (CurrentBall != null && @event.IsActionPressed(Config.LaunchAction))
			{
				CurrentBall.Launch(GameManager.Instance.CurrentPlayerData.BallSpeed,
					GameManager.Instance.CurrentPlayerData.LaunchDirection.Normalized());
			}
		}

		public override void _PhysicsProcess(double delta)
		{
			float halfWidth = _sprite.Texture.GetWidth() * GlobalScale.X * 0.5f;
			float minX = _minX + halfWidth;
			float maxX = _maxX - halfWidth;

			if (Mathf.IsZeroApprox(_mouseInput))
			{
				// There is no mouse movement, let's poll horizontal axis instead.
				Vector2 movement = new Vector2(_horizontalInput, 0f);
				movement *= _speed * (float)delta;
				MoveAndCollide(movement);

				// "Consume" the input. Otherwise this could work incorrectly e.g. in the case when user's
				// controller's battery runs out.
				_horizontalInput = 0f;
			}
			else
			{
				// This implementation ignores the paddle speed. It feels way better for the player,
				// though.
				Vector2 movement = new Vector2(_mouseInput, 0f);
				MoveAndCollide(movement);
				_mouseInput = 0;
			}

			// HACK: Instead of clamping the position, you should resize the movement vector
			// for physics to work correctly for sure.
			Position = new Vector2(Mathf.Clamp(Position.X, minX, maxX), Position.Y);
		}

		public void Expand(float multiplyer)
		{
			Width *= multiplyer;
		}

		public void Shrink(float multiplyer)
		{
			Width *= (1 / multiplyer);
		}
		#endregion Public Interface

		#region Private implementation
		private void UpdateWorldBounds()
		{
			Rect2 viewPortRect = GetViewport().GetVisibleRect();
			_minX = (int)viewPortRect.Position.X;
			_maxX = (int)viewPortRect.End.X;
		}

		private void UpdatePaddleWidth()
		{
			if (_collisionShape != null)
			{
				_collisionShape.Scale = new Vector2(Scale.X, _width);
			}
		}

		public Dictionary Save()
		{
			Dictionary positionData = new Dictionary
			{
				{"X", GlobalPosition.X},
				{"Y", GlobalPosition.Y}
			};

			Dictionary paddleData = new Dictionary();
			paddleData["Speed"] = _speed;
			paddleData["Position"] = positionData;

			return paddleData;
		}

		public void Load(Dictionary data)
		{
			_speed = (float)data["Speed"];

			Dictionary positionData = (Dictionary)data["Position"];
			GlobalPosition = new Vector2((float)positionData["X"], (float)positionData["Y"]);
		}
		#endregion Private implementation
	}
}