using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NeutralChocolate
{
    static class Art
    {
        public static Texture2D PlayerUp { get; private set; }
        public static Texture2D PlayerDown { get; private set; }
        public static Texture2D PlayerLeft { get; private set; }
        public static Texture2D PlayerRight { get; private set; }
        public static Texture2D Bush { get; private set; }
        public static Texture2D Tree { get; private set; }
		public static Texture2D Pixel { get; private set; }
        public static Texture2D Snake { get; private set; }
        public static Texture2D Eye { get; private set; }
        public static Texture2D Bullet { get; private set; }
        public static Texture2D Heart { get; private set; }
        public static Texture2D PlayButton { get; private set; }
        public static Texture2D OptionsButton { get; private set; }

        public static void Load(ContentManager Content)
        {
            PlayerUp = Content.Load<Texture2D>("Player/playerUp");
            PlayerDown = Content.Load<Texture2D>("Player/playerDown");
            PlayerLeft = Content.Load<Texture2D>("Player/playerLeft");
            PlayerRight = Content.Load<Texture2D>("Player/playerRight");
            Bullet = Content.Load<Texture2D>("Misc/bullet");
            Tree = Content.Load<Texture2D>("Obsticales/tree");
            Bush = Content.Load<Texture2D>("Obsticales/bush");
            Eye = Content.Load<Texture2D>("Enemies/eyeEnemy");
            Snake = Content.Load<Texture2D>("Enemies/snakeEnemy");
            Heart = Content.Load<Texture2D>("Misc/heart");
            PlayButton = Content.Load<Texture2D>("Misc/PlayButton");
            OptionsButton = Content.Load<Texture2D>("Misc/OptionsButton");
            Pixel = Content.Load<Texture2D>("pixel");

        }
    }
}
