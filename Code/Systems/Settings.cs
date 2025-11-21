using Godot;

namespace GA.GArkanoid.Systems
{
	public class Settings
	{
		private SettingsData _data = null;

		public SettingsData Data
		{
			get => _data;
			set
			{
				_data = value;
				ApplySettings(_data);
			}
		}

		public Settings()
		{
			// Load settings from a file.
			Data = Load();
		}

		public Settings(SettingsData data)
		{
			Data = data;
		}

		/// <summary>
		/// Saves settings stored to the Data property.
		/// </summary>
		public void Save()
		{
			ConfigFile settingsConfig = new ConfigFile();

			settingsConfig.SetValue("Audio", Config.MasterSettingsKey, Data.MasterVolume);
			settingsConfig.SetValue("Audio", Config.MusicSettingsKey, Data.MusicVolume);
			settingsConfig.SetValue("Audio", Config.SFXSettingsKey, Data.SFXVolume);
			settingsConfig.SetValue("Video", Config.ResolutionWidthKey, Data.Resolution.X);
			settingsConfig.SetValue("Video", Config.ResolutionHeightKey, Data.Resolution.Y);
			settingsConfig.SetValue("Video", Config.FullscreenKey, Data.IsFullscreen);

			settingsConfig.Save(Config.SettingsPath);
		}

		/// <summary>
		/// Loads the settings from a settings file. Returns default settings if the file is not found.
		/// </summary>
		public SettingsData Load()
		{
			SettingsData defaultData = SettingsData.Default();
			ConfigFile config = new ConfigFile();
			Error loadError = config.Load(Config.SettingsPath);
			if (loadError != Error.Ok)
			{
				return defaultData;
			}

			SettingsData data = new SettingsData();

			data.MasterVolume = (float)config.GetValue("Audio", Config.MasterSettingsKey, defaultData.MasterVolume);
			data.MusicVolume = (float)config.GetValue("Audio", Config.MusicSettingsKey, defaultData.MusicVolume);
			data.SFXVolume = (float)config.GetValue("Audio", Config.SFXSettingsKey, defaultData.SFXVolume);

			int width = (int)config.GetValue("Video", Config.ResolutionWidthKey, defaultData.Resolution.X);
			int height = (int)config.GetValue("Video", Config.ResolutionHeightKey, defaultData.Resolution.Y);
			data.Resolution = new Vector2I(width, height);
			data.IsFullscreen = (bool)config.GetValue("Video", Config.FullscreenKey, defaultData.IsFullscreen);

			return data;
		}

		private void ApplySettings(SettingsData data)
		{
			AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex(Config.MasterBusName), data.MasterVolume);
			AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex(Config.MusicBusName), data.MusicVolume);
			AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex(Config.SFXBusName), data.SFXVolume);
			DisplayServer.WindowSetSize(data.Resolution);

			DisplayServer.WindowMode windowMode = data.IsFullscreen
				? DisplayServer.WindowMode.Fullscreen
				: DisplayServer.WindowMode.Windowed;
			DisplayServer.WindowSetMode(windowMode);
		}
	}

	public class SettingsData
	{
		public static SettingsData Default()
		{
			// TODO: Do not hard code default values like this. Instead, read them from the config.
			return new SettingsData()
			{
				MasterVolume = -6f,
				MusicVolume = -6f,
				SFXVolume = -6f,
				Resolution = new Vector2I(640, 360),
				IsFullscreen = false
			};
		}

		public float MasterVolume { get; set; }
		public float MusicVolume { get; set; }
		public float SFXVolume { get; set; }
		public Vector2I Resolution { get; set; }
		public bool IsFullscreen { get; set; }

		public SettingsData()
		{
		}

		public SettingsData(SettingsData other)
		{
			MasterVolume = other.MasterVolume;
			MusicVolume = other.MusicVolume;
			SFXVolume = other.SFXVolume;
			Resolution = other.Resolution;
			IsFullscreen = other.IsFullscreen;
		}
	}
}