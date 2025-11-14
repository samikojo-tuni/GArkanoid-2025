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
		public static StringName PauseAction = "Pause";

		#region Initial ball settings
		public const float BallSpeed = 100f;
		public static Vector2 BallDirection = new Vector2(1, -1).Normalized();
		#endregion Initial ball settings

		#region Player data
		public static int MaxLives = 100;
		public static int InitialLives = 3;
		public static int InitialScore = 0;
		#endregion Player data

		#region Audio
		public static StringName MasterBusName = "Master";
		public static StringName MusicBusName = "Music";
		public static StringName SFXBusName = "SFX";
		public static string MusicDataPath = "res://Config/MusicData.tres";
		#endregion
	}
}