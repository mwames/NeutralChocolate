using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.Tiled;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;

namespace NeutralChocolate
{
    public class GameScene : IScene
    {
        private TiledMapRenderer renderer;
        private TiledMap map;
        private OrthographicCamera cam;
        private Player player;
        private List<IEnemy> enemies = new List<IEnemy>();
        private List<IEntity> obstacles = new List<IEntity>();
        private List<IEnemy> bullets = new List<IEnemy>();
        public DialogBox _dialogBox;


        public GameScene(GraphicsDevice graphicsDevice, TiledMap map)
        {
            MediaPlayer.Play(Sound.Overworld);
            this.map = map;
            this.renderer = new TiledMapRenderer(graphicsDevice);
            renderer.LoadMap(map);

            cam = new OrthographicCamera(graphicsDevice);
            player = new Player(bullets);
            _dialogBox = new DialogBox();

            player.Initialize();

            //if object ref not found, make sure you are pulling the latest tiled map version. 
            var allEnemies = new List<TiledMapObject>(map.GetLayer<TiledMapObjectLayer>("Monsters").Objects);
            var allObstacles = new List<TiledMapObject>(map.GetLayer<TiledMapObjectLayer>("obstacles").Objects);

            enemies = allEnemies
                .Select(enemy => (IEnemy)EntityFactory(enemy))
                .Where((enemy) => enemy != null)
                .ToList();

            obstacles = allObstacles
                .Select(obstacle => EntityFactory(obstacle))
                .Where(obstacle => obstacle != null)
                .ToList();
        }
        public void Update(GameTime gameTime)
        {
            if (!_dialogBox.Active)
            {
                player.Update(gameTime, player.Position, map.WidthInPixels, map.HeightInPixels);
                UpdateCamera();

                // Run the update function for each entity.
                enemies.ForEach(entity => entity.Update(gameTime, player.Position, 0, 0));
                bullets.ForEach(entity => entity.Update(gameTime, player.Position, 0, 0));
            }
            renderer.Update(gameTime);

            // Check collisions
            ResolvePlayer(player, obstacles.Concat(enemies).ToList());
            bullets.ForEach(bullet => ResolveBullet(bullet, enemies));

            // Remove any necessary entities after collision resolution.
            enemies.RemoveAll(entity => entity.Health <= 0);
            bullets.RemoveAll(entity => entity.Health <= 0);

            _dialogBox.Update();

            // Debug key to show opening a new dialog box on demand
            if (Input.kCurrent.IsKeyDown(Keys.O))
            {
                if (!_dialogBox.Active)
                {

                    _dialogBox = new DialogBox { Text = "New dialog box! maybe the words will wrap but if it doesn't maybe jam a bunch of text here to watch it loop or maybe it will extend to the next screen." };
                    _dialogBox.Initialize();
                }
            }
            if (Input.WasPressed(Buttons.Start))
                 Store.scenes.ChangeScene(SceneName.Pause);


        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont, GraphicsDevice graphicsDevice)
        {

            // World space
            spriteBatch.Begin(transformMatrix: cam.GetViewMatrix());
            renderer.Draw(cam.GetViewMatrix());
           
            if (player.Health > 0)
            {
                player.Draw(spriteBatch);

                // left side 
                if (player.Position.X >= 0 && player.Position.X <= 60 && player.Position.Y >= 640 && player.Position.Y <= 916)
                {
                    Store.scenes.ChangeScene(SceneName.Town);
                }

                // top
                if (player.Position.X >= 1025 && player.Position.X <= 1217 && player.Position.Y >= 0 && player.Position.Y <= 60)
                {
                    spriteBatch.DrawRectangle(1025, 0, 192, 60, Color.Blue);
                }

                enemies.ForEach(enemy => enemy.Draw(spriteBatch));
                bullets.ForEach(bullet => bullet.Draw(spriteBatch));
                obstacles.ForEach(obstacle => obstacle.Draw(spriteBatch));
            }
            else
            {
                Store.scenes.ChangeScene(SceneName.GameOver);
            }
            spriteBatch.End();

            // Screen space
            spriteBatch.Begin();
            for (int i = 0; i < player.Health; i++)
            {
                spriteBatch.Draw(Art.Heart, new Vector2(i * 63, 0), Color.White);
            }
            spriteBatch.End();

            // NOTE: The NonPremultiplied blendstate is used to make the dialog box partially transparent
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            // Draw the dialog box to the screen
            _dialogBox.Draw(spriteBatch);

            spriteBatch.End();
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

        private void UpdateCamera()
        {
            // Camera logic
            float tempX = player.Position.X;
            float tempY = player.Position.Y;
            int camW = Winder.Width;
            int camH = Winder.Height;
            int mapW = map.WidthInPixels;
            int mapH = map.HeightInPixels;

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
            return entity1.Bounds.Intersects(entity2.Bounds);
        }

        private void ResolveBullet(IEnemy bullet, List<IEnemy> enemies)
        {
            enemies.ForEach(enemy =>
            {
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

        private Vector2 GetDeflection(Player player, IEntity entity)
        {
            var deflection = new Vector2();

            // Set the X component
            if (player.Bounds.Center.X > entity.Bounds.Center.X)
            {
                deflection.X = entity.Bounds.Right - player.Bounds.Left;
            }
            else
            {
                deflection.X = entity.Bounds.Left - player.Bounds.Right;
            }

            // Set the Y component
            if (player.Bounds.Center.Y > entity.Bounds.Center.Y) {
                deflection.Y = entity.Bounds.Bottom - player.Bounds.Top;
            }
            else
            {
                deflection.Y = entity.Bounds.Top - player.Bounds.Bottom;
            }

            return deflection;
        }

        private void ResolvePlayer(Player player, List<IEntity> entities)
        {
            entities.ForEach(entity =>
            {
                if (Bonked(player, entity))
                {
                    player.OnCollide(entity.Damage, GetDeflection(player, entity));
                    return;
                }
            });
        }

        private bool DidCollide(IEntity otherEntity)
        {
            foreach (IEntity o in obstacles)
            {
                if (o.Bounds.Intersects(otherEntity.Bounds))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
