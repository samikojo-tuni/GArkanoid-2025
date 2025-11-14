// © 2025 Sami Kojo <sami.kojo@tuni.fi>
// License: 3-Clause BSD License (See the project root folder for details).

using GA.GArkanoid.Systems;

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

		public override void OnEnter(bool forceLoad = false)
		{
			base.OnEnter(forceLoad);

			GameManager.Instance.PlayMusic(StateType);
		}
	}
}