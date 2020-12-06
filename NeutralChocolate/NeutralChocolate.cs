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
        private TiledMapRenderer mapRenderer;
        private TiledMap myMap;
        private OrthographicCamera cam;
        private Player player;
        private List<IEnemy> enemies = new List<IEnemy>();
        private List<IObstacle> obstacles = new List<IObstacle>();
        private List<IEnemy> bullets = new List<IEnemy>();

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
            graphics.PreferredBackBufferWidth = 1280;  //GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = 720; //GraphicsDevice.DisplayMode.Height;
            //graphics.IsFullScreen = true;
            mapRenderer = new TiledMapRenderer(GraphicsDevice);
            cam = new OrthographicCamera(GraphicsDevice);
            graphics.ApplyChanges();
            Store.scenes.Add(SceneName.Pause, new PauseScene());
            player = new Player(bullets);

            font = Content.Load<SpriteFont>("gameFont");
            Winder.Initialize(Window, font);
            base.Initialize();
        }

        private IEntity EntityFactory(TiledMapObject tmo)
        {
            string type;
            tmo.Properties.TryGetValue("Type", out type);

            switch (type)
            {
                case "Snake":
                    return new Snake(tmo.Position);
                case "Eye":
                    return new Eye(tmo.Position);
                case "Tree":
                    return new Tree(tmo.Position);
                case "Bush":
                    return new Bush(tmo.Position);
                default:
                    return null;
            }
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            player_Sprite = Content.Load<Texture2D>("Player/player");
            myMap = Content.Load<TiledMap>("Misc/Test2");
            mapRenderer.LoadMap(myMap);


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
            Store.soundEffects.Add(SoundEffectName.Blip, Content.Load<SoundEffect>("Sounds/blip"));
            Store.songs.Add(SongName.Overworld, Content.Load<Song>("Sounds/nature")); // should be Sounds/nature
            Store.songs.Play(SongName.Overworld);

            player.Initialize();

            //if object ref not found, make sure you are pulling the latest tiled map version. 
            var allEnemies = new List<TiledMapObject>(myMap.GetLayer<TiledMapObjectLayer>("Monsters").Objects);
            var allObstacles = new List<TiledMapObject>(myMap.GetLayer<TiledMapObjectLayer>("obstacles").Objects);

            enemies = allEnemies
                .Select(enemy => (IEnemy)EntityFactory(enemy))
                .Where((enemy) => enemy != null)
                .ToList();

            obstacles = allObstacles
                .Select(obstacle => (IObstacle)EntityFactory(obstacle))
                .Where(obstacle => obstacle != null)
                .ToList();
        }

        private void DrawUI(int playerHealth)
        {
            for (int i = 0; i < playerHealth; i++)
            {
                spriteBatch.Draw(Store.textures.Get(TextureName.Heart), new Vector2(i * 63, 0), Color.White);
            }
        }

        private bool DidCollide(IEntity otherEntity)
        {
            foreach (IObstacle o in obstacles)
            {
                int sum = o.Radius + otherEntity.Radius;
                if (Vector2.Distance(o.HitPosition, otherEntity.Position) < sum)
                {
                    return true;
                }
            }
            return false;
        }

        private void UpdateCamera() {
            // Camera logic
            float tempX = player.Position.X;
            float tempY = player.Position.Y;
            int camW = graphics.PreferredBackBufferWidth;
            int camH = graphics.PreferredBackBufferHeight;
            int mapW = myMap.WidthInPixels;
            int mapH = myMap.HeightInPixels;

            if (tempX < camW / 2)
            {
                tempX = camW / 2;
            }

            if (tempY < camH / 2)
            {
                tempY = camH / 2;
            }

            if (tempX > (mapW - (camW / 2)))
            {
                tempX = (mapW - (camW / 2));
            }

            if (tempY > (mapH - (camH / 2)))
            {
                tempY = (mapH - (camH / 2));
            }

            cam.LookAt(new Vector2(tempX, tempY));
        }

        private bool Bonked(IEntity entity1, IEntity entity2)
        {
            return Vector2.Distance(entity1.Position, entity2.Position) < entity1.Radius + entity2.Radius;
        }

        private void ResolveBullet(IEnemy bullet, List<IEnemy> enemies)
        {
            enemies.ForEach(enemy => {
                if (Bonked(bullet, enemy))
                {
                    bullet.OnHit();
                    enemy.OnHit();
                }
                if (DidCollide(bullet))
                {
                    bullet.OnHit();
                }
            });
        }

        private void ResolvePlayer(Player player, List<IEnemy> enemies) {
            enemies.ForEach(enemy => {
                if (Bonked(player, enemy))
                {
                    player.OnCollide();
                    return;
                }
            });
        }

        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            UpdateCamera();
            mapRenderer.Update(gameTime);

            // Run the update function for each entity.
            enemies.ForEach(entity => entity.Update(gameTime, player.Position));
            bullets.ForEach(entity => entity.Update(gameTime, player.Position));
            player.Update(gameTime, player.Position);

            // Check collisions
            ResolvePlayer(player, enemies);
            bullets.ForEach(bullet => ResolveBullet(bullet, enemies));

            // Remove any necessary entities after collision resolution.
            enemies.RemoveAll(entity => entity.Health <= 0);
            bullets.RemoveAll(entity => entity.Health <= 0);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.ForestGreen);
            mapRenderer.Draw(cam.GetViewMatrix());
            spriteBatch.Begin(transformMatrix: cam.GetViewMatrix());
            player.Draw(spriteBatch);

            enemies.ForEach(enemy => enemy.Draw(spriteBatch));
            bullets.ForEach(bullet => bullet.Draw(spriteBatch));
            obstacles.ForEach(obstacle => obstacle.Draw(spriteBatch));

            spriteBatch.End();

            // Draw to screen space
            spriteBatch.Begin(transformMatrix: Matrix.Identity);
            PadPrinter.Print(spriteBatch, font);
            DrawUI(player.Health);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
