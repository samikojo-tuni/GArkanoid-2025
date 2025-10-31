using System;
using Godot;

namespace GA.GArkanoid.UI
{
	public partial class UIAudioControl : Control
	{
		[Signal] public delegate void VolumeChangedEventHandler(string busName, float decibel);

		[Export] private Label _nameLabel = null;
		[Export] private Slider _volumeSlider = null;
		[Export] private Label _valueLabel = null;
		private string _busName = null;

		public override void _EnterTree()
		{
			_volumeSlider.ValueChanged += OnVolumeChanged;
		}

		public override void _ExitTree()
		{
			_volumeSlider.ValueChanged -= OnVolumeChanged;
		}

		public void Setup(string busName, string displayName, float decibelVolume)
		{
			_busName = busName;
			_nameLabel.Text = displayName;
			SetVolume(decibelVolume);
		}

		private void SetVolume(float decibelVolume)
		{
			// Volume on a linear scale [0,1].
			float linearVolume = Mathf.DbToLinear(decibelVolume);
			_volumeSlider.Value = linearVolume;
		}

		private void OnVolumeChanged(double value)
		{
			UpdateVolume();
		}

		private void UpdateVolume()
		{
			// Volume on a linear scale [0,1].
			float linearVolume = (float)_volumeSlider.Value;

			// Volume on a decibel scale [-80,0].
			float decibelVolume = Mathf.LinearToDb(linearVolume);

			// Update volume label
			_valueLabel.Text = ((int)(linearVolume * 100)).ToString();

			EmitSignal(SignalName.VolumeChanged, _busName, decibelVolume);
		}
	}
}