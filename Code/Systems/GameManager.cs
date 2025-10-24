// © 2025 Sami Kojo <sami.kojo@tuni.fi>
// License: 3-Clause BSD License (See the project root folder for details).

using System;
using System.Collections.Generic;
using GA.Common.Godot;
using GA.GArkanoid.States;
using Godot;

/*
GameManager

- Switch scenes
- Score keeping (DONE)
- Lives (DONE)
- Centralized access point to other managers / systems (TODO)
*/
namespace GA.GArkanoid.Systems
{
	/// <summary>
	/// The singleton which controls game related functionality like loading scenes.
	/// </summary>
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
		private SceneTree _sceneTree = null;

		private List<GameStateBase> _gameStates = new List<GameStateBase>()
		{
			new GameState(),
			new MainMenuState(),
// TODO: Add all the rest as well.
		};

		private Stack<GameStateBase> _loadedStates = new Stack<GameStateBase>();

		public GameStateBase ActiveState =>
			_loadedStates.Count > 0
				? _loadedStates.Peek()
				: null;

		public int Score
		{
			get { return _score; }
			private set
			{
				_score = Mathf.Clamp(value, 0, int.MaxValue);
				EmitSignal(SignalName.ScoreChanged, _score);

				GD.Print($"Score: {Score}");
			}
		}

		public int Lives
		{
			get { return _lives; }
			set
			{
				_lives = Mathf.Clamp(value, 0, Config.MaxLives);
				EmitSignal(SignalName.LivesChanged, _lives);

				GD.Print($"Lives: {Lives}");
			}
		}

		// TODO: Add a signal to indicate the level change
		public int LevelIndex
		{
			get;
			private set;
		} = 1;

		public SceneTree SceneTree
		{
			get
			{
				// Lazy load. Meaning that we load the value to the backing field only
				// when it's needed for the first time.
				if (_sceneTree == null)
				{
					_sceneTree = GetTree();
				}

				return _sceneTree;
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
			LevelIndex = 1;
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

		#region State machine

		public bool ChangeState(StateType stateType)
		{
			if (ActiveState == null)
			{
				GD.PushWarning("No state is currently active. Is this a bug?");
			}

			if (ActiveState != null && !ActiveState.CanTransitionTo(stateType))
			{
				GD.PrintErr($"Invalid state transition from {ActiveState.StateType} to {stateType}");
				return false;
			}

			GameStateBase nextState = GetState(stateType);
			if (nextState == null)
			{
				GD.PrintErr($"The target state of type {stateType} does not exist!");
				return false;
			}

			// TODO: Test this properly

			GameStateBase previousState = ActiveState;
			bool keepPrevious = nextState.IsAdditive;

			if (!keepPrevious && ActiveState != null)
			{
				_loadedStates.Pop();
			}

			// Works great in Godot, but is broken in Unity!
			previousState?.OnExit(); // TODO: Test if keepPrevious needs to be defined.

			// Same as 
			// if (previousState != null)
			// {
			// 	previousState.OnExit();
			// }

			if (ActiveState != nextState)
			{
				if (ActiveState != null && !nextState.IsAdditive)
				{
					_loadedStates.Clear();
				}

				_loadedStates.Push(nextState);
			}

			nextState.OnEnter();

			return true;
		}

		public bool ActivatePreviousState()
		{
			if (_loadedStates.Count < 2)
			{
				GD.PrintErr("No previous state to transition to.");
				return false;
			}

			GameStateBase currentState = _loadedStates.Pop();
			currentState.OnExit();

			ActiveState.OnEnter();

			return true;
		}

		private GameStateBase GetState(StateType stateType)
		{
			foreach (GameStateBase state in _gameStates)
			{
				if (state.StateType == stateType)
				{
					return state;
				}
			}

			return null;
		}

		#endregion State machine
	}
}
