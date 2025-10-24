using System;
using Godot;

namespace GA.GArkanoid
{
	public partial class Wall : Node2D
	{
		[Export]
		public bool IsHazard { get; private set; } = false;
	}
}