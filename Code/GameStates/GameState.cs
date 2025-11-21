// © 2025 Sami Kojo <sami.kojo@tuni.fi>
// License: 3-Clause BSD License (See the project root folder for details).

using GA.GArkanoid.Systems;

namespace GA.GArkanoid.States
{
	public class GameState : GameStateBase
	{
		public override StateType StateType => StateType.Game;
		public override string ScenePath => "res://Scenes/Level.tscn";

		public GameState()
		{
			AddTargetState(StateType.Pause);
			AddTargetState(StateType.GameOver);
			AddTargetState(StateType.Win);
			AddTargetState(StateType.Game);
		}

		public override void OnEnter(bool forceLoad = false)
		{
			base.OnEnter(forceLoad);

			GameManager.Instance.PlayMusic(StateType);
		}
	}
}