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
        private List<IEntity> entities = new List<IEntity>();
        private List<IEntity> bullets = new List<IEntity>();
        public DialogBox dialogBox = new DialogBox();


        public GameScene(GraphicsDevice graphicsDevice, TiledMap map)
        {
            MediaPlayer.Play(Sound.Overworld);
            
            this.map = map;
            this.renderer = new TiledMapRenderer(graphicsDevice);
            this.cam = new OrthographicCamera(graphicsDevice);
            this.renderer.LoadMap(map);
            var enemies = EntityFactory.ReadMapLayer(map, "Monsters");
            var obstacles = EntityFactory.ReadMapLayer(map, "obstacles");
            this.entities = enemies.Concat(obstacles).ToList();
            this.player = new Player(bullets);
        }
        public void Update(GameTime gameTime)
        {
            if (!dialogBox.Active)
            {
                Camera.Update(cam, player.Position, map);
                player.Update(gameTime, player.Position, map.WidthInPixels, map.HeightInPixels);
                entities.ForEach(entity => entity.Update(gameTime, player.Position, 0, 0));
                bullets.ForEach(entity => entity.Update(gameTime, player.Position, 0, 0));
            }

            renderer.Update(gameTime);

            // Check collisions
            entities.ForEach(entity =>
            {
                if (Collision.DidCollide(player, entity))
                {
                    player.OnHit(entity.Damage, Collision.GetDeflection(player, entity));
                }
            });

            bullets.ForEach(bullet => ResolveBullet(bullet, entities));

            // Remove any necessary entities after collision resolution.
            entities.RemoveAll(entity => entity.Health <= 0);
            bullets.RemoveAll(entity => entity.Health <= 0);
            dialogBox.Update();

            // Debug key to show opening a new dialog box on demand
            if (Input.kCurrent.IsKeyDown(Keys.O))
            {
                if (!dialogBox.Active)
                {
                    dialogBox = new DialogBox { Text = "New dialog box! maybe the words will wrap but if it doesn't maybe jam a bunch of text here to watch it loop or maybe it will extend to the next screen." };
                    dialogBox.Initialize();
                }
            }

            if (Input.WasPressed(Buttons.Start)) {
                Store.scenes.ChangeScene(SceneName.Pause);
            }
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

            entities.ForEach(enemy => enemy.Draw(spriteBatch));
            bullets.ForEach(bullet => bullet.Draw(spriteBatch));
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
            dialogBox.Draw(spriteBatch);

            spriteBatch.End();
        }

        private void ResolveBullet(IEntity bullet, List<IEntity> enemies)
        {
            enemies.ForEach(enemy =>
            {
                if (Collision.DidCollide(bullet, enemy))
                {
                    bullet.OnHit(0, new Vector2(0, 0));
                    enemy.OnHit(0, new Vector2(0, 0));
                }
            });
        }
    }
}
