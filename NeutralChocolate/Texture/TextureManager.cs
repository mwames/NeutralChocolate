using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace NeutralChocolate
{
    public enum TextureName
    {
        None,
        Bullet,
        Tree,
        Bush,
        Snake,
        Eye,
        Heart,
        Player,
        PlayerUp,
        PlayerDown,
        PlayerLeft,
        PlayerRight,
        PlayButton,
        OptionsButton,
        Pixel,
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
