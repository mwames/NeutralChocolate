using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Tiled.Renderers;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Tiled;

namespace NeutralChocolate
{
    enum Dir
    {
        Down,
        Up,
        Left,
        Right
    }
  
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont font;

        Texture2D player_Sprite;
        Texture2D playerDown;
        Texture2D playerUp;
        Texture2D playerLeft;
        Texture2D playerRight;
        TiledMapRenderer mapRenderer;
        TiledMap myMap;

        OrthographicCamera cam;

        Player player;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
           
            Store.textures = new TextureManager();
            Store.soundEffects = new SoundEffectManager();
            Store.songs = new SongManager();
            graphics.PreferredBackBufferWidth = 1280;  //GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = 720; //GraphicsDevice.DisplayMode.Height;
            //graphics.IsFullScreen = true;
            mapRenderer = new TiledMapRenderer(GraphicsDevice);
            cam = new OrthographicCamera(GraphicsDevice);
            graphics.ApplyChanges();
            player = new Player();

            font = Content.Load<SpriteFont>("gameFont");
            Winder.Initialize(Window, font);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            player_Sprite = Content.Load<Texture2D>("Player/player");
            playerDown = Content.Load<Texture2D>("Player/playerDown");
            playerUp = Content.Load<Texture2D>("Player/playerUp");
            playerLeft = Content.Load<Texture2D>("Player/playerLeft");
            playerRight = Content.Load<Texture2D>("Player/playerRight");
            myMap = Content.Load<TiledMap>("Misc/Test2");
            mapRenderer.LoadMap(myMap);


            // these are ordered in the same way as the enum up top to condense animation code on player class
            player.animations[0] = new AnimatedSprite(playerDown, 1, 4);
            player.animations[1] = new AnimatedSprite(playerUp, 1, 4);
            player.animations[2] = new AnimatedSprite(playerLeft, 1, 4);
            player.animations[3] = new AnimatedSprite(playerRight, 1, 4);
            
            Store.textures.Add(TextureName.Bullet, Content.Load<Texture2D>("Misc/bullet"));
            Store.textures.Add(TextureName.Tree, Content.Load<Texture2D>("Obsticales/tree"));
            Store.textures.Add(TextureName.Bush, Content.Load<Texture2D>("Obsticales/bush"));
            Store.textures.Add(TextureName.Eye, Content.Load<Texture2D>("Enemies/eyeEnemy"));
            Store.textures.Add(TextureName.Snake, Content.Load<Texture2D>("Enemies/snakeEnemy"));
            Store.textures.Add(TextureName.Heart, Content.Load<Texture2D>("Misc/heart"));              
            Store.soundEffects.Add(SoundEffectName.Blip, Content.Load<SoundEffect>("Sounds/blip"));
            Store.songs.Add(SongName.Overworld, Content.Load<Song>("Sounds/nature")); // should be Sounds/nature
            Store.songs.Play(SongName.Overworld);
          


            // Enemy.enemies.Add(new Snake(new Vector2(500, 200))); // you can use this to test if your classes and such work. Not normally good practice except for testing.
            // Enemy.enemies.Add(new Eye(new Vector2(450, 400)));
            // Obstacle.obstacles.Add(new Tree(new Vector2(174, 200)));
            // Obstacle.obstacles.Add(new Bush(new Vector2(600, 300)));


            //if object ref not found, make sure you are pulling the latest tiled map version. 
             TiledMapObject[] allEnemies = myMap.GetLayer<TiledMapObjectLayer>("Monsters").Objects; 
             foreach (var en in allEnemies)
             {
                 string type;
                 en.Properties.TryGetValue("Type", out type);

                 if (type == "Snake")
                     Enemy.enemies.Add(new Snake(en.Position));
                 else if (type == "Eye")
                     Enemy.enemies.Add(new Eye(en.Position));
             }

            TiledMapObject[] allObstacles = myMap.GetLayer<TiledMapObjectLayer>("obstacles").Objects;

