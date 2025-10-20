// © 2025 Sami Kojo <sami.kojo@tuni.fi>
// License: 3-Clause BSD License (See the project root folder for details).

using System;
using System.Collections.Generic;
using GA.Common.Godot;
using Godot;

/*
GameManager

- Switch scenes (TODO)
- Score keeping (DONE)
- Lives (DONE)
- Centralized access point to other managers / systems (TODO)
*/
namespace GA.GArkanoid.Systems
{
	public partial class GameManager : Singleton<GameManager>
	{
		[Signal]
		public delegate void LivesChangedEventHandler(int lives);

		[Signal]
		public delegate void ScoreChangedEventHandler(int score);

		[Signal]
		public delegate void GameResetEventHandler();

		private int _score = 0;
		private int _lives = 0;

		public int Score
		{
			get { return _score; }
			private set
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

		protected override void Initialize()
		{
			GD.Print("GameManager initialized!");
		}

		public void Reset()
		{
			Lives = Config.InitialLives;
			Score = Config.InitialScore;
			EmitSignal(SignalName.GameReset);
		}

		public void AddScore(int score)
		{
			if (score < 0)
			{
				GD.PrintErr("Added score can't be negative!");
				return;
			}

			Score += score;
		}

		public void SubtractScore(int score)
		{
			if (score < 0)
			{
				GD.PrintErr("Added score can't be negative!");
				return;
			}

			Score -= score;
		}

		public void IncreaseLives()
		{
			Lives++;
		}

		public void DecreaseLives()
		{
			Lives--;
		}
	}
}
