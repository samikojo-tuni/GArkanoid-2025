using Godot;

namespace GA.GArkanoid.Data
{
	[GlobalClass]
	public partial class PlayerData : Resource
	{
		[Signal] public delegate void LivesChangedEventHandler(int lives);
		[Signal] public delegate void ScoreChangedEventHandler(int score);

		[Export] private int _lives = 0;

		private int _score = 0;

		[Export] public float BallSpeed { get; set; } = 50f;
		[Export] public Vector2 LaunchDirection { get; set; } = new Vector2(1f, 1f);
		[Export] public float PaddleWidth { get; set; } = 1f;
		[Export] public float PaddleSpeed { get; set; } = 50f;

		public int LevelIndex { get; set; } = 1;

		public int Score
		{
			get { return _score; }
			set
			{
				_score = Mathf.Clamp(value, 0, int.MaxValue);
				EmitSignal(SignalName.ScoreChanged, _score);
			}
		}

		public int Lives
		{
			get { return _lives; }
			set
			{
				_lives = Mathf.Clamp(value, 0, Config.MaxLives);
				EmitSignal(SignalName.LivesChanged, _lives);
			}
		}

		public bool IsAlive => Lives > 0;

		public void DeductLife()
		{
			if (Lives > 0)
			{
				Lives--;
			}
		}
	}
}