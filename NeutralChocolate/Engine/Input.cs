using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace NeutralChocolate
{
    public static class Input
    {
        public static KeyboardState kCurrent;
        public static KeyboardState kPrevious;
        public static GamePadState gCurrent;
        public static GamePadState gPrevious;
        public static MouseState mCurrent;
        public static MouseState mPrevious;

        public static bool Clicked => mCurrent.LeftButton == ButtonState.Pressed && mPrevious.LeftButton != ButtonState.Pressed;
        public static bool RightClicked => mCurrent.RightButton == ButtonState.Pressed && mPrevious.RightButton != ButtonState.Pressed;
        public static Vector2 ClickedAt => new Vector2(mCurrent.X, mCurrent.Y);

        public static void Update()
        {
            kPrevious = kCurrent;
            kCurrent = Keyboard.GetState();
            gPrevious = gCurrent;
            gCurrent = GamePad.GetState(PlayerIndex.One);
            mPrevious = mCurrent;
            mCurrent = Mouse.GetState();
        }

        public static bool WasPressed(Keys key)
        {
            return kCurrent.IsKeyDown(key) && !kPrevious.IsKeyDown(key);
        }

        public static bool WasPressed(Buttons button)
        {
            return gCurrent.IsButtonDown(button) && !gPrevious.IsButtonDown(button);
        }

        public static bool IsPressed(Keys key)
        {
            return kCurrent.IsKeyDown(key);
        }

        public static bool IsPressed(Buttons button)
        {
            return gCurrent.IsButtonDown(button);
        }
    }
}
