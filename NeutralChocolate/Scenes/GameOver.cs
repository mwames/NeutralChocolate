using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NeutralChocolate
{
    public class GameOverScene : IScene
    {

        public bool again = true;
        public bool exit = false;
        public void Update(GameTime gameTime)
        {
            //Store.songs.Play(SongName.GameOver);

            if (Input.WasPressed(Keys.Right) || Input.WasPressed(Buttons.DPadRight))
            {
                exit = true;
                again = false;
            }

            if (Input.WasPressed(Keys.Left) || Input.WasPressed(Buttons.DPadLeft))
            {
                exit = false;
                again = true;
            }

            if (again == true && (Input.WasPressed(Keys.Enter) || Input.WasPressed(Buttons.Start)))
            {
                exit = false;
                Store.scenes.ChangeScene(SceneName.TitleScene); // To do Load new Game to restart 
            }

            if (exit == true && (Input.WasPressed(Keys.Enter) || Input.WasPressed(Buttons.Start)))
            {
                again = false;
                Program.Game.Exit();
            }
        }

        public void Draw(NeutralChocolate game)
        {
            game.spriteBatch.Begin();
            var gameOverMessage = "So Sad";
            var stringBox = game.font.MeasureString(gameOverMessage);
            game.GraphicsDevice.Clear(Color.DarkBlue);
            game.spriteBatch.DrawString(
                game.font,
                gameOverMessage,
                new Vector2(
                    Winder.Width / 2 - stringBox.X / 2,
                    Winder.Height / 2 - stringBox.X / 2 - 20
                ),
                Color.DarkGoldenrod
            );

            game.spriteBatch.DrawString(game.font, "Choose your fate", new Vector2(Winder.Width / 2 - 150, Winder.Height - 200), Color.DarkGoldenrod);
            if (again)
            {
                game.spriteBatch.DrawString(game.font, "Try Again", new Vector2(Winder.Width / 2 - 150, Winder.Height - 150), Color.Yellow);
            }
            else
            {
                game.spriteBatch.DrawString(game.font, "Try Again", new Vector2(Winder.Width / 2 - 150, Winder.Height - 150), Color.DarkGoldenrod);
            }

            if (exit)
            {
                game.spriteBatch.DrawString(game.font, "Exit", new Vector2(Winder.Width / 2 + 25, Winder.Height - 150), Color.Yellow);
            }

            else
            {
                game.spriteBatch.DrawString(game.font, "Exit", new Vector2(Winder.Width / 2 + 25, Winder.Height - 150), Color.DarkGoldenrod);
            }

            game.spriteBatch.End();

        }
    }
}
