// © 2025 Sami Kojo <sami.kojo@tuni.fi>
// License: 3-Clause BSD License (See the project root folder for details).
// Inspired by: https://godotshaders.com/shader/9-slice-shader/?post_id=2490&new_post=true
// Original shader is licensed under CC0 license: https://creativecommons.org/publicdomain/zero/1.0/

using Godot;
using System;

namespace GA.GArkanoid
{
	[Tool]
	/// <summary>
	/// A helper class to update the 9-slice sprite shader parameters when the transform changes.
	/// </summary>
	public partial class NineSpliceSprite2D : Sprite2D
	{
		/// <summary>
		/// Updates the node every time transform changes (globally).
		/// </summary>
		public override void _Ready()
		{
			SetNotifyTransform(enable: true);
		}

		/// <summary>
		/// Set shader's scale parameter based on the current transform's global scale value.
		/// Global in this context means that the scale is updated when the transform itself
		/// is updated or any of its parents is updated.
		/// </summary>
		/// <param name="what">Notification type (int)</param>
		public override void _Notification(int what)
		{
			if (what == NotificationTransformChanged)
			{
				Material material = GetMaterial();
				if (material != null)
				{
					material.Set("shader_parameter/scale", GlobalScale);
				}
			}
		}
	}
}