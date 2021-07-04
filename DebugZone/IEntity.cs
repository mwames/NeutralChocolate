using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DebugZone
{    
    public interface IEntity {
        Vector3 Position { get; set; }
        Texture2D Texture { get; set; }
        void Update();
        void Draw(SpriteBatch spriteBatch);
    }

    public interface IScreenEntity {
        void Update();
        void Draw(SpriteBatch spriteBatch);
    }

    public interface ILivingEntity : IEntity {
        int Health { get; set; }
    }

    public interface IUsableEntity : IEntity {
        void Use();
        void UseOn(IEntity entity);
    }
}
