// © 2025 Sami Kojo <sami.kojo@tuni.fi>
// License: 3-Clause BSD License (See the project root folder for details).

using Godot;

namespace GA.GArkanoid
{
	public partial class Wall : Node2D
	{
		[Export]
		public bool IsHazard { get; private set; } = false;
	}
}