            foreach (var obj in allObstacles)
            {
                string type;
                obj.Properties.TryGetValue("Type", out type);

                if (type == "Tree")
                    Obstacle.obstacles.Add(new Tree(obj.Position));
                else if (type == "Bush")
                    Obstacle.obstacles.Add(new Bush(obj.Position));
            }
        }

        protected override void Update(GameTime gameTime)
        {
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            mapRenderer.Update(gameTime);

            player.Update(gameTime, myMap.WidthInPixels, myMap.HeightInPixels);
            //*****************************************************************
            //              Camera logic
                        float tempX = player.Position.X;
                        float tempY = player.Position.Y;
                        int camW = graphics.PreferredBackBufferWidth;
                        int camH = graphics.PreferredBackBufferHeight;
                        int mapW = myMap.WidthInPixels;  
                        int mapH = myMap.HeightInPixels;

                        if (tempX <camW /2)
                        {
                            tempX = camW / 2;
                        }    

                        if (tempY < camH /2)
                        {
                            tempY = camH / 2;
                        }

                         if (tempX > (mapW -(camW/2)))
                         {
                             tempX = (mapW - (camW / 2));
                         }

                         if (tempY > (mapH -(camH /2)))
                         {
                             tempY = (mapH - (camH / 2));
                         }

                        cam.LookAt(new Vector2(tempX,tempY)); // add to focus around player, and locks it to map, 
                        //cam.LookAt(player.Position); generic look at map
            //****************************************************************
            foreach (Projectile proj in Projectile.projectiles) // goes through every elemeent in the projectile list and referes to it as proj the current projectile
            {
                proj.Update(gameTime);
            }

            foreach (Enemy en in Enemy.enemies)
            {
                en.Update(gameTime, player.Position);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.ForestGreen);
            mapRenderer.Draw(cam.GetViewMatrix()); //, cam.GetViewMatrix());
            spriteBatch.Begin(transformMatrix: cam.GetViewMatrix()); 
            
            
            if (player.Health > 0)
                player.anim.Draw(spriteBatch, new Vector2(player.Position.X - 48, player.Position.Y - 48)); // for the sprite center minus half the pixel size for X and Y

            foreach (Enemy en in Enemy.enemies)
            {
                Texture2D spriteToDraw;
                int rad;

                if (en.GetType() == typeof(Snake))
                {
                    spriteToDraw = Store.textures.Get(TextureName.Snake);
                    rad = 50;
                }
                else
                {
                    spriteToDraw = Store.textures.Get(TextureName.Eye);
                    rad = 73;
                }

                spriteBatch.Draw(spriteToDraw, new Vector2(en.Postion.X - rad, en.Postion.Y - rad), Color.White);
            }

            foreach (Projectile proj in Projectile.projectiles) //shooting projectiles
            {
                spriteBatch.Draw(Store.textures.Get(TextureName.Bullet), new Vector2(proj.Position.X - proj.Radius, proj.Position.Y - proj.Radius), Color.White); // - radius makes it center of sprite, 
            }

            // Projectile collision
            foreach (Projectile proj in Projectile.projectiles)
            {
                foreach (Enemy en in Enemy.enemies)
                {
                    int sum = proj.Radius + en.Radius;
                    if (Vector2.Distance(proj.Position, en.Postion) < sum)
                    {
                        proj.Collided = true;
                        en.Health--;
                    }
                }
                if (Obstacle.didCollide(proj.Position, proj.Radius))
                    proj.Collided = true;
            }

            foreach (Enemy en in Enemy.enemies) // Creating player damage
            {
                int sum = player.Radius + en.Radius;
                if (Vector2.Distance(player.Position, en.Postion) < sum && player.HealthTimer <= 0)
                {
                    player.Health--;
                    player.HealthTimer = 1.5f;
                }

                if (player.Health <= 0)  // stops music on death
                {
                    MediaPlayer.Stop();
                }
            }

            Projectile.projectiles.RemoveAll(p => p.Collided);
            Enemy.enemies.RemoveAll(e => e.Health <= 0);


            foreach (Obstacle o in Obstacle.obstacles)
            {
                Texture2D spriteToDraw;
                if (o.GetType() == typeof(Tree))
                    spriteToDraw = Store.textures.Get(TextureName.Tree);
                else
                    spriteToDraw = Store.textures.Get(TextureName.Bush);
                spriteBatch.Draw(spriteToDraw, o.Position, Color.White);
            }
            spriteBatch.End();

            spriteBatch.Begin(); // use this section to keep heath or such locked on screen for camera
            PadPrinter.Print(spriteBatch, font);
            for (int i = 0; i < player.Health; i++)
            {
                spriteBatch.Draw(Store.textures.Get(TextureName.Heart), new Vector2(i * 63, 0), Color.White);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
