// © 2025 Sami Kojo <sami.kojo@tuni.fi>
// License: 3-Clause BSD License (See the project root folder for details).

using System;
using GA.Common;
using GA.GArkanoid.Physics;
using Godot;

namespace GA.GArkanoid
{
	public partial class CustomBall : Sprite2D
	{
		/// <summary>
		/// The movement direction. This vector should always be normalized.
		/// </summary>
		[Export]
		private Vector2 _direction = Vector2.Zero;

		/// <summary>
		/// The movement speed (in pixels / second).
		/// </summary>
		[Export]
		private float _speed = 100f;

		/// <summary>
		/// Contains references to every wall in the scene.
		/// </summary>
		[Export]
		private Sprite2D[] _walls = null;

		/// <summary>
		/// The radius of the ball (for optional challenge).
		/// </summary>
		[Export]
		private float _radius = 16f;

		[Export]
		private bool _useCircleCollision = true;

		public Vector2 Velocity
		{
			get { return _direction * _speed; }
		}

		public override void _Ready()
		{
			_direction = _direction.Normalized();
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
			// Move the ball by modifying its global position each frame.

			Vector2 movement = Velocity * (float)delta;
			Vector2 newPosition = GlobalPosition + movement;

			// TODO: Calculate possible collisions with walls here.
			// TODO: Bounce the ball in case of a collision.
			foreach (Sprite2D wall in _walls)
			{
				CustomPhysics.Hit hit = _useCircleCollision
					? CustomPhysics.Intersects(wall.GetBoundingBox(), newPosition, _radius)
					: CustomPhysics.Intersects(wall.GetBoundingBox(), newPosition);

				if (hit != null)
				{
					float radius = _useCircleCollision ? _radius : 0;
					// Collision happened
					_direction = CustomPhysics.Bounce(_direction, hit.Normal).Normalized();

					Vector2 remaining = _direction * hit.Delta;
					newPosition = hit.Point + hit.Normal * radius + remaining;

					// TODO: Will this cause issues (if the ball collides with another wall after the bounce?)
					break;
				}
			}


			GlobalPosition = newPosition;
		}

		/// <summary>
		/// Demonstrates how to use extension methods.
		/// </summary>
		private void ExtensionExamples()
		{
			Vector2 spriteSize = this.Texture.GetSize();
			// Extension methods can be used like they were methods (member functions).
			Rect2 boundingBox = this.GetBoundingBox();
			// Same as below
			boundingBox = NodeExtensions.GetBoundingBox(this);

			Vector2 extents = boundingBox.GetExtents();
			// Same as
			extents = NodeExtensions.GetExtents(boundingBox);
		}

	}
}