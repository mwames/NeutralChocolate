using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NeutralChocolate
{
    class Projectile : IEntity
    {
        private readonly int SPEED = 800;
        private Collider collider;
        public Rectangle Bounds => collider.bounds;
        private Vector2 position;
        private Dir direction;
        public Vector2 Position => position;
        public int Damage => 0;

        public int Health { get; set; }

        public Projectile(Vector2 position, Dir direction)
        {
            Health = 1;
            this.position = position;
            this.direction = direction;
            var bounds = new Rectangle((int)position.X, (int)position.Y, Art.Bullet.Width, Art.Bullet.Height);
            this.collider = new Collider(bounds);
        }
        public void Update(GameTime gameTime, Vector2 playerPos, int mapW, int mapH)
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

            collider.Update(gameTime, position, mapW, mapH);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Store.modes.currentMode == Mode.Collider)
            {
                collider.Draw(spriteBatch);
            }
            spriteBatch.Draw(Art.Bullet, position, Color.White);
        }

        public void OnHit(int damage, Vector2 magnitude)
        {
            Health -= 1;
        }
    }
}
