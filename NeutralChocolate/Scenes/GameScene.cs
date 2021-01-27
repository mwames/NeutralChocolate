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

            enemies = EntityFactory.ReadMapLayer(map, "Monsters")
                .Cast<IEnemy>()
                .ToList();

            obstacles = EntityFactory.ReadMapLayer(map, "obstacles");
        }
        public void Update(GameTime gameTime)
        {
            if (!_dialogBox.Active)
            {
                player.Update(gameTime, player.Position, map.WidthInPixels, map.HeightInPixels);
                Camera.Update(cam, player.Position, map);

                // Run the update function for each entity.
                enemies.ForEach(entity => entity.Update(gameTime, player.Position, 0, 0));
                bullets.ForEach(entity => entity.Update(gameTime, player.Position, 0, 0));
            }
            renderer.Update(gameTime);

            // Check collisions
            enemies.Concat(obstacles).ToList().ForEach(entity =>
            {
                if (Collision.DidCollide(player, entity))
                {
                    player.OnCollide(entity.Damage, Collision.GetDeflection(player, entity));
                }
            });

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

        private void ResolveBullet(IEnemy bullet, List<IEnemy> enemies)
        {
            enemies.ForEach(enemy =>
            {
                if (Collision.DidCollide(bullet, enemy))
                {
                    bullet.OnHit();
                    enemy.OnHit();
                }
            });
        }
    }
}
