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
        private Camera camera;
        private Model model;
        private Matrix world1 = Matrix.CreateTranslation(new Vector3(0, 0, 0));
        private Matrix world2 = Matrix.CreateTranslation(new Vector3(-10, 0, 0));
        private Matrix world3 = Matrix.CreateTranslation(new Vector3(10, 0, 0));
        private Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.1f, 100f);
        private BasicEffect basicEffect;

        private int x = 270;
        private int y = 0;
        private int z = 0;


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
            basicEffect = new BasicEffect(GraphicsDevice)
            {
                TextureEnabled = true,
                VertexColorEnabled = true,
            };
        }

        protected override void Update(GameTime gameTime)
        {
            var kb = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (kb.IsKeyDown(Keys.X)) {
                x = (x + 1) % 360;
            }
            if (kb.IsKeyDown(Keys.Y)) {
                y = (y + 1) % 360;
            }
            if (kb.IsKeyDown(Keys.Z)) {
                z = (z + 1) % 360;
            }

            camera.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.ForestGreen);

            var view = camera.GetView();
            DrawModel(model, world1, view, this.projection);
            DrawModel(model, world2, view, this.projection);
            DrawModel(model, world3, view, this.projection);

            basicEffect.World = Matrix.CreateScale(0.1f)
                * Matrix.CreateRotationX(MathHelper.ToRadians(x))
                * Matrix.CreateRotationY(MathHelper.ToRadians(y))
                * Matrix.CreateRotationZ(MathHelper.ToRadians(z));

            basicEffect.View = view;
            basicEffect.Projection = projection;

            spriteBatch.Begin(0, null, null, DepthStencilState.DepthRead, RasterizerState.CullNone, basicEffect);
            spriteBatch.Draw(eye, Vector2.Zero, Color.White);
            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.DrawString(font, $"X: {x}", new Vector2(10, 10), Color.White);
            spriteBatch.DrawString(font, $"Y: {y}", new Vector2(10, 60), Color.White);
            spriteBatch.DrawString(font, $"Z: {z}", new Vector2(10, 110), Color.White);
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
