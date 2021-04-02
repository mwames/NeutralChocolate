using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NeutralChocolate
{
    public class PauseScene : IScene
    {
        public void Update(GameTime gameTime)
        {
            if (Input.WasPressed(Keys.Enter)|| Input.WasPressed(Buttons.Start))
            {
                Store.scenes.ChangeScene(SceneName.Game);
            }
        }
        public void Draw(NeutralChocolate game)
        {
            game.spriteBatch.Begin();
            string gameOverMessage = "- PAUSE -";
            game.GraphicsDevice.Clear(Color.DarkCyan);
            game.spriteBatch.DrawString(
                game.font,
                gameOverMessage,
                new Vector2(
                    Winder.Width / 2 - game.font.MeasureString(gameOverMessage).X / 2,
                    Winder.Height / 2 - 20
                ),
                Color.Black
                );
            game.spriteBatch.End();
        }
    }
}
