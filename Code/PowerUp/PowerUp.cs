using Godot;

namespace GA.GArkanoid
{
	public partial class PowerUp : Area2D
	{
		[Signal] public delegate void PowerUpCollectedEventHandler(PowerUp collected);
		[Signal] public delegate void PowerUpExpiredEventHandler(PowerUp expired);
		[Export] public PowerUpType Type { get; set; } = PowerUpType.None;
		[Export] private float _fallSpeed = 100;
		[Export] private float _despawnTime = 5;

		public override void _EnterTree()
		{
			BodyEntered += OnBodyEntered;
		}

		public override void _ExitTree()
		{
			BodyEntered -= OnBodyEntered;
		}

		public override void _Process(double delta)
		{
			// Falling down movement
			GlobalPosition += Vector2.Down * _fallSpeed * (float)delta;
			_despawnTime -= (float)delta;

			if (_despawnTime <= 0)
			{
				EmitSignal(SignalName.PowerUpExpired, this);
				QueueFree();
			}

			// TODO: Consider adding some additional visual feedback.
		}

		private void OnBodyEntered(Node2D body)
		{
			if (body is Paddle)
			{
				EmitSignal(SignalName.PowerUpCollected, this);
				QueueFree();
			}
		}
	}
}