using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DebugZone
{
    public class Camera
    {

        public Vector3 position;
        public Vector3 lookAt;
        public Vector3 up;
        public Vector3 forward;

        public Point mousePrevious;
        public Point mouseCurrent;
        public float speed = 0.5f;
        float mouseAmount = 0.005f;

        public Camera(Vector3 position, Vector3 forward, Vector3 up)
        {
            this.position = position;

            this.forward = forward;
            forward.Normalize();

            this.lookAt = this.position + this.forward;
            this.up = up;
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
            updatePosition();
            updateForward();
            updateLookAt();
        }

        private void updatePosition()
        {
            var kb = Keyboard.GetState();

            Vector3 direction = copyAndNormalize(forward);
            Vector3 normal = Vector3.Cross(direction, up);

            if (kb.IsKeyDown(Keys.W))
            {
                position += forward * speed;
            }
            if (kb.IsKeyDown(Keys.A))
            {
                position -= normal * speed;
            }
            if (kb.IsKeyDown(Keys.S))
            {
                position -= forward * speed;
            }
            if (kb.IsKeyDown(Keys.D))
            {
                position += normal * speed;
            }
        }

        private void updateForward() {
            // Mouse
            mousePrevious = mouseCurrent;
            mouseCurrent = Mouse.GetState().Position;
            
            var mouseDiff = mouseCurrent - mousePrevious;

            Vector3 direction = copyAndNormalize(forward);
            Vector3 normal = Vector3.Cross(direction, up);

            float y = mouseDiff.Y;
            float x = mouseDiff.X;

            y *= Screen.Height/800.0f;
            x *= Screen.Width/1280.0f;

            forward += x * mouseAmount * normal;

            forward -= y * mouseAmount * up;
            forward.Normalize();

            var pos = new Point(Screen.Width / 2, Screen.Height / 2);
            mousePrevious = pos;
            mouseCurrent = pos;
            Mouse.SetPosition(pos.X, pos.Y);
        }

        private void updateLookAt() {
            lookAt = position + forward;
        }

        private Vector3 copyAndNormalize(Vector3 vector)
        {
            Vector3 copy = new Vector3(vector.X, vector.Y, vector.Z);
            copy.Normalize();
            return copy;
        }

        public double ToDegrees(double radians)
        {
            return radians * (180 / Math.PI);
        }

        public float ToRadians(int degrees)
        {
            return (float)(degrees * (Math.PI / 180));
        }

        public Matrix GetView()
        {
            return Matrix.CreateLookAt(position, lookAt, up);
        }
    }
}
