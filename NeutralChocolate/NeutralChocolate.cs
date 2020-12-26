using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Tiled.Renderers;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Tiled;
using System.Collections.Generic;
using System.Linq;

namespace NeutralChocolate
{
    public class NeutralChocolate : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        public SpriteFont font;
        public DialogBox _dialogBox;
        
        public NeutralChocolate()
        {
            graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Store.scenes= new SceneManager();
            //Store.textures = new TextureManager();
            //Store.soundEffects = new SoundEffectManager();
            //Store.songs = new SongManager();
            Store.modes = new ModeManager();
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();

            font = Content.Load<SpriteFont>("gameFont");
            Winder.Initialize(Window, font);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Art.Load(Content);  
            Sound.Load(Content);
           
            var overworldMap = Content.Load<TiledMap>("Misc/Test2");
            var townMap = Content.Load<TiledMap>("Misc/Town1");
            Store.scenes.Add(SceneName.Game, new GameScene(GraphicsDevice, overworldMap));
            Store.scenes.Add(SceneName.Pause, new PauseScene());
            Store.scenes.Add(SceneName.TitleScene, new TitleScene());
            Store.scenes.Add(SceneName.GameOver, new GameOverScene());
            Store.scenes.ChangeScene(SceneName.TitleScene);

             _dialogBox = new DialogBox
            {
                Text = "Hello Gang! Press Y to proceed.\n" +
                       "I will be on the next pane! " +
                       "And wordwrap will occur, especially if there are some longer words!\n" +
                       "After this dialog box finishes,  press O to open a new one."
            };

            // Initialize the dialog box (this also calls the Show() method)
            _dialogBox.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            Input.Update();
            if (Input.WasPressed(Buttons.Back) || Input.WasPressed(Keys.Escape))
                Exit();
            if (Input.WasPressed(Keys.F1))
                Store.modes.Toggle(Mode.Collider);

            Store.scenes.Scene.Update(gameTime);
            base.Update(gameTime);

             _dialogBox.Update();

            // Debug key to show opening a new dialog box on demand
            if (Input.kCurrent.IsKeyDown(Keys.O))
            {
                if (!_dialogBox.Active)
                {
                    _dialogBox = new DialogBox {Text = "New dialog box! maybe the words will wrap"};
                    _dialogBox.Initialize();
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.ForestGreen);
            Store.scenes.Scene.Draw(spriteBatch, font, GraphicsDevice);
            base.Draw(gameTime);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            // Draw the dialog box to the screen
            _dialogBox.Draw(spriteBatch);

            spriteBatch.End();
        }
    }
}
