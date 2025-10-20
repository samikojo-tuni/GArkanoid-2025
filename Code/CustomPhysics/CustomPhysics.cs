// © 2025 Sami Kojo <sami.kojo@tuni.fi>
// License: 3-Clause BSD License (See the project root folder for details).

using GA.Common;
using Godot;

namespace GA.GArkanoid.Physics
{
	public static class CustomPhysics
	{
		public class Hit
		{
			// It is possible to create non-static classes inside a static class.
			// The Hit class has to be public, because we will use it from outide of
			// CustomPhysics.

			// The exact point where the collision occurred in world coordinates

			public Vector2 Point;

			// The surface normal (perpendicular direction) at the collision point
			// This vector points "outward" from the surface that was hit
			// Used to calculate how objects should bounce or reflect
			public Vector2 Normal;

			// How far the colliding object penetrated into the surface
			// This vector shows the direction and distance to move the object
			// to separate it from the collision (resolve overlap)
			public Vector2 Delta;
		}

		/// <summary>
		/// This function checks if a single point is inside a rectangle.
		/// Used for simple collision detection (like checking if a bullet hit a target)
		/// </summary>
		/// <param name="rectangle">The rectangle to check collision against</param>
		/// <param name="point">The point to test for collision</param>
		/// <returns>Hit object with collision info, or null if no collision</returns>
		public static Hit Intersects(Rect2 rectangle, Vector2 point)
		{
			// STEP 1: Get rectangle's center and half-dimensions
			Vector2 center = rectangle.GetCenter();
			Vector2 extents = rectangle.GetExtents();

			// STEP 2: Calculate vector from rectangle center to the test point
			Vector2 delta = point - center;

			// STEP 3: Calculate how far the point penetrates into the rectangle
			// If point is outside, penetration will be negative
			float penetrationX = extents.X - Mathf.Abs(delta.X);
			float penetrationY = extents.Y - Mathf.Abs(delta.Y);

			// STEP 4: Check if point is actually inside the rectangle
			if (penetrationX < 0 || penetrationY < 0)
			{
				// No collision
				return null;
			}

			// STEP 5: Determine which edge of the rectangle is closest to the point
			// This tells us the collision normal (which direction to bounce)
			Vector2 normal;
			Vector2 penetrationVector;
			Vector2 collisionPoint;

			if (penetrationX < penetrationY)
			{
				// Point is closer to left/right edge
				float signX = Mathf.Sign(delta.X);
				normal = new Vector2(signX, 0);
				penetrationVector = new Vector2(penetrationX * signX, 0);
				collisionPoint = new Vector2(center.X + (extents.X * signX), point.Y);
			}
			else
			{
				// Point is closer to top/bottom edge
				float singY = Mathf.Sign(delta.Y);
				normal = new Vector2(0, singY);
				penetrationVector = new Vector2(0, penetrationY * singY);
				collisionPoint = new Vector2(point.X, center.Y + (extents.Y * singY));
			}

			// STEP 6: Create and return collision information
			return new Hit()
			{
				Point = collisionPoint,
				Normal = normal,
				Delta = penetrationVector
			};
		}

		/// <summary>
		/// This function checks if a circle collides with a rectangle
		/// Used for ball vs block collision in games like Arkanoid/Breakout
		/// </summary>
		/// <param name="rectangle">The rectangle to check collision against</param>
		/// <param name="point">The center of the circle</param>
		/// <param name="radius">The radius of the circle</param>
		/// <returns>Hit object with collision info, or null if no collision</returns>
		public static Hit Intersects(Rect2 rectangle, Vector2 point, float radius)
		{
			// STEP 1: Find the closest point on the rectangle to the circle center
			// This handles both cases: circle hitting edge/corner of rectangle
			float closestX = Mathf.Clamp(point.X, rectangle.Position.X,
				rectangle.Position.X + rectangle.Size.X);
			float closestY = Mathf.Clamp(point.Y, rectangle.Position.Y,
				rectangle.Position.Y + rectangle.Size.Y);
			Vector2 closestPoint = new Vector2(closestX, closestY);

			// STEP 2: Calculate distance from circle center to closest point
			Vector2 delta = point - closestPoint;

			// When comparing lengths, it's usually enough to compare squared lenghts instead of actual lengths.
			// This eliminates expensive square root calculation.
			float distanceSquared = delta.LengthSquared();

			// STEP 3: Check if circle is close enough to touch the rectangle
			if (distanceSquared > radius * radius)
			{
				// No collision
				return null;
			}

			// STEP 4: Calculate collision normal (direction circle should bounce)
			// Normal points from the collision point toward the circle center
			Vector2 normal = delta.Normalized();

			// STEP 5: Calculate how far the circle penetrated into the rectangle
			float penetrationDepth = radius - Mathf.Sqrt(distanceSquared);
			Vector2 penetrationVector = normal * penetrationDepth;

			// STEP 6: Create and return collision information
			return new Hit()
			{
				Point = closestPoint,
				Normal = normal,
				Delta = penetrationVector
			};
		}

		/// <summary>
		/// This function calculates how a moving object should bounce off a surface.
		/// </summary>
		/// <param name="direction">The current movement direction of the object (before collision)</param>
		/// <param name="normal">The surface normal at the collision point</param>
		/// <returns>New direction vector after bouncing</returns>
		public static Vector2 Bounce(Vector2 direction, Vector2 normal)
		{
			Vector2 dx = direction.Dot(normal) * normal;
			Vector2 dy = direction - dx;
			return dy - dx;
		}
	}
}