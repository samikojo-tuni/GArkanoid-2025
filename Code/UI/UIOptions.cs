using System;
using System.Collections.Generic;
using GA.GArkanoid.Systems;
using Godot;

namespace GA.GArkanoid.UI
{
	public partial class UIOptions : Control
	{
		private struct Resolution
		{
			public int Width;
			public int Height;
			public int Multiplyer;

			public override string ToString()
			{
				return $"{Width}x{Height}";
			}

			public static explicit operator Vector2I(Resolution resolution)
			{
				return new Vector2I(resolution.Width, resolution.Height);
			}
		}

		[Export] private Button _okButton = null;
		[Export] private Button _cancelButton = null;
		[Export] private Button _mainMenuButton = null;
		[Export] private UIAudioControl _masterControl = null;
		[Export] private UIAudioControl _musicControl = null;
		[Export] private UIAudioControl _sfxControl = null;
		[Export] private OptionButton _resolutionDropdown = null;
		[Export] private CheckBox _fullscreenToggle = null;
		[Export] private string _masterBusName = null;
		[Export] private string _musicBusName = null;
		[Export] private string _sfxBusName = null;
		[Export] private string _masterBusDisplayName = null;
		[Export] private string _musicBusDisplayName = null;
		[Export] private string _sfxBusDisplayName = null;
		private Dictionary<int, Resolution> _resolutions = new Dictionary<int, Resolution>();

		public override void _Ready()
		{
			InitializeAudioControls();
			InitializeVideoControls();
		}

		public override void _EnterTree()
		{
			_okButton.Pressed += OnOKPressed;
			_cancelButton.Pressed += OnCancelPressed;
			_mainMenuButton.Pressed += OnMainMenuPressed;

			_masterControl.VolumeChanged += OnVolumeChanged;
			_musicControl.VolumeChanged += OnVolumeChanged;
			_sfxControl.VolumeChanged += OnVolumeChanged;

			_fullscreenToggle.Toggled += OnFullScreenToggled;
		}

		public override void _ExitTree()
		{
			_okButton.Pressed -= OnOKPressed;
			_cancelButton.Pressed -= OnCancelPressed;
			_mainMenuButton.Pressed -= OnMainMenuPressed;

			_masterControl.VolumeChanged -= OnVolumeChanged;
			_musicControl.VolumeChanged -= OnVolumeChanged;
			_sfxControl.VolumeChanged -= OnVolumeChanged;

			_fullscreenToggle.Toggled -= OnFullScreenToggled;
		}

		private void InitializeAudioControls()
		{
			bool result = true;
			result = result && SetupAudioControl(_masterControl, _masterBusName, _masterBusDisplayName);
			result = result && SetupAudioControl(_musicControl, _musicBusName, _musicBusDisplayName);
			result = result && SetupAudioControl(_sfxControl, _sfxBusName, _sfxBusDisplayName);

			if (!result)
			{
				GD.PrintErr("Error while initializing audio controls!");
			}
		}

		private void InitializeVideoControls()
		{
			Vector2I viewportSize = GameManager.MinWindowSize;
			int currentScreen = DisplayServer.WindowGetCurrentScreen();
			Vector2I screenSize = DisplayServer.ScreenGetSize(currentScreen);
			int multiplyer = 1;
			int id = 0;

			while (GetWidth(viewportSize, multiplyer) <= screenSize.X &&
				GetHeight(viewportSize, multiplyer) <= screenSize.Y)
			{
				// Add all supported resolutions here.
				Resolution resolution = new Resolution()
				{
					Width = GetWidth(viewportSize, multiplyer),
					Height = GetHeight(viewportSize, multiplyer),
					Multiplyer = multiplyer,
				};

				_resolutions.Add(id, resolution);
				_resolutionDropdown.AddItem(resolution.ToString(), id);

				multiplyer++;
				id++;
			}

			// TODO: Read the correct resolution from settings.
			// TODO: Same for the fullscreen toggle.
		}

		private static int GetWidth(Vector2I viewportSize, int multiplyer)
		{
			return viewportSize.X * multiplyer;
		}

		private static int GetHeight(Vector2I viewportSize, int multiplyer)
		{
			return viewportSize.Y * multiplyer;
		}

		private bool SetupAudioControl(UIAudioControl audioControl, string busName,
			string displayName)
		{
			int index = AudioServer.GetBusIndex(busName);
			if (index >= 0)
			{
				float volume = AudioServer.GetBusVolumeDb(index);
				audioControl.Setup(busName, displayName, volume);
			}

			return index >= 0;
		}

		private void OnFullScreenToggled(bool toggledOn)
		{
			_resolutionDropdown.Disabled = toggledOn;
		}

		private void OnVolumeChanged(string busName, float decibel)
		{
			int index = AudioServer.GetBusIndex(busName);
			if (index >= 0)
			{
				AudioServer.SetBusVolumeDb(index, decibel);
			}
		}

		private void OnMainMenuPressed()
		{
			GameManager.Instance.ChangeState(States.StateType.MainMenu);
		}

		private void OnCancelPressed()
		{
			DiscardChanges();
			CloseSettings();
		}

		private void OnOKPressed()
		{
			ApplySettings();
			SaveSettings();
			CloseSettings();
		}

		private void ApplySettings()
		{
			// Set the resolution
			int resolutionIndex = _resolutionDropdown.Selected;
			if (resolutionIndex < 0)
			{
				// Nothing selected
				return;
			}

			Resolution resolution = _resolutions[resolutionIndex];
			DisplayServer.WindowSetSize((Vector2I)resolution);

			// Set window state
			DisplayServer.WindowMode windowMode = _fullscreenToggle.ButtonPressed
				? DisplayServer.WindowMode.Fullscreen
				: DisplayServer.WindowMode.Windowed;

			DisplayServer.WindowSetMode(windowMode);
		}

		private void SaveSettings()
		{
			// TODO: Implement saving settings!
		}

		private void DiscardChanges()
		{
			// TODO: Implement discarding settings!
		}

		private void CloseSettings()
		{
			// TODO: Redesign the system so that it automatically detects wether to go back or not.
			GameManager.Instance.ActivatePreviousState();
		}
	}
}
