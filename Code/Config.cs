// © 2025 Sami Kojo <sami.kojo@tuni.fi>
// License: 3-Clause BSD License (See the project root folder for details).

using System.IO;
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
		public const int LevelCount = 2;
		#endregion Player data

		#region Audio
		public static StringName MasterBusName = "Master";
		public static StringName MusicBusName = "Music";
		public static StringName SFXBusName = "SFX";
		public static string MusicDataPath = "res://Config/MusicData.tres";

		public static string MusicSettingsKey = "Music";
		public static string SFXSettingsKey = "SFX";
		public static string MasterSettingsKey = "Master";
		public static string ResolutionWidthKey = "ResolutionX";
		public static string ResolutionHeightKey = "ResolutionY";
		public static string FullscreenKey = "Fullscreen";
		#endregion Audio

		#region Save
		public static string QuickSaveName = "QuickSave";
		public static string SaveFolderName = "Save";
		public static string SaveFileExtension = ".json";
		public static string PlayerDataKey = "PlayerData";
		public static string LevelDataKey = "LevelData";
		public static StringName QuickSaveAction = "QuickSave";

		public static string SettingsPath = "user://Settings/settings.cfg";

		public static string GetSaveFolderPath()
		{
			// The path on user's disk where save files can be written.
			string path = ProjectSettings.GlobalizePath("user://");
			// Path.Combine selects corrent path separation character (/ or \) based on the user's platform.
			path = Path.Combine(path, SaveFolderName);
			return path;
		}
		#endregion Save
	}
}