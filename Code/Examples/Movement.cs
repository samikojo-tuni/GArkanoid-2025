// © 2025 Sami Kojo <sami.kojo@tuni.fi>
// License: 3-Clause BSD License (See the project root folder for details).
using Godot;
using System;

namespace GA.GArkanoid.Examples
{
	public partial class Movement : Node2D
	{
		public enum InputType
		{
			None = 0,
			SeparateActions,
			VectorActions
		}

		[Export]
		private InputType _inputType = InputType.None;

		/// <summary>
		/// The speed of a mover in pixels per second.
		/// </summary>
		[Export]
		private float _speed = 100;

		public override void _Input(InputEvent @event)
		{
			if (@event.IsActionPressed("Jump"))
			{
				GD.Print("Jump!");
			}
		}


		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
			// Early exit condition
			if (_inputType == InputType.None)
			{
				return;
			}

			Vector2 direction = Vector2.Zero;
			switch (_inputType)
			{
				case InputType.SeparateActions:
					direction = ReadInputActionsSeparately();
					// The direction vector normalized. Normalizing a vector means that we scale it so that its
					// length is one.
					direction = direction.Normalized();
					break;
				case InputType.VectorActions:
					direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down", deadzone: 0.1f);
					break;
				default:
					break;
			}

			if (direction != Vector2.Zero)
			{
				Vector2 movement = direction * _speed * (float)delta;
				Position += movement;

				float currentSpeed = direction.Length() * _speed;
				GD.Print($"Current speed: {currentSpeed}");
			}
		}

		private Vector2 ReadInputActionsSeparately()
		{
			Vector2 inputVector = new Vector2();
			if (Input.IsActionPressed("ui_left"))
			{
				// Moving left. Returns true every frame left action is pressed.
				inputVector.X -= 1;
			}
			if (Input.IsActionPressed("ui_right"))
			{
				inputVector.X += 1;
			}
			if (Input.IsActionPressed("ui_up"))
			{
				inputVector.Y -= 1;
			}
			if (Input.IsActionPressed("ui_down"))
			{
				inputVector.Y += 1;
			}

			return inputVector;
		}
	}
}