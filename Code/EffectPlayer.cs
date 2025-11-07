using System;
using System.Collections.Generic;
using GA.GArkanoid.Data;
using Godot;

namespace GA.GArkanoid
{
	public enum EffectType
	{
		None = 0,
		Bounce,
		Hit,
		Death,
	}

	public partial class EffectPlayer : Node2D
	{
		[Export] public EffectMap EffectMap { get; private set; }

		public override void _Ready()
		{
			if (EffectMap == null)
			{
				GD.PrintErr($"Can't play effects, the {nameof(EffectMap)} is not set!");
			}
		}

		public void PlayEffect(EffectType effectType, Vector2 position)
		{
			PackedScene effectScene = EffectMap.GetSceneResource(effectType);
			if (effectScene != null)
			{
				Effect effect = effectScene.Instantiate<Effect>();
				if (effect != null)
				{
					AddChild(effect);
					effect.GlobalPosition = position;
					effect.Play();
				}
			}
		}
	}
}