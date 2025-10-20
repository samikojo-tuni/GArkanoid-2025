// © 2025 Sami Kojo <sami.kojo@tuni.fi>
// License: 3-Clause BSD License (See the project root folder for details).

using System;
using GA.GArkanoid.Systems;
using Godot;

namespace GA.GArkanoid
{
	public partial class Block : StaticBody2D
	{
		[Export]
		private Color _blockColor = Colors.White;

		[Export]
		private int _score = 0;

		// TODO: Add support for health.

		private Sprite2D _sprite = null;

		public override void _Ready()
		{
			_sprite = GetNode<Sprite2D>("Sprite2D");
			if (_sprite != null)
			{
				_sprite.Modulate = _blockColor;
			}
		}

		public void Hit()
		{
			// TODO: Implement the Health system
			QueueFree();

			// Access GameManager singleton using its Instance property.
			GameManager.Instance.AddScore(_score);
		}
	}
}