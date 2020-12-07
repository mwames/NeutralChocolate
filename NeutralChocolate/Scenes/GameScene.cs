using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.Tiled;
using System.Collections.Generic;
using System.Linq;

namespace NeutralChocolate
{
    public class GameScene : IScene
    {
        private TiledMapRenderer renderer;
        private TiledMap map;
        private OrthographicCamera cam;
        private Player player;
        private List<IEnemy> enemies = new List<IEnemy>();
        private List<IObstacle> obstacles = new List<IObstacle>();
        private List<IEnemy> bullets = new List<IEnemy>();
        public GameScene(GraphicsDevice graphicsDevice, TiledMap map)
        {
            this.map = map;
            this.renderer = new TiledMapRenderer(graphicsDevice);
            renderer.LoadMap(map);

            cam = new OrthographicCamera(graphicsDevice);
            player = new Player(bullets);



            player.Initialize();

            //if object ref not found, make sure you are pulling the latest tiled map version. 
            var allEnemies = new List<TiledMapObject>(map.GetLayer<TiledMapObjectLayer>("Monsters").Objects);
            var allObstacles = new List<TiledMapObject>(map.GetLayer<TiledMapObjectLayer>("obstacles").Objects);

            enemies = allEnemies
                .Select(enemy => (IEnemy)EntityFactory(enemy))
                .Where((enemy) => enemy != null)
                .ToList();

            obstacles = allObstacles
                .Select(obstacle => (IObstacle)EntityFactory(obstacle))
                .Where(obstacle => obstacle != null)
                .ToList();
        }
        public void Update(GameTime gameTime)
        {
            UpdateCamera();
            renderer.Update(gameTime);

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
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont, GraphicsDevice graphicsDevice)
        {
            // World space
            spriteBatch.Begin(transformMatrix: cam.GetViewMatrix());
            renderer.Draw(cam.GetViewMatrix());
            if (player.Health >0)
            {
            player.Draw(spriteBatch);
            

            enemies.ForEach(enemy => enemy.Draw(spriteBatch));
            bullets.ForEach(bullet => bullet.Draw(spriteBatch));
            obstacles.ForEach(obstacle => obstacle.Draw(spriteBatch));
            }
            spriteBatch.End();

            // Screen space
            spriteBatch.Begin();
            for (int i = 0; i < player.Health; i++)
            {
                spriteBatch.Draw(Store.textures.Get(TextureName.Heart), new Vector2(i * 63, 0), Color.White);
            }
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
            return Vector2.Distance(entity1.Position, entity2.Position) < entity1.Radius + entity2.Radius;
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

        private void ResolvePlayer(Player player, List<IEnemy> enemies)
        {
            enemies.ForEach(enemy =>
            {
                if (Bonked(player, enemy))
                {
                    player.OnCollide();
                    return;
                }
            });
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
    }
}
