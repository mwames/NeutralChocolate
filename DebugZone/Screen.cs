using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DebugZone
{
    public static class Screen
    {
        public static GameWindow Window = null;
        public static GraphicsDevice GraphicsDevice = null;
        public static SpriteFont Font = null;
        public static int Height => Window.ClientBounds.Height;
        public static int Width => Window.ClientBounds.Width;
    }
}
