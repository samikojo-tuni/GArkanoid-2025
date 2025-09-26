// © 2025 Sami Kojo <sami.kojo@tuni.fi>
// License: 3-Clause BSD License (See the project root folder for details).

using System;
using Godot;

namespace GA.GArkanoid
{
	public partial class Paddle : CharacterBody2D
	{
		private float _horizontalInput = 0.0f;
		private float _mouseInput = 0.0f;
		private int _minX = 0;
		private int _maxX = 0;
		private Sprite2D _sprite = null;

		/// <summary>
		/// The speed of the paddle (pixels / second);
		/// </summary>
		/// <value></value>
		[Export]
		public float Speed { get; set; } = 100f;

		#region Public Interface
		public override void _Ready()
		{
			_sprite = GetNode<Sprite2D>("CollisionShape2D/PaddleSprite");
			UpdateWorldBounds();
		}

		public override void _Process(double delta)
		{
			_horizontalInput = Input.GetAxis(Config.MoveLeftAction, Config.MoveRightAction);
		}

		public override void _Input(InputEvent @event)
		{
			if (@event is InputEventMouseMotion mouseMotionEvent)
			{
				_mouseInput += mouseMotionEvent.Relative.X;
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
				movement *= Speed * (float)delta;
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
		#endregion Public Interface

		#region Private implementation
		private void UpdateWorldBounds()
		{
			Rect2 viewPortRect = GetViewport().GetVisibleRect();
			_minX = (int)viewPortRect.Position.X;
			_maxX = (int)viewPortRect.End.X;
		}
		#endregion Private implementation
	}
}