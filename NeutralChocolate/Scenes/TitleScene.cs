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
        

        public void Update(GameTime gameTime)
        {
          

            if (Input.WasPressed(Keys.Right) || Input.WasPressed(Buttons.DPadRight))
            {
                options = true;
                start = false;
            }

            if (Input.WasPressed(Keys.Left) || Input.WasPressed(Buttons.DPadLeft))
            {
                options = false;
                start = true;
            }

            if (start == true && (Input.WasPressed(Keys.Enter) || Input.WasPressed(Buttons.Start)))
            {
                options = false;
                Store.scenes.ChangeScene(SceneName.Game);
            }

            if (options == true && (Input.WasPressed(Keys.Enter) || Input.WasPressed(Buttons.Start)))
            {
                start = false;
                Store.scenes.ChangeScene(SceneName.Game); // todo  come up with options and build the screen
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont, GraphicsDevice graphicsDevice)
        {
            graphicsDevice.Clear(Color.White);
            spriteBatch.Begin();
            spriteBatch.Draw(
              Store.textures.Get(TextureName.PlayerDown),
              new Rectangle(0, MARGIN_TOP, Winder.Width, Winder.Height / 2),
                Color.White
             );

            var startColor = start ? Color.Yellow : Color.White;
            var optionsColor = options ? Color.Yellow : Color.White;

            var playButton = Store.textures.Get(TextureName.PlayButton);
            spriteBatch.Draw(
                playButton,
                new Rectangle((Winder.Width / 2) - 96 - 10, (Winder.Height / 2) + MARGIN_TOP * 2, 96, 54),
                startColor
            );

            spriteBatch.Draw(
                Store.textures.Get(TextureName.OptionsButton),
                new Rectangle((Winder.Width / 2) + 10, Winder.Height / 2 + MARGIN_TOP * 2, 110, 54),
                optionsColor
            );

            spriteBatch.DrawString(
                spriteFont,
                MESSAGE,
                new Vector2(
                    Winder.Width / 2 - spriteFont.MeasureString(MESSAGE).X / 2,
                    Winder.Height / 2 - 30 + MARGIN_TOP
                ),
                Color.Black
                );

                spriteBatch.DrawString(
                spriteFont,
                COMPANY,
                new Vector2(
                    Winder.Width / 2 -150,
                    Winder.Height  - 30
                ),
                Color.Black
                );

                spriteBatch.End();
        }
    }
}
