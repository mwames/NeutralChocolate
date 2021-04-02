using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NeutralChocolate
{
    public class TitleScene : IScene
    {
        readonly string MESSAGE = "The Outdoor adventures of Bathroom Jim";
        readonly string COMPANY = "A KanAme Production";
        readonly int MARGIN_TOP = 75;
        public bool options = false;
        public bool start = true;
        int mAlphaValue = 1;
        int mFadeIncrement = 3;
        double mFadeDelay = .035;

        public void Update(GameTime gameTime)
        {
            mFadeDelay -= gameTime.ElapsedGameTime.TotalSeconds;
            if (mFadeDelay <= 0)
            {
                //Reset the Fade delay
                mFadeDelay = .035;

                //Increment/Decrement the fade value for the image
                mAlphaValue += mFadeIncrement;
            }

            if (Input.WasPressed(Keys.Right) || Input.WasPressed(Buttons.LeftThumbstickRight))
            {
                options = true;
                start = false;
            }

            if (Input.WasPressed(Keys.Left) || Input.WasPressed(Buttons.LeftThumbstickLeft))
            {
                options = false;
                start = true;
            }

            if (start == true && (Input.WasPressed(Keys.Enter) || Input.WasPressed(Buttons.Start) || Input.WasPressed(Buttons.B)))
            {
                options = false;
                Store.scenes.ChangeScene(SceneName.Game);
            }

            if (options == true && (Input.WasPressed(Keys.Enter) || Input.WasPressed(Buttons.Start) || Input.WasPressed(Buttons.B)))
            {
                start = false;
                Store.scenes.ChangeScene(SceneName.Game); // todo  come up with options and build the screen
            }
        }

        public void Draw(NeutralChocolate game)
        {
            game.GraphicsDevice.Clear(Color.LightSkyBlue);
            game.spriteBatch.Begin();
            game.spriteBatch.Draw(
              Art.PlayerDown,
              new Rectangle(0, MARGIN_TOP, Winder.Width, Winder.Height / 2),
                new Color(255, 255, 255, MathHelper.Clamp(mAlphaValue, 0, 255))
             );

            var startColor = start ? Color.Yellow : Color.White;
            var optionsColor = options ? Color.Yellow : Color.White;

            var playButton = Art.PlayButton;
            game.spriteBatch.Draw(
                playButton,
                new Rectangle((Winder.Width / 2) - 96 - 10, (Winder.Height / 2) + MARGIN_TOP * 2, 96, 54),
                startColor
            );

            game.spriteBatch.Draw(
                Art.OptionsButton,
                new Rectangle((Winder.Width / 2) + 10, Winder.Height / 2 + MARGIN_TOP * 2, 110, 54),
                optionsColor
            );

            game.spriteBatch.DrawString(
                game.font,
                MESSAGE,
                new Vector2(
                    Winder.Width / 2 - game.font.MeasureString(MESSAGE).X / 2,
                    Winder.Height / 2 - 30 + MARGIN_TOP
                ),
                Color.Black
                );

            game.spriteBatch.DrawString(
            game.font,
            COMPANY,
            new Vector2(
                Winder.Width / 2 - 150,
                Winder.Height - 30
            ),
            Color.Black
            );

            game.spriteBatch.End();
        }
    }
}
