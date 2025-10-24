// © 2025 Sami Kojo <sami.kojo@tuni.fi>
// License: 3-Clause BSD License (See the project root folder for details).

namespace GA.GArkanoid.States
{
	public class MainMenuState : GameStateBase
	{
		public override StateType StateType => StateType.MainMenu;

		public override string ScenePath => "res://Scenes/UI/MainMenu.tscn";

		public MainMenuState()
		{
			AddTargetState(StateType.Settings);
			AddTargetState(StateType.Game);
		}
	}
}