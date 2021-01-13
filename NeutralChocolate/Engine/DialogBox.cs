using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace NeutralChocolate
{
    public class DialogBox
    {
        public string Text { get; set; }
        public bool Active { get; private set; }
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public Color FillColor { get; set; }
        public Color BorderColor { get; set; }
        public Color DialogColor { get; set; }
        public int BorderWidth { get; set; }

        private readonly Texture2D _fillTexture;
        private readonly Texture2D _borderTexture;
        private List<string> _pages;
        private const float DialogBoxMargin = 14f;
        private Vector2 _characterSize = Winder.spriteFont.MeasureString(new StringBuilder("s", 1));
        private int MaxCharsPerLine => (int)Math.Floor((Size.X - DialogBoxMargin) / _characterSize.X);
        private int MaxLines => (int)Math.Floor((Size.Y - DialogBoxMargin) / _characterSize.Y) - 1;
        private int _currentPage;
        private int _interval;
        private Rectangle TextRectangle => new Rectangle(Position.ToPoint(), Size.ToPoint());
        private List<Rectangle> BorderRectangles => new List<Rectangle>
        {
            // Top
            new Rectangle(
                TextRectangle.X - BorderWidth,
                TextRectangle.Y - BorderWidth,
                TextRectangle.Width + BorderWidth * 2,
                BorderWidth
            ),

            // Right
            new Rectangle(
                TextRectangle.X + TextRectangle.Size.X,
                TextRectangle.Y, BorderWidth,
                TextRectangle.Height
            ),

            // Bottom
            new Rectangle(
                TextRectangle.X - BorderWidth,
                TextRectangle.Y + TextRectangle.Size.Y,
                TextRectangle.Width + BorderWidth * 2,
                BorderWidth
            ),

            // Left
            new Rectangle(
                TextRectangle.X - BorderWidth,
                TextRectangle.Y,
                BorderWidth,
                TextRectangle.Height
            )
        };

        private Vector2 TextPosition => new Vector2(Position.X + DialogBoxMargin / 2, Position.Y + DialogBoxMargin / 2);
        private Stopwatch _stopwatch;

        public DialogBox()
        {
            BorderWidth = 4;
            DialogColor = Color.Black;

            FillColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            BorderColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);

            _fillTexture = new Texture2D(Program.Game.GraphicsDevice, 1, 1);
            _fillTexture.SetData(new[] { FillColor });

            _borderTexture = new Texture2D(Program.Game.GraphicsDevice, 1, 1);
            _borderTexture.SetData(new[] { BorderColor });

            _pages = new List<string>();
            _currentPage = 0;

            var sizeX = (int)(Winder.Width * 0.5);
            var sizeY = (int)(Winder.Height * 0.2);

            Size = new Vector2(sizeX, sizeY);

            var posX = Winder.Width / 2 - (Size.X / 2f);
            var posY = Winder.Height - Size.Y - 30;

            Position = new Vector2(posX, posY);

            System.Console.WriteLine("DIALOG BOX CONSTRUCTOR");
            System.Console.WriteLine(Size.X);
            System.Console.WriteLine(_characterSize.X);
        }

        public void Initialize(string text = null)
        {
            Text = text ?? Text;
            _currentPage = 0;
            Show();
        }

        public void Show()
        {
            Active = true;
            // use stopwatch to manage blinking indicator
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
            _pages = WordWrap(Text);
        }

        public void Hide()
        {
            Active = false;
            _stopwatch.Stop();
            _stopwatch = null;
        }

        public void Update()
        {
            if (Active)
            {
                // Button press will proceed to the next page of the dialog box
                if ((Input.kCurrent.IsKeyDown(Keys.Y) && Input.kPrevious.IsKeyUp(Keys.Y)) ||
                    (Input.gCurrent.Buttons.A == ButtonState.Pressed))
                {
                    if (_currentPage >= _pages.Count - 1)
                    {
                        Hide();
                    }
                    else
                    {
                        _currentPage++;
                        _stopwatch.Restart();
                    }
                }

                // Shortcut button to skip entire dialog box
                if ((Input.kCurrent.IsKeyDown(Keys.X) && Input.kPrevious.IsKeyUp(Keys.X)) ||
                    (Input.gCurrent.Buttons.X == ButtonState.Pressed))
                {
                    Hide();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Active)
            {
                // Draw each side of the border rectangle
                foreach (var side in BorderRectangles)
                {
                    System.Console.WriteLine("SIDE");
                    System.Console.WriteLine(side.ToString());
                    spriteBatch.Draw(_borderTexture, side, side, Color.White);
                }


                // Draw background fill texture (in this example, it's 50% transparent white)
                spriteBatch.Draw(_fillTexture, TextRectangle, TextRectangle, Color.White);

                // Draw the current page onto the dialog box
                spriteBatch.DrawString(Winder.spriteFont, _pages[_currentPage], TextPosition, DialogColor);

                if (BlinkIndicator() || _currentPage == _pages.Count - 1)
                {
                    var indicatorPosition = new Vector2(TextRectangle.X + TextRectangle.Width - (_characterSize.X) - 4,
                        TextRectangle.Y + TextRectangle.Height - (_characterSize.Y));

                    spriteBatch.DrawString(Winder.spriteFont, ">", indicatorPosition, Color.Red);
                }
            }
        }

        /// Whether the indicator should be visible or not
        private bool BlinkIndicator()
        {
            _interval = (int)Math.Floor((double)(_stopwatch.ElapsedMilliseconds % 1000));

            return _interval < 500;
        }

        /// Wrap words to the next line where applicable
        private List<string> WordWrap(string text)
        {
            var pages = new List<string>();

            var capacity = MaxCharsPerLine * MaxLines > text.Length
                ? text.Length
                : MaxCharsPerLine * MaxLines;

            var result = new StringBuilder(capacity);
            var resultLines = 0;

            var currentWord = new StringBuilder();
            var currentLine = new StringBuilder();

            for (var i = 0; i < text.Length; i++)
            {
                var currentChar = text[i];
                var isNewLine = text[i] == '\n';
                var isLastChar = i == text.Length - 1;

                currentWord.Append(currentChar);

                if (char.IsWhiteSpace(currentChar) || isLastChar)
                {
                    var potentialLength = currentLine.Length + currentWord.Length;

                    if (potentialLength > MaxCharsPerLine)
                    {
                        result.AppendLine(currentLine.ToString());

                        currentLine.Clear();

                        resultLines++;
                    }

                    currentLine.Append(currentWord);

                    currentWord.Clear();

                    if (isLastChar || isNewLine)
                    {
                        result.AppendLine(currentLine.ToString());
                    }

                    if (resultLines > MaxLines || isLastChar || isNewLine)
                    {
                        pages.Add(result.ToString());

                        result.Clear();

                        resultLines = 0;

                        if (isNewLine)
                        {
                            currentLine.Clear();
                        }
                    }
                }
            }

            return pages;
        }
    }
}
