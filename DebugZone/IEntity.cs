using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DebugZone
{    
    interface IEntity {
        Vector3 Position { get; set; }
        Texture2D Texture { get; set; }
        void Update();
        void Draw(SpriteBatch spriteBatch);
    }

    interface ILivingEntity : IEntity {
        int Health { get; set; }
    }

    interface IUsableEntity : IEntity {
        void Use();
        void UseOn(IEntity entity);
    }
}
