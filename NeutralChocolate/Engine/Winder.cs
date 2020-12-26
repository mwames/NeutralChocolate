using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NeutralChocolate
{
    public static class Winder
    {
        private static GameWindow window = null;
        public static SpriteFont spriteFont = null;
        public static GraphicsDevice graphicsDevice;
        public static void Initialize(GameWindow window, SpriteFont spriteFont)
        {
            if (Winder.window != null || Winder.spriteFont != null)
            {
                throw new System.Exception("Winder has already been initialized. Do not mutate shared state.");
            }
            else
            {
                Winder.window = window;
                Winder.spriteFont = spriteFont;
            }
        }

        public static int Height => window.ClientBounds.Height;
        public static int Width => window.ClientBounds.Width;

        public static int HCenter(string message)
        {
            var messageWidth = (int)spriteFont.MeasureString(message).X;
            return (Width / 2) - (messageWidth / 2);
        }

        public static int VCenter(string message)
        {
            var messageHeight = (int)spriteFont.MeasureString(message).Y;
            return (Height / 2) - (messageHeight / 2);
        }
    }
}
