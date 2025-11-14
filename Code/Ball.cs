// © 2025 Sami Kojo <sami.kojo@tuni.fi>
// License: 3-Clause BSD License (See the project root folder for details).

using System;
using GA.GArkanoid.Save;
using GA.GArkanoid.Systems;
using Godot;
using Godot.Collections;

namespace GA.GArkanoid
{
	public partial class Ball : CharacterBody2D, ISave
	{
		public float Speed { get; private set; } = 0.0f;

		// The direction of the ball. Initial value Vector2.Zero means that
		// the ball is not moving.
		public Vector2 Direction { get; private set; } = Vector2.Zero;

		public bool IsLaunched
		{
			get { return !Direction.IsZeroApprox(); }
		}

		public override void _PhysicsProcess(double delta)
		{
			// If the ball is not launched, just return. Nothing to do here.
			if (!IsLaunched)
			{
				return;
			}

			KinematicCollision2D collisionData = MoveAndCollide(Velocity * (float)delta);
			if (Bounce(collisionData))
			{
				GodotObject collidedObject = collisionData.GetCollider();
				if (collidedObject is Block block)
				{
					LevelManager.Active.EffectPlayer.PlayEffect(EffectType.Hit, GlobalPosition);
					block.Hit();
				}
				else if (collidedObject is Wall wall && wall.IsHazard)
				{
					LevelManager.Active.EffectPlayer.PlayEffect(EffectType.Death, GlobalPosition);
					GameManager.Instance.DecreaseLives();
				}
				else
				{
					// Regular collision, nothing special
					// TODO: Play effect
					LevelManager.Active.EffectPlayer.PlayEffect(EffectType.Bounce, GlobalPosition);
				}
			}
		}

		private bool Bounce(KinematicCollision2D collisionData)
		{
			if (collisionData != null)
			{
				Direction = Direction.Bounce(collisionData.GetNormal()).Normalized();
				Velocity = Direction * Speed;
			}

			return collisionData != null;
		}

		/// <summary>
		/// Launches the ball.
		/// Precondition: Direction should be normalized before calling this method.
		/// </summary>
		/// <param name="speed">The speed of the ball</param>
		/// <param name="direction">The direction the ball is launched to.
		/// Has to be normalized.</param>
		public void Launch(float speed, Vector2 direction)
		{
			Speed = speed;
			Direction = direction;
			Velocity = Direction * Speed;
		}

		public void Reset()
		{
			Speed = 0;
			Direction = Vector2.Zero;
			Velocity = Vector2.Zero;
		}

		public Dictionary Save()
		{
			throw new NotImplementedException();
		}

		public void Load(Dictionary data)
		{
			throw new NotImplementedException();
		}
	}
}