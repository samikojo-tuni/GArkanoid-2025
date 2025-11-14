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


		#region Player data
		public static int MaxLives = 100;
		public static string DefaultPlayerDataPath = "res://Config/DefaultPlayerData.tres";
		#endregion Player data

		#region Audio
		public static StringName MasterBusName = "Master";
		public static StringName MusicBusName = "Music";
		public static StringName SFXBusName = "SFX";
		public static string MusicDataPath = "res://Config/MusicData.tres";
		#endregion
	}
}