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

        private float offsetX = 0;
        private float offsetY = 0;
        private Camera camera;
        private Model model;
        private Matrix world1 = Matrix.CreateTranslation(new Vector3(0, 0, 0));
        private Matrix world2 = Matrix.CreateTranslation(new Vector3(-10, 0, 0));
        private Matrix world3 = Matrix.CreateTranslation(new Vector3(10, 0, 0));
        private Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.1f, 100f);

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
            camera = new Camera(new Vector3(0, -5, 0), new Vector3(0, 0, 0));
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            eye = Content.Load<Texture2D>("Enemies/eyeEnemy");
        }

        protected override void Update(GameTime gameTime)
        {
            var kb = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            camera.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.ForestGreen);
            DrawModel(model, world1, camera.GetView(), projection);
            DrawModel(model, world2, camera.GetView(), projection);
            DrawModel(model, world3, camera.GetView(), projection);
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
