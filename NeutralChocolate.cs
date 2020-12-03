using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NeutralChocolate
{
    public class NeutralChocolate : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont font;

        public NeutralChocolate()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            font = Content.Load<SpriteFont>("gameFont");
            Winder.Initialize(Window, font);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            var gamePadState = GamePad.GetState(PlayerIndex.One);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            var gamePadState = GamePad.GetState(PlayerIndex.One);

            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            PadPrinter.Print(spriteBatch, font);
            
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
