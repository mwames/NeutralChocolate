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
    public class TownScene : IScene
    {
        private TiledMapRenderer renderer;
        private TiledMap map;
        private OrthographicCamera cam;
        private Player player; 
        private List<IEnemy> bullets = new List<IEnemy>();

        private List<IObstacle> obstacles = new List<IObstacle>();

        public DialogBox _dialogBox;
      
        public TownScene(GraphicsDevice graphicsDevice, TiledMap map)
        {
            MediaPlayer.Play(Sound.Overworld);
            this.map = map;
            this.renderer = new TiledMapRenderer(graphicsDevice);
            renderer.LoadMap(map);

            cam = new OrthographicCamera(graphicsDevice);
            player = new Player(bullets);
            _dialogBox = new DialogBox ();
            
            player.Initialize();
            
            //if object ref not found, make sure you are pulling the latest tiled map version. 
           
            var allObstacles = new List<TiledMapObject>(map.GetLayer<TiledMapObjectLayer>("obstacles").Objects);

            obstacles = allObstacles
                .Select(obstacle => (IObstacle)EntityFactory(obstacle))
                .Where(obstacle => obstacle != null)
                .ToList();
        }
        public void Update(GameTime gameTime)
        {
            if(!_dialogBox.Active)
            {
            player.Update(gameTime, player.Position, map.WidthInPixels,map.HeightInPixels);
            UpdateCamera();
            }
            bullets.ForEach(entity => entity.Update(gameTime, player.Position,0,0));
            bullets.RemoveAll(entity => entity.Health <= 0);
            renderer.Update(gameTime);
            
             _dialogBox.Update();

            // Debug key to show opening a new dialog box on demand
            if (Input.kCurrent.IsKeyDown(Keys.O))
            {
                if (!_dialogBox.Active)
                {
               
                    _dialogBox = new DialogBox {Text = "New dialog box! maybe the words will wrap but if it doesn't maybe jam a bunch of text here to watch it loop or maybe it will extend to the next screen."};
                    _dialogBox.Initialize();
                }
            }
            
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont, GraphicsDevice graphicsDevice)
        {
            
            // World space
            spriteBatch.Begin(transformMatrix: cam.GetViewMatrix());
            renderer.Draw(cam.GetViewMatrix());
            if (player.Health > 0)
            {
            player.Draw(spriteBatch);

            // Right side 
            if (player.Position.X <=map.WidthInPixels && player.Position.X >=map.WidthInPixels-60  && player.Position.Y >= 640 && player.Position.Y <=916)
            {
            //spriteBatch.DrawRectangle(0,640,60,256, Color.Blue);
            Store.scenes.ChangeScene(SceneName.Game);
            //spriteBatch.Draw(Art.Tree, new Vector2(200,500),Color.Brown);
            }

            obstacles.ForEach(obstacle => obstacle.Draw(spriteBatch));
            bullets.ForEach(bullet => bullet.Draw(spriteBatch));
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
            return Vector2.Distance(entity1.Position, entity2.Position) < entity1.Radius + entity2.Radius;
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