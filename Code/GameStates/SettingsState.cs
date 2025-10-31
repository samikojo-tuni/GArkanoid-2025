namespace GA.GArkanoid.States
{
	public class SettingsState : GameStateBase
	{
		public override StateType StateType => StateType.Settings;
		public override string ScenePath => "res://Scenes/UI/Options.tscn";
		public override bool IsAdditive => true;

		public SettingsState()
		{
			AddTargetState(StateType.MainMenu);
		}
	}
}