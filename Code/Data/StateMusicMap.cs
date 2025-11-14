using GA.GArkanoid.States;
using Godot;

namespace GA.GArkanoid.Data
{
	[GlobalClass, Tool]
	public partial class StateMusicMap : Resource
	{
		[Export] public StateType StateType { get; set; }
		[Export] public AudioStream MusicTrack { get; set; }
	}
}