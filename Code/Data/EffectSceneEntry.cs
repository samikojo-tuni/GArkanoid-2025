// © 2025 Sami Kojo <sami.kojo@tuni.fi>
// License: 3-Clause BSD License (See the project root folder for details).

using Godot;

namespace GA.GArkanoid.Data
{
	[Tool, GlobalClass]
	public partial class EffectSceneEntry : Resource
	{
		[Export]
		public EffectType EffectType { get; set; }

		[Export]
		public PackedScene Scene { get; set; }
	}
}