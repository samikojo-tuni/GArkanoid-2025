// © 2025 Sami Kojo <sami.kojo@tuni.fi>
// License: 3-Clause BSD License (See the project root folder for details).

namespace GA.GArkanoid.States
{
	public class GameOverState : GameStateBase
	{
		public override StateType StateType => StateType.GameOver;

		public override string ScenePath => "res://Scenes/UI/GameOver.tscn";

		public GameOverState()
		{
			AddTargetState(StateType.MainMenu);
			AddTargetState(StateType.Game);
		}
	}
}