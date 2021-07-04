using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DebugZone
{
    public enum Position
    {
        Bottom,
        Top
    }
    public class TextBox : IScreenEntity {
        // Constants
        public const int WIDTH = 800;
        public const int HEIGHT = 150;
        public const int BORDER_THICKNESS = 5;
        public const int MAX_ROWS_TEXT = 3;

        // Auto Properties
        public string Message { get; set; }
        public Position Position { get; set; }
        public Texture2D Texture { get; set; }
        public Texture2D Vertical { get; set; }
        public Texture2D Horizontal { get; set; }

        // IDK about all this shit.
        public bool endOfMessage = false;
        public string[] lines { get; set; }
        public int currentLine { get; set; } = 0;
        private KeyboardState previous = Keyboard.GetState();
        private KeyboardState current = Keyboard.GetState();

        // Properties
        private Color color;
        public Color Color
        {
            get { return color; }
            set {
                color = value;
                Texture.SetData(generateColorData(WIDTH, HEIGHT, value));
            }
        }
        

        public TextBox(string message, Position position)
        {
            Position = position;
            Message = message;
            lines = buildLines(message);

            Texture = new Texture2D(Screen.GraphicsDevice, WIDTH, HEIGHT);
            Texture.SetData(generateColorData(WIDTH, HEIGHT, Color.RoyalBlue));

            Vertical = new Texture2D(Screen.GraphicsDevice, BORDER_THICKNESS, HEIGHT + BORDER_THICKNESS);
            Vertical.SetData(generateColorData(BORDER_THICKNESS, HEIGHT + BORDER_THICKNESS, Color.White));

            Horizontal = new Texture2D(Screen.GraphicsDevice, WIDTH + BORDER_THICKNESS, BORDER_THICKNESS);
            Horizontal.SetData(generateColorData(WIDTH + BORDER_THICKNESS, BORDER_THICKNESS, Color.White));
        }

        private string[] buildLines(string message)
        {
            var words = message.Split(" ");
            var lines = wordsToLines(words, WIDTH - 20);
            return lines;
        }

        private string[] wordsToLines(string[] words, int maxWidth)
        {
            // This breaks the text at a space, generating a list of
            // lines which will all fit inside the textbox.
            var currentLine = "";
            var lines = new List<string>();
            foreach (string word in words) {
                if (Screen.Font.MeasureString(currentLine + word).X < maxWidth) {
                    currentLine += $" {word}";
                } else {
                    lines.Add(currentLine);
                    currentLine = word;
                }
            }

            if (Screen.Font.MeasureString(currentLine).X > 0) {
                lines.Add(currentLine);
            }

            return lines.ToArray();
        }

        private Color[] generateColorData(int width, int height, Color color)
        {
            Color[] colorData = new Color[width * height];
            for (int i = 0; i < colorData.Length; i++) colorData[i] = color;
            return colorData;
        }

        public void Update()
        {
            previous = current;
            current = Keyboard.GetState();

            if (previous.IsKeyDown(Keys.Space) && !current.IsKeyDown(Keys.Space)) {
                if (!endOfMessage) {
                    updateDisplayedLines();
                }
                else
                {
                    UI.Delete(this);
                }
            }
        }

        private void updateDisplayedLines() {
            if (currentLine + MAX_ROWS_TEXT <= lines.Length)
            {
                currentLine += MAX_ROWS_TEXT;
            }

            if (currentLine + MAX_ROWS_TEXT > lines.Length)
            {
                endOfMessage = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var position = getPosition();
            var padding = new Vector2(10, 10);
            // Draw Background
            spriteBatch.Draw(Texture, position, Color.White);
            
            // Draw Borders
            //Left
            spriteBatch.Draw(Vertical, position, Color.White);
            // Right
            spriteBatch.Draw(Vertical, new Vector2(position.X + WIDTH, position.Y), Color.White);
            // Top
            spriteBatch.Draw(Horizontal, position, Color.White);
            // Bottom
            spriteBatch.Draw(Horizontal, new Vector2(position.X, position.Y + HEIGHT), Color.White);

            // Draw Message
            var linesToDraw = ArrayHelper.Take<string>(currentLine, MAX_ROWS_TEXT, lines);
            drawLines(spriteBatch, linesToDraw);

            // Draw end icon
            if (endOfMessage) {
                var endMessage = "END";
                var endDimensions = Screen.Font.MeasureString(endMessage);
                var endX = (position.X + WIDTH) - endDimensions.X - 20;
                var endY = (position.Y + WIDTH) - endDimensions.Y - 20;
                spriteBatch.DrawString(Screen.Font, "END", new Vector2(endX, endY), Color.White);
            }
        }

        private void drawLines(SpriteBatch spriteBatch, string[] toDraw) {
            var padding = 20;
            var position = getPosition();
            var lineHeight = Screen.Font.MeasureString(toDraw[0]).Y;
            var x = position.X + padding;

            var i = 0;
            foreach(string line in toDraw)
            {
                var y = position.Y + padding + i * lineHeight;
                spriteBatch.DrawString(Screen.Font, line, new Vector2(x, y), Color.White);
                i += 1;
            }
        }

        private Vector2 getPosition() {
            return new Vector2(getX(), getY());
        }

        private float getX()
        {
            return Screen.Width / 2 - WIDTH / 2;
        }

        private float getY()
        {
            return Position == Position.Top
                ? 10
                : Screen.Height - HEIGHT - 10;
        }
    }
}
