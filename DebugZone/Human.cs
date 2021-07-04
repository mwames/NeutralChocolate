using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DebugZone
{
    public class Human {
        public Model Model { get; set; }
        public Vector3 Position { get; set; }
        public float Scale { get; set; }
        public float Rotation { get; set; }
        public Texture2D Texture { get; set; }
        public Human(Model model, Vector3 position)
        {
            Rotation = 0;
            Scale = 1;
            
            Model = model;
            Position = position;
        }
        public Human(Model model, Vector3 position, Texture2D texture)
        {
            Rotation = 0;
            Scale = 1;

            Model = model;
            Position = position;
            Texture = texture;
        }

        public void Update()
        {
            // Position += Vector3.UnitZ / 10;
            // Rotation += 0.03f;
        }

        public void Draw(Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World =
                        Matrix.CreateScale(Scale)
                        * Matrix.CreateRotationZ(Rotation)
                        * Matrix.CreateTranslation(Position)
                        * Matrix.CreateRotationZ(Rotation);
                    effect.View = view;
                    effect.Projection = projection;

                    if (Texture != null) {
                        effect.TextureEnabled = true;
                        effect.Texture = Texture;
                    }
                }

                mesh.Draw();
            }

        }
    }
}
