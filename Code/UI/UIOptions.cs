using System;
using GA.GArkanoid.Systems;
using Godot;

namespace GA.GArkanoid.UI
{
	public partial class UIOptions : Control
	{
		[Export] private Button _okButton = null;
		[Export] private Button _cancelButton = null;
		[Export] private UIAudioControl _masterControl = null;
		[Export] private UIAudioControl _musicControl = null;
		[Export] private UIAudioControl _sfxControl = null;
		[Export] private string _masterBusName = null;
		[Export] private string _musicBusName = null;
		[Export] private string _sfxBusName = null;
		[Export] private string _masterBusDisplayName = null;
		[Export] private string _musicBusDisplayName = null;
		[Export] private string _sfxBusDisplayName = null;

		public override void _Ready()
		{
			InitializeAudioControls();
			InitializeVideoControls();
		}

		public override void _EnterTree()
		{
			_okButton.Pressed += OnOKPressed;
			_cancelButton.Pressed += OnCancelPressed;

			_masterControl.VolumeChanged += OnVolumeChanged;
			_musicControl.VolumeChanged += OnVolumeChanged;
			_sfxControl.VolumeChanged += OnVolumeChanged;
		}

		public override void _ExitTree()
		{
			_okButton.Pressed -= OnOKPressed;
			_cancelButton.Pressed -= OnCancelPressed;

			_masterControl.VolumeChanged -= OnVolumeChanged;
			_musicControl.VolumeChanged -= OnVolumeChanged;
			_sfxControl.VolumeChanged -= OnVolumeChanged;
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
			// TODO: Implement me!
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

		private void OnVolumeChanged(string busName, float decibel)
		{
			int index = AudioServer.GetBusIndex(busName);
			if (index >= 0)
			{
				AudioServer.SetBusVolumeDb(index, decibel);
			}
		}

		private void OnCancelPressed()
		{
			DiscardChanges();
			CloseSettings();
		}

		private void OnOKPressed()
		{
			SaveSettings();
			CloseSettings();
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
