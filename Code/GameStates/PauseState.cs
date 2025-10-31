using GA.GArkanoid.Systems;

namespace GA.GArkanoid.States
{
	public class PauseState : GameStateBase
	{
		public override StateType StateType => StateType.Pause;

		public override string ScenePath => "res://Scenes/UI/Options.tscn";
		public override bool IsAdditive => true;

		public PauseState()
		{
			AddTargetState(StateType.Game);
			AddTargetState(StateType.MainMenu);
		}

		public override void OnEnter(bool forceLoad = false)
		{
			// In this case it's important to call base implelentation. 
			// Othervise the scene loading would break.
			base.OnEnter(forceLoad);

			GameManager.Instance.Pause();
		}

		public override void OnExit(bool keepLoaded = false)
		{
			base.OnExit(keepLoaded);

			// TODO: This should work. If there are issues, move this somewhere else.
			GameManager.Instance.Resume();
		}
	}
}