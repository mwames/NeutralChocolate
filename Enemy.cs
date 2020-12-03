using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NeutralChocolate
{
    class Enemy
    {
        private Vector2 position;
        protected int health; 
        protected int speed;
        protected int radius;

        public static List<Enemy> enemies = new List<Enemy>();

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public Vector2 Postion
        {
            get { return position; }
        }
        public int Radius
        {
            get { return radius; }

        }

        public Enemy(Vector2 newPos)
        {
            position = newPos;
        }

        public void Update(GameTime gameTime, Vector2 playerPos)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 moveDir = playerPos - position;
            moveDir.Normalize(); // Direction stays the same but the lenth would go to 1. 

            Vector2 tempPos = position;
            tempPos += moveDir *speed *dt;

            if(!Obstacle.didCollide(tempPos,radius))
            {
                position += moveDir * speed * dt;
            }
        }
    }

    class Snake : Enemy
    {
        public Snake(Vector2 newPos) : base(newPos) 
        {
            speed = 160;
            radius = 42; 
            health = 3;
        }
    }
    class Eye : Enemy
    {
        public Eye(Vector2 newPos) : base(newPos)
        {
            speed = 88;
            radius = 45;
            health = 5;
        }
    }
}
