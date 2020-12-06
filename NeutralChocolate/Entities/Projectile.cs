using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NeutralChocolate
{
    class Projectile : IEnemy
    {

        private readonly int SPEED = 800;
        private int health = 1;
        private Vector2 position;
        private Dir direction;
        public Projectile(Vector2 position, Dir direction)
        {
            this.position = position;
            this.direction = direction;
        }
        public Vector2 Position => position;
        public int Radius => 15;

        public int Health
        {
            get => health;
            set => health = value;
        }

        public void Update(GameTime gameTime, Vector2 playerPos)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            switch (direction)
            {
                case Dir.Right:
                    position.X += SPEED * dt;
                    break;
                case Dir.Left:
                    position.X -= SPEED * dt;
                    break;
                case Dir.Down:
                    position.Y += SPEED * dt;
                    break;
                case Dir.Up:
                    position.Y -= SPEED * dt;
                    break;

                default:
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Store.textures.Get(TextureName.Bullet), position, Color.White);
        }

        public void OnHit()
        {
            Health -= 1;
        }
    }
}
