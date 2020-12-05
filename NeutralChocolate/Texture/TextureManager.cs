using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace NeutralChocolate
{
    public enum TextureName
    {
        None,
      
    }

    public class TextureManager
    {
        public Dictionary<TextureName, Texture2D> textures = new Dictionary<TextureName, Texture2D>();

        public void Add(TextureName name, Texture2D texture)
        {
            textures.Add(name, texture);
        }

        public Texture2D Get(TextureName name)
        {
            return textures[name];
        }
    }
}
