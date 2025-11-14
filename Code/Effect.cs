// © 2025 Sami Kojo <sami.kojo@tuni.fi>
// License: 3-Clause BSD License (See the project root folder for details).

using Godot;

namespace GA.GArkanoid
{
	public partial class Effect : Node2D
	{
		[Export]
		public GpuParticles2D ParticleEffect { get; private set; }

		[Export]
		public AudioStreamPlayer Audio { get; set; }

		private int _effectCount = 0;

		// TODO: Add audio effect as well

		public void Play()
		{
			if (ParticleEffect != null)
			{
				_effectCount++;
				ParticleEffect.OneShot = true;
				ParticleEffect.Emitting = true;

				ParticleEffect.Finished += OnParticleEffectFinished;
			}

			if (Audio != null)
			{
				_effectCount++;
				Audio.Play();

				Audio.Finished += OnAudioEffectFinished;
			}
		}

		private void OnAudioEffectFinished()
		{
			Audio.Finished -= OnAudioEffectFinished;
			OnEffectFinished();
		}

		private void OnParticleEffectFinished()
		{
			ParticleEffect.Finished -= OnParticleEffectFinished;
			OnEffectFinished();
		}

		// TODO: Add a fallback timer
		private void OnEffectFinished()
		{
			_effectCount--;
			if (_effectCount <= 0)
			{
				// TODO: Instead of destroying the object, we should pool inactive objects and reuse them.
				// Will be implemented suring the spring.
				QueueFree();
			}
		}
	}
}