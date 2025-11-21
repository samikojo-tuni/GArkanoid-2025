using System;
using System.Collections.Generic;
using GA.Common;
using Godot;

namespace GA.GArkanoid.Editor
{
	[Tool]
	public partial class ResetGUIDs : Node2D
	{
		public override void _Ready()
		{
			if (Engine.IsEditorHint())
			{
				EnsureUniqueGUIDs();
			}
		}

		private void EnsureUniqueGUIDs()
		{
			HashSet<string> guids = new HashSet<string>();
			IList<Block> blocks = this.GetNodesInChildren<Block>(recursive: true);

			foreach (Block block in blocks)
			{
				if (block == null)
				{
					// Bug, this should not happen
					continue;
				}

				string guid = block.GUID;
				if (string.IsNullOrWhiteSpace(guid) || guids.Contains(guid))
				{
					// GUID either does not exist or it is a duplicate. Create a new one.
					string newGUID;
					do
					{
						newGUID = Guid.NewGuid().ToString();
					} while (guids.Contains(newGUID));

					guid = newGUID;
					block.SetGUID(guid);
				}

				guids.Add(guid);
			}
		}
	}
}