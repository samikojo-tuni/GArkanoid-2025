// © 2025 Sami Kojo <sami.kojo@tuni.fi>
// License: 3-Clause BSD License (See the project root folder for details).

using System;
using Godot;

namespace GA.GArkanoid
{
	public partial class AreaCollisionTest : Area2D
	{
		public override void _Ready()
		{
			BodyEntered += OnBodyEntered;
		}

		private void OnBodyEntered(Node2D body)
		{
			if (body is Ball ball)
			{
				GD.Print("Collided with a ball");
			}
		}
	}
}