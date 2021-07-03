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
        private Matrix world1 = Matrix.CreateTranslation(new Vector3(0, 0, 0));
        private Matrix world2 = Matrix.CreateTranslation(new Vector3(-10, 0, 0));
        private Matrix world3 = Matrix.CreateTranslation(new Vector3(10, 0, 0));
        private Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.1f, 100f);
        private BasicEffect basicEffect;
        private List<Enemy> enemies = new List<Enemy>();

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
            Screen.Initialize(this.Window);
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
            DrawModel(model, world1, view, this.projection);
            DrawModel(model, world2, view, this.projection);
            DrawModel(model, world3, view, this.projection);

            basicEffect.World = Matrix.CreateScale(0.05f)
                * Matrix.CreateRotationZ(MathHelper.ToRadians(180))
                * Matrix.CreateConstrainedBillboard(Vector3.Zero, camera.position, Vector3.UnitZ, null, null);

            basicEffect.View = view;
            basicEffect.Projection = projection;

            spriteBatch.Begin(0, null, null, DepthStencilState.DepthRead, RasterizerState.CullNone, basicEffect);
            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(spriteBatch, view);
            }
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
