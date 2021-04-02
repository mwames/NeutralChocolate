using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;

namespace NeutralChocolate
{
    public class NeutralChocolate : Game
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public SpriteFont font;

        public NeutralChocolate()
        {
            graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Store.scenes = new SceneManager();
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
            var area1 = Content.Load<TiledMap>("Misc/Area1");
            var backarea = Content.Load<TiledMap>("Misc/Backroad");

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
            if (Input.WasPressed(Keys.F2))
                Store.modes.Toggle(Mode.God);

            Store.scenes.Scene.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.ForestGreen);
            Store.scenes.Scene.Draw(this);
            base.Draw(gameTime);
        }
    }
}
