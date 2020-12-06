using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NeutralChocolate
{
    public class PauseScene : IScene
    {
        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont, GraphicsDevice graphicsDevice)
        {
            string gameOverMessage = "- PAUSE -";
            graphicsDevice.Clear(Color.DarkCyan);
            spriteBatch.DrawString(
                spriteFont,
                gameOverMessage,
                new Vector2(
                    Winder.Width/ 2 - spriteFont.MeasureString(gameOverMessage).X / 2,
                    Winder.Height / 2 - 20
                ),
                Color.Black
                );
        }

        public void Update(InputState input, GameTime gameTime)
        {
            if (input.WasPressed(Buttons.Start) || input.WasPressed(Keys.Enter)) {
                Store.scenes.ChangeScene(SceneName.Game); 
            }
        }
    }
}
