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
    enum Dir
    {
        Down,
        Up,
        Left,
        Right
    }

    public class NeutralChocolate : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont font;
        private Texture2D player_Sprite;


        public NeutralChocolate()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Store.scenes= new SceneManager();
            Store.textures = new TextureManager();
            Store.soundEffects = new SoundEffectManager();
            Store.songs = new SongManager();
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

            player_Sprite = Content.Load<Texture2D>("Player/player");

            // these are ordered in the same way as the enum up top to condense animation code on player class
            Store.textures.Add(TextureName.PlayerUp, Content.Load<Texture2D>("Player/playerUp"));
            Store.textures.Add(TextureName.PlayerDown, Content.Load<Texture2D>("Player/playerDown"));
            Store.textures.Add(TextureName.PlayerLeft, Content.Load<Texture2D>("Player/playerLeft"));
            Store.textures.Add(TextureName.PlayerRight, Content.Load<Texture2D>("Player/playerRight"));
            Store.textures.Add(TextureName.Bullet, Content.Load<Texture2D>("Misc/bullet"));
            Store.textures.Add(TextureName.Tree, Content.Load<Texture2D>("Obsticales/tree"));
            Store.textures.Add(TextureName.Bush, Content.Load<Texture2D>("Obsticales/bush"));
            Store.textures.Add(TextureName.Eye, Content.Load<Texture2D>("Enemies/eyeEnemy"));
            Store.textures.Add(TextureName.Snake, Content.Load<Texture2D>("Enemies/snakeEnemy"));
            Store.textures.Add(TextureName.Heart, Content.Load<Texture2D>("Misc/heart"));
            Store.textures.Add(TextureName.PlayButton, Content.Load<Texture2D>("Misc/PlayButton"));
            Store.textures.Add(TextureName.OptionsButton, Content.Load<Texture2D>("Misc/OptionsButton"));
            Store.soundEffects.Add(SoundEffectName.Blip, Content.Load<SoundEffect>("Sounds/blip"));
            Store.songs.Add(SongName.Overworld, Content.Load<Song>("Sounds/nature")); // should be Sounds/nature
            Store.songs.Play(SongName.Overworld);

            var overworldMap = Content.Load<TiledMap>("Misc/Test2");
            var townMap = Content.Load<TiledMap>("Misc/Town1");
            Store.scenes.Add(SceneName.Game, new GameScene(GraphicsDevice, overworldMap));
            Store.scenes.Add(SceneName.Pause, new PauseScene());
            Store.scenes.Add(SceneName.TitleScene, new TitleScene());
            Store.scenes.ChangeScene(SceneName.TitleScene);
        }

        protected override void Update(GameTime gameTime)
        {
            Input.Update();
            if (Input.WasPressed(Buttons.Back) || Input.WasPressed(Keys.Escape))
                Exit();

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
