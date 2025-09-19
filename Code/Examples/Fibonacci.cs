// © 2025 Sami Kojo <sami.kojo@tuni.fi>
// License: 3-Clause BSD License (See the project root folder for details).
using Godot;
using System;

namespace GA.GArkanoid.Examples
{
	public partial class Fibonacci : Node
	{
		[Export]
		private int _iterations = 10;

		private int _previous = 0;
		private int _current = 0;
		private int _frameCount = 0;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			_previous = 0;
			_current = 1;
			_frameCount = 0;
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
			// Early exit condition. If the iteration count is reached, just exit the method.
			if (_frameCount >= _iterations)
			{
				// Return keyword is used to exit the method.
				return;
			}

			GD.Print($"Frame: {_frameCount}, Value: {_previous}");

			int next = _previous + _current;
			_previous = _current;
			_current = next;

			// Same as: _frameCount = _frameCount + 1;
			_frameCount++;
		}
	}
}