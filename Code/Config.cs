// © 2025 Sami Kojo <sami.kojo@tuni.fi>
// License: 3-Clause BSD License (See the project root folder for details).

using Godot;

namespace GA.GArkanoid
{
	public static class Config
	{
		public static StringName MoveLeftAction = "MoveLeft";
		public static StringName MoveRightAction = "MoveRight";
		public static StringName LaunchAction = "Launch";

		#region Initial ball settings
		public const float BallSpeed = 100f;
		public static Vector2 BallDirection = new Vector2(1, -1).Normalized();
		#endregion Initial ball settings
	}
}