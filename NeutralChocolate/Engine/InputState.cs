using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace NeutralChocolate {
    public class InputState {
        public KeyboardState kCurrent;
        public KeyboardState kPrevious;
        public GamePadState gCurrent;
        public GamePadState gPrevious;
        public MouseState mCurrent;
        public MouseState mPrevious;

        public bool Clicked => mCurrent.LeftButton == ButtonState.Pressed && mPrevious.LeftButton != ButtonState.Pressed;
        public bool RightClicked => mCurrent.RightButton == ButtonState.Pressed && mPrevious.RightButton != ButtonState.Pressed;
        public Vector2 ClickedAt => new Vector2(mCurrent.X, mCurrent.Y);

        public InputState(
            KeyboardState kCurrent,
            KeyboardState kPrevious,
            GamePadState gCurrent,
            GamePadState gPrevious,
            MouseState mCurrent,
            MouseState mPrevious
        ) {
            this.kCurrent = kCurrent;
            this.kPrevious = kPrevious;
            this.gCurrent = gCurrent;
            this.gPrevious = gPrevious;
            this.mCurrent = mCurrent;
            this.mPrevious = mPrevious;
        }

        public bool WasPressed(Keys key)
        {
            return kCurrent.IsKeyDown(key) && !kPrevious.IsKeyDown(key);
        }

        public bool WasPressed(Buttons button)
        {
            return gCurrent.IsButtonDown(button) && !gPrevious.IsButtonDown(button);
        }

        public bool IsPressed(Keys key)
        {
            return kCurrent.IsKeyDown(key);
        }

        public bool IsPressed(Buttons button) {
            return gCurrent.IsButtonDown(button);
        }
    }
}
