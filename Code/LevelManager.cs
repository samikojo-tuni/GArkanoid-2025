// © 2025 Sami Kojo <sami.kojo@tuni.fi>
// License: 3-Clause BSD License (See the project root folder for details).

using System.Collections;
using System.Collections.Generic;
using GA.Common;
using GA.GArkanoid.Save;
using GA.GArkanoid.Systems;
using Godot;
using Godot.Collections;

namespace GA.GArkanoid
{
	public partial class LevelManager : Node2D, ISave
	{
		private const string LevelContentPath = "res://Scenes/LevelContent";
		private const string LevelContentName = "Level";
		private const string LevelContentExtension = ".tscn";

		private int _blockCount = 0;

		[Export]
		public Ball CurrentBall { get; private set; } = null;

		[Export]
		public Paddle CurrentPaddle { get; private set; } = null;

		// TODO: When is this initialized?
		public static LevelManager Active { get; private set; } = null;

		// TODO: Get reference runtime if null.
		[Export]
		public EffectPlayer EffectPlayer { get; private set; }

		public override void _Ready()
		{
			Active = this;

			LoadLevel(GameManager.Instance.LevelIndex);

			if (EffectPlayer == null)
			{
				EffectPlayer = this.GetNode<EffectPlayer>();
			}

			IList<Block> blocks = this.GetNodesInChildren<Block>();

			// Load the level
			if (GameManager.Instance.LoadedLevelData != null)
			{
				Load(GameManager.Instance.LoadedLevelData);
				GameManager.Instance.LoadedLevelData = null;
			}

			// Initialize Blocks
			foreach (Block block in blocks)
			{
				if (block.IsEnabled)
				{
					_blockCount++;

					block.BlockDestroyed += OnBlockDestroyed;
				}
			}
		}

		public override void _EnterTree()
		{
			GameManager.Instance.LivesChanged += OnLivesChanged;
		}

		public override void _ExitTree()
		{
			GameManager.Instance.LivesChanged -= OnLivesChanged;
		}

		public override void _Process(double delta)
		{
			if (Input.IsActionJustPressed(Config.PauseAction))
			{
				GameManager.Instance.ChangeState(States.StateType.Pause);
			}

			if (Input.IsActionJustPressed(Config.QuickSaveAction))
			{
				GameManager.Instance.Save(Config.QuickSaveName);
			}

			if (Input.IsActionJustPressed("AutoWin"))
			{
				foreach (Block block in this.GetNodesInChildren<Block>())
				{
					block.Hit();
				}
			}
		}

		public static string GetLevelContentPath(int levelIndex)
		{
			return $"{LevelContentPath}/{LevelContentName}{levelIndex}{LevelContentExtension}";
		}

		private bool LoadLevel(int levelIndex)
		{
			string levelContentPath = GetLevelContentPath(levelIndex);
			PackedScene levelContentScene = GD.Load<PackedScene>(levelContentPath);
			if (levelContentScene == null)
			{
				GD.PrintErr($"Cannot load a level at path {levelContentPath}");
				return false;
			}

			Node2D levelContentNode = levelContentScene.Instantiate<Node2D>();
			if (levelContentNode == null)
			{
				GD.PrintErr($"Level scene at the path {levelContentPath} cannot be loaded!");
				return false;
			}

			AddChild(levelContentNode);
			return true;
		}

		private void OnLivesChanged(int lives)
		{
			if (lives > 0)
			{
				CurrentBall.Reset();
			}
			else
			{
				// Game over
				GameManager.Instance.ChangeState(States.StateType.GameOver);
			}
		}

		private void OnBlockDestroyed(Block block)
		{
			block.BlockDestroyed -= OnBlockDestroyed;
			_blockCount--;

			// One one level exists, win when all blocks are destroyed.
			if (_blockCount <= 0)
			{
				GameManager.Instance.LevelIndex++;
				if (GameManager.Instance.LevelIndex > Config.LevelCount)
				{
					GameManager.Instance.ChangeState(States.StateType.Win);
				}
				else
				{
					GameManager.Instance.ChangeState(States.StateType.Game);
				}
			}
		}

		public Dictionary Save()
		{
			Dictionary ballData = CurrentBall.Save();
			Dictionary paddleData = CurrentPaddle.Save();

			Dictionary blockData = new Dictionary();
			foreach (Block block in this.GetNodesInChildren<Block>(recursive: true))
			{
				blockData[block.GUID] = block.IsEnabled;
			}

			Dictionary levelData = new Dictionary();
			levelData["Ball"] = ballData;
			levelData["Paddle"] = paddleData;
			levelData["Blocks"] = blockData;

			return levelData;
		}

		public void Load(Dictionary data)
		{
			Dictionary blockData = (Dictionary)data["Blocks"];
			IList<Block> blocks = this.GetNodesInChildren<Block>();
			// Restore level data.
			foreach (Block block in blocks)
			{
				bool isEnabled = blockData.TryGetValue(block.GUID, out Variant value) && (bool)value;
				block.IsEnabled = isEnabled;
			}

			CurrentBall.Load((Dictionary)data["Ball"]);
			CurrentPaddle.Load((Dictionary)data["Paddle"]);
		}
	}
}