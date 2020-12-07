using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NeutralChocolate {
    public static class PadPrinter {
        private static readonly int MARGIN = 10;
        private static readonly int HEIGHT = 35;
        private static readonly int RIGHT_OFFSET = 20;
        private static string ButtonName(Buttons button) {
            return button.ToString().Replace("LeftThumbstick", "");
        }
        private static bool IsDown(Buttons button) {
            return GamePad.GetState(PlayerIndex.One).IsButtonDown(button);
        }
        public static void Print(SpriteBatch batch, SpriteFont font) {
            // Top row
            var up = Buttons.LeftThumbstickUp;
            batch.DrawString(font, ButtonName(up), new Vector2(MARGIN * 6, 1 * HEIGHT + MARGIN), IsDown(up) ? Color.Green : Color.Red);

            var y = Buttons.Y;
            batch.DrawString(font, ButtonName(y), new Vector2(MARGIN * 25, 1 * HEIGHT + MARGIN), IsDown(y) ? Color.Green : Color.Red);

            // Second row
            var left = Buttons.LeftThumbstickLeft;
            batch.DrawString(font, ButtonName(left), new Vector2(MARGIN, 2 * HEIGHT + MARGIN), IsDown(left) ? Color.Green : Color.Red);

            var right = Buttons.LeftThumbstickRight;
            batch.DrawString(font, ButtonName(right), new Vector2(MARGIN * 9, 2 * HEIGHT + MARGIN), IsDown(right) ? Color.Green : Color.Red);

            var x = Buttons.X;
            batch.DrawString(font, ButtonName(x), new Vector2(MARGIN * 21, 2 * HEIGHT + MARGIN), IsDown(x) ? Color.Green : Color.Red);

            var b = Buttons.B;
            batch.DrawString(font, ButtonName(b), new Vector2(MARGIN * 29, 2 * HEIGHT + MARGIN), IsDown(b) ? Color.Green : Color.Red);

            // Bottom Row
            var down = Buttons.LeftThumbstickDown;
            batch.DrawString(font, ButtonName(down), new Vector2(MARGIN * 4, 3 * HEIGHT + MARGIN), IsDown(down) ? Color.Green : Color.Red);

            var a = Buttons.A;
            batch.DrawString(font, ButtonName(a), new Vector2(MARGIN * 25, 3 * HEIGHT + MARGIN), IsDown(a) ? Color.Green : Color.Red);
        }

        public static void PrintRight(SpriteBatch batch, SpriteFont font) {
            // Top row
            var up = Buttons.LeftThumbstickUp;
            batch.DrawString(font, ButtonName(up), new Vector2(Winder.Width - RIGHT_OFFSET - MARGIN * 24, 1 * HEIGHT + MARGIN), IsDown(up) ? Color.Green : Color.Red);

            var y = Buttons.Y;
            batch.DrawString(font, ButtonName(y), new Vector2(Winder.Width - RIGHT_OFFSET - MARGIN * 6, 1 * HEIGHT + MARGIN), IsDown(y) ? Color.Green : Color.Red);

            // Second row
            var left = Buttons.LeftThumbstickLeft;
            batch.DrawString(font, ButtonName(left), new Vector2(Winder.Width - RIGHT_OFFSET - MARGIN * 29, 2 * HEIGHT + MARGIN), IsDown(left) ? Color.Green : Color.Red);

            var right = Buttons.LeftThumbstickRight;
            batch.DrawString(font, ButtonName(right), new Vector2(Winder.Width - RIGHT_OFFSET - MARGIN * 21, 2 * HEIGHT + MARGIN), IsDown(right) ? Color.Green : Color.Red);

            var x = Buttons.X;
            batch.DrawString(font, ButtonName(x), new Vector2(Winder.Width - RIGHT_OFFSET - MARGIN * 9, 2 * HEIGHT + MARGIN), IsDown(x) ? Color.Green : Color.Red);

            var b = Buttons.B;
            batch.DrawString(font, ButtonName(b), new Vector2(Winder.Width - RIGHT_OFFSET - MARGIN * 2, 2 * HEIGHT + MARGIN), IsDown(b) ? Color.Green : Color.Red);

            // Bottom Row
            var down = Buttons.LeftThumbstickDown;
            batch.DrawString(font, ButtonName(down), new Vector2(Winder.Width - RIGHT_OFFSET - MARGIN * 26, 3 * HEIGHT + MARGIN), IsDown(down) ? Color.Green : Color.Red);

            var a = Buttons.A;
            batch.DrawString(font, ButtonName(a), new Vector2(Winder.Width - RIGHT_OFFSET - MARGIN * 5, 3 * HEIGHT + MARGIN), IsDown(a) ? Color.Green : Color.Red);
        }
    }
}
