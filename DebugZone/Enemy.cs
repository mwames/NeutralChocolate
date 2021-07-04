using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DebugZone
{
    public class Enemy
    {
        public Vector3 Position { get; set; }
        public Texture2D Texture { get; set; }
        private Random random = new Random();
        private int direction;

        public Enemy(Vector3 position, Texture2D texture, int direction)
        {
            Position = position;
            Texture = texture;
            this.direction = direction;
        }


        public void Update()
        {
            // Move();
            // Act();
            // var delta = new Vector3(direction, direction, 0);
            // Position += delta;
        }

        public void Draw(SpriteBatch spriteBatch, Matrix view)
        {
            Vector3 pos = Vector3.Transform(Position, view);
            spriteBatch.Draw(Texture, new Vector2(pos.X, pos.Y), Color.White);
        }

        public void Attack(NeutralChocolate.IEntity target) {
            
        }
    }
}
