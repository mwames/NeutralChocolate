using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DebugZone
{
    public class Camera {

        public Vector3 position;
        public Vector3 target;
        public Point mousePrevious;
        public Point mouseCurrent;

        public Camera(Vector3 position, Vector3 target)
        {
            this.position = position;
            this.target = target;
        }

        public Vector3 Transform(Matrix matrix, Vector3 vector)
        {
            var fromVector = new Matrix(
                new Vector4(vector.X, 0, 0, 0),
                new Vector4(vector.Y, 0, 0, 0),
                new Vector4(vector.Z, 0, 0, 0),
                new Vector4(0, 0, 0, 0)
            );

            var result = Matrix.Multiply(matrix, fromVector);

            return new Vector3(result.M11, result.M21, result.M31);
        }

        public void Update()
        {
            var kb = Keyboard.GetState();
            
            AngleBetween(Vector3.UnitY, target + position);

            if (kb.IsKeyDown(Keys.W))
            {
                var delta = new Vector3(0, 1, 0);
                position += delta;
                target += delta;
            }
            if (kb.IsKeyDown(Keys.A))
            {
                var delta = new Vector3(-1, 0, 0);
                position += delta;
                target += delta;
            }
            if (kb.IsKeyDown(Keys.S))
            {
                var delta = new Vector3(0, -1, 0);
                position += delta;
                target += delta;
            }
            if (kb.IsKeyDown(Keys.D))
            {
                var delta = new Vector3(1, 0, 0);
                position += delta;
                target += delta;
            }

            // Mouse
            mousePrevious = mouseCurrent;
            mouseCurrent = Mouse.GetState().Position;
            var mouseDiff = mouseCurrent - mousePrevious;

            if (kb.IsKeyDown(Keys.Left))
            {
                var rotation = Matrix.CreateRotationZ((float)ToRadians(-1));
                var mockTarget = Transform(rotation, target - position);
                target = mockTarget;
            }
            if (kb.IsKeyDown(Keys.Right))
            {
                var rotation = Matrix.CreateRotationZ((float)ToRadians(1));
                var mockTarget = Transform(rotation, target - position);
                target = mockTarget;
            }
            // if (mouseDiff.Y < 0)
            // {
            //     var toTheUp = Vector3.Forward * Math.Abs(mouseDiff.Y);
            //     //System.Console.WriteLine("toTheUp");
            //     target += toTheUp;
            // }
            // if (mouseDiff.Y > 0)
            // {
            //     var toTheDown = Vector3.Backward * Math.Abs(mouseDiff.Y);
            //     //System.Console.WriteLine("toTheDown");
            //     target += toTheDown;
            // }

        }

        public double ToDegrees(double radians) {
            return radians * (180/Math.PI);
        }

        public double ToRadians(double degrees) {
            return degrees * (Math.PI/180);
        }

        public double AngleBetween(Vector3 a, Vector3 b) {
            var dot = Vector3.Dot(a, b);
            var magA = Vector3.Distance(Vector3.Zero, a);
            var magB = Vector3.Distance(Vector3.Zero, b);
            var prod = magA * magB;
            var quot = (double)(dot / prod);

            var angle = Math.Acos(quot);

            return ToDegrees(angle);
        }

        public Matrix GetView() {
            return Matrix.CreateLookAt(position, target, Vector3.UnitZ);
        }
    }
}
