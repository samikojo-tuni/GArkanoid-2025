namespace GA.GArkanoid
{
	public interface IPowerUpEffect
	{
		float Duration { get; }
		// The set accessor is public here because we will implelent stacking existing effects.
		float TimeRemaining { get; set; }
		bool IsActive { get; }
		PowerUpType PowerUpType { get; }

		void Activate();
		bool Update(float deltaTime);
		void Deactivate();
	}
}