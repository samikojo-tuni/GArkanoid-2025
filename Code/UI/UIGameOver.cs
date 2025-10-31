// © 2025 Sami Kojo <sami.kojo@tuni.fi>
// License: 3-Clause BSD License (See the project root folder for details).
using System;
using GA.GArkanoid.Systems;
using Godot;

namespace GA.GArkanoid.UI
{
	public partial class UIGameOver : Control
	{
		[Export] private Label _scoreLabel = null;
		[Export] private Button _newGameButton = null;
		[Export] private Button _backToMenuButton = null;

		public override void _EnterTree()
		{
			_newGameButton.Pressed += OnNewGamePressed;
			_backToMenuButton.Pressed += OnBackToMenuPressed;
		}

		public override void _ExitTree()
		{
			_newGameButton.Pressed -= OnNewGamePressed;
			_backToMenuButton.Pressed -= OnBackToMenuPressed;
		}

		public override void _Ready()
		{
			_scoreLabel.Text = $"Your score: {GameManager.Instance.Score}";
		}

		private void OnNewGamePressed()
		{
			// TODO: Reset lives and score as well.
			GameManager.Instance.ChangeState(States.StateType.Game);
		}

		private void OnBackToMenuPressed()
		{
			GameManager.Instance.ChangeState(States.StateType.MainMenu);
		}
	}
}
