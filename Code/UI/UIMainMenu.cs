// © 2025 Sami Kojo <sami.kojo@tuni.fi>
// License: 3-Clause BSD License (See the project root folder for details).

using System;
using GA.GArkanoid.States;
using GA.GArkanoid.Systems;
using Godot;

namespace GA.GArkanoid.UI
{
	public partial class UIMainMenu : Control
	{
		[Export]
		private Button _newGameButton = null;

		[Export]
		private Button _settingsButton = null;

		[Export]
		private Button _quitButton = null;

		[Export] private Button _loadButton = null;

		public override void _EnterTree()
		{
			_newGameButton.Pressed += OnNewGame;
			_settingsButton.Pressed += OnSettings;
			_quitButton.Pressed += OnQuit;
			_loadButton.Pressed += OnLoadGame;
		}

		public override void _ExitTree()
		{
			_newGameButton.Pressed -= OnNewGame;
			_settingsButton.Pressed -= OnSettings;
			_quitButton.Pressed -= OnQuit;
			_loadButton.Pressed -= OnLoadGame;
		}

		private void OnQuit()
		{
			GameManager.Instance.SceneTree.Quit();
		}

		private void OnSettings()
		{
			GameManager.Instance.ChangeState(StateType.Settings);
		}

		private void OnNewGame()
		{
			GameManager.Instance.Reset();
			GameManager.Instance.ChangeState(StateType.Game);
		}

		private void OnLoadGame()
		{
			if (GameManager.Instance.Load(Config.QuickSaveName))
			{
				GameManager.Instance.ChangeState(StateType.Game);
			}
			else
			{
				GD.PrintErr("Error while loading game");
			}
		}
	}
}