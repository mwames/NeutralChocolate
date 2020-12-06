using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NeutralChocolate {
    public interface IEntity {
        Vector2 Position { get; }
        int Radius { get; }
        void Update(GameTime gameTime, Vector2 playerPos);
        void Draw(SpriteBatch spriteBatch);
    }

    public interface IEnemy : IEntity {
        int Health { get; set; }
        void OnHit();
        
    }
    public interface IObstacle : IEntity {
        Vector2 HitPosition { get; }
    }
}
