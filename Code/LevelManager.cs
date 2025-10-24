using System;
using GA.GArkanoid.Systems;
using Godot;

namespace GA.GArkanoid
{
	public partial class LevelManager : Node2D
	{
		private const string LevelContentPath = "res://Scenes/LevelContent";
		private const string LevelContentName = "Level";
		private const string LevelContentExtension = ".tscn";

		[Export]
		public Ball CurrentBall { get; private set; } = null;

		[Export]
		public Paddle CurrentPaddle { get; private set; } = null;

		// TODO: When is this initialized?
		public static LevelManager Active { get; private set; } = null;

		public override void _Ready()
		{
			Active = this;
			// TODO: Will this work when loading a new level?
			GameManager.Instance.Reset();
			LoadLevel(GameManager.Instance.LevelIndex);
			GameManager.Instance.LivesChanged += OnLivesChanged;
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
				// TODO: Don't actually destroy the ball!
				CurrentBall.QueueFree();
				CurrentBall = null;
				GD.Print("Game Over!");
			}
		}
	}
}