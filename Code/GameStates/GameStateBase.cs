// © 2025 Sami Kojo <sami.kojo@tuni.fi>
// License: 3-Clause BSD License (See the project root folder for details).

using System.Collections.Generic;
using GA.GArkanoid.Systems;
using Godot;

namespace GA.GArkanoid.States
{
	public enum StateType
	{
		None = 0,
		MainMenu,
		LevelSelect,
		Settings,
		Game,
		Pause,
		GameOver,
		Win,
	}

	/// <summary>
	/// The base functionality for game states.
	/// </summary>
	public abstract class GameStateBase
	{
		/// <summary>
		/// The type of a state.
		/// </summary>
		public abstract StateType StateType { get; }

		/// <summary>
		/// The path to the associated scene.
		/// </summary>
		public abstract string ScenePath { get; }

		/// <summary>
		/// Should it be possible to load this state additively, meaning on top
		/// of another state.
		/// </summary>
		public virtual bool IsAdditive => false;

		// The list of valid target states for the current state.
		private List<StateType> _targetStates = new List<StateType>();

		// The scene node reprecenting this state.
		private Node _scene = null;

		// A refence to the assoiciated scene resource on the disk.
		private PackedScene _sceneResource = null;

		protected GameStateBase()
		{
			// Caches the correct scene resource.
			_sceneResource = GD.Load<PackedScene>(ScenePath);
		}

		protected void AddTargetState(StateType type)
		{
			if (!_targetStates.Contains(type))
			{
				_targetStates.Add(type);
			}
		}

		protected bool RemoveTargetState(StateType type)
		{
			return _targetStates.Remove(type);
		}

		public bool CanTransitionTo(StateType targetState)
		{
			return _targetStates.Contains(targetState);
		}

		/// <summary>
		/// State activation logic. This should be run just after the state is activated.
		/// Loads the appropriate scene, if it's not loaded already.
		/// </summary>
		/// <param name="forceLoad"></param>
		public virtual void OnEnter(bool forceLoad = false)
		{
			if (forceLoad || _scene == null)
			{
				// The scene is not loaded or the force load is triggered.
				if (_scene != null)
				{
					// Loading the scene forcefully.
					_scene.QueueFree();
				}

				if (_sceneResource == null)
				{
					GD.PrintErr($"There's no scene in the path {ScenePath}");
					return;
				}

				_scene = _sceneResource.Instantiate();

				// Deferred addition of the scene root since can still be busy, especially
				// when loading the initial state when the game starts.
				GameManager.Instance.SceneTree.
					Root.CallDeferred(Node.MethodName.AddChild, _scene);

				// TODO: Threaded level loading: https://docs.godotengine.org/en/latest/tutorials/io/background_loading.html
			}
		}

		/// <summary>
		/// State deactivation logic. This should be run just before the 
		/// state is inactivated.
		/// </summary>
		/// <param name="keepLoaded"></param>
		public virtual void OnExit(bool keepLoaded = false)
		{
			if (!keepLoaded && _scene != null)
			{
				// The associated scene should be unloaded.
				_scene.QueueFree();
				_scene = null;
			}
		}
	}
}
