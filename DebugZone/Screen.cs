using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DebugZone
{
    public static class Screen
    {
        private static GameWindow window = null;
        public static int Height => window.ClientBounds.Height;
        public static int Width => window.ClientBounds.Width;

        public static void Initialize(GameWindow window)
        {
            if (Screen.window != null)
            {
                throw new System.Exception("Winder has already been initialized. Do not mutate shared state.");
            }
            else
            {
                Screen.window = window;
            }
        }
    }
}
