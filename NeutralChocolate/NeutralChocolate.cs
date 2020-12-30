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
        private SpriteFont font;
        
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
            Store.scenes.Add(SceneName.Town, new TownScene(GraphicsDevice, townMap));
            Store.scenes.ChangeScene(SceneName.TitleScene);

             
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

            
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.ForestGreen);
            Store.scenes.Scene.Draw(spriteBatch, font, GraphicsDevice);
            base.Draw(gameTime);
        }
    }
}
