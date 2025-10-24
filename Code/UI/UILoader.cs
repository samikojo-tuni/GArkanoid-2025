// © 2025 Sami Kojo <sami.kojo@tuni.fi>
// License: 3-Clause BSD License (See the project root folder for details).

using System;
using GA.GArkanoid.States;
using GA.GArkanoid.Systems;
using Godot;

namespace GA.GArkanoid.UI
{
	public partial class UILoader : Control
	{
		[Export] private StateType _initialStateType = StateType.MainMenu;

		public override void _Ready()
		{
			GameManager.Instance.ChangeState(_initialStateType);
			QueueFree();
		}
	}
}