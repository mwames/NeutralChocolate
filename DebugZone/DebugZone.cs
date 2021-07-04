using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DebugZone
{
    public class DebugZone : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont font;
        private Texture2D eye;
        private Texture2D snake;
        private Camera camera;
        private Model model;
        private Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.1f, 100f);
        private BasicEffect basicEffect;
        private List<Enemy> enemies = new List<Enemy>();
        private List<Human> humans = new List<Human>();
        private TextBox textBox;

        public DebugZone()
        {
            graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();
            font = Content.Load<SpriteFont>("gameFont");
            model = Content.Load<Model>("human");
            camera = new Camera(new Vector3(0, -50, 0), new Vector3(0, 50, 0), Vector3.UnitZ);
            Screen.Window = this.Window;
            Screen.GraphicsDevice = this.GraphicsDevice;
            Screen.Font = font;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            eye = Content.Load<Texture2D>("Enemies/eyeEnemy");
            snake = Content.Load<Texture2D>("Enemies/snakeEnemy");
            basicEffect = new BasicEffect(GraphicsDevice)
            {
                TextureEnabled = true,
                VertexColorEnabled = true,
            };

            textBox = new TextBox("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Dolor morbi non arcu risus quis varius quam quisque. Dignissim diam quis enim lobortis scelerisque. Tortor pretium viverra suspendisse potenti nullam. Aliquam malesuada bibendum arcu vitae elementum curabitur vitae nunc sed. Massa ultricies mi quis hendrerit dolor magna eget est lorem. Nulla pharetra diam sit amet nisl. Dapibus ultrices in iaculis nunc sed augue lacus viverra vitae. Bibendum at varius vel pharetra vel turpis nunc eget. Libero justo laoreet sit amet cursus sit amet. Quam lacus suspendisse faucibus interdum posuere lorem ipsum. Egestas dui id ornare arcu. Pulvinar neque laoreet suspendisse interdum consectetur libero id faucibus nisl. Porta lorem mollis aliquam ut porttitor leo a diam sollicitudin. Elementum facilisis leo vel fringilla est. Pellentesque dignissim enim sit amet venenatis urna. Neque gravida in fermentum et sollicitudin. Non curabitur gravida arcu ac tortor dignissim. Dui faucibus in ornare quam viverra orci sagittis eu volutpat. Orci dapibus ultrices in iaculis.", Position.Top);

            humans.Add(new Human(model, Vector3.Zero));
            humans.Add(new Human(model, new Vector3(-10, 0, 0)));
            humans.Add(new Human(model, new Vector3(10, 0, 0)));

            enemies.Add(new Enemy(new Vector3(-200, 100, 0), eye, -1));
            enemies.Add(new Enemy(new Vector3(100, -200, 0), snake, 1));
        }

        protected override void Update(GameTime gameTime)
        {
            var kb = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            camera.Update();
            textBox.Update();

            foreach (Human human in humans)
            {
                human.Update();
            }

            foreach (Enemy enemy in enemies)
            {
                enemy.Update();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.ForestGreen);

            var view = camera.GetView();
            foreach (Human human in humans)
            {
                human.Draw(view, projection);
            }

            basicEffect.World = Matrix.CreateScale(0.05f)
                * Matrix.CreateRotationZ(MathHelper.ToRadians(180))
                * Matrix.CreateConstrainedBillboard(Vector3.Zero, camera.position, Vector3.UnitZ, null, null);
            basicEffect.View = view;
            basicEffect.Projection = projection;

            // spriteBatch.Begin(0, null, null, DepthStencilState.DepthRead, RasterizerState.CullNone, basicEffect);
            // foreach (Enemy enemy in enemies)
            // {
            //     enemy.Draw(spriteBatch, view);
            // }
            // spriteBatch.End();

            spriteBatch.Begin();
            textBox.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawModel(Model model, Matrix world, Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;
                }

                mesh.Draw();
            }
        }
    }
}
