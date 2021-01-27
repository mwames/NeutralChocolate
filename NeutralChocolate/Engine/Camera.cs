using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;

namespace NeutralChocolate {
    public static class Camera {

        public static void Update(OrthographicCamera camera, Vector2 position, TiledMap map)
        {
            // Camera logic
            float tempX = position.X;
            float tempY = position.Y;
            int camW = Winder.Width;
            int camH = Winder.Height;
            int mapW = map.WidthInPixels;
            int mapH = map.HeightInPixels;

            if (tempX < camW / 2)
            {
                tempX = camW / 2;
            }

            if (tempY < camH / 2)
            {
                tempY = camH / 2;
            }

            if (tempX > (mapW - (camW / 2)))
            {
                tempX = (mapW - (camW / 2));
            }

            if (tempY > (mapH - (camH / 2)))
            {
                tempY = (mapH - (camH / 2));
            }

            camera.LookAt(new Vector2(tempX, tempY));
        }
    }
}
