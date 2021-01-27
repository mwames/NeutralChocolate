using Microsoft.Xna.Framework;

namespace NeutralChocolate {
    public static class Collision {
        public static bool DidCollide(Collider c1, Collider c2) {
            return c1.bounds.Intersects(c2.bounds);
        }
        public static bool DidCollide(IEntity entity1, IEntity entity2)
        {
            return entity1.Bounds.Intersects(entity2.Bounds);
        }

        public static Vector2 GetDeflection(Player player, IEntity entity)
        {
            var deflection = new Vector2();

            // Set the X component
            if (player.Bounds.Center.X > entity.Bounds.Center.X)
            {
                deflection.X = entity.Bounds.Right - player.Bounds.Left;
            }
            else
            {
                deflection.X = entity.Bounds.Left - player.Bounds.Right;
            }

            // Set the Y component
            if (player.Bounds.Center.Y > entity.Bounds.Center.Y) {
                deflection.Y = entity.Bounds.Bottom - player.Bounds.Top;
            }
            else
            {
                deflection.Y = entity.Bounds.Top - player.Bounds.Bottom;
            }

            return deflection;
        }
    }
}
