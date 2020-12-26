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
        /// All text contained in this dialog box
        public string Text { get; set; }

        /// Bool that determines active state of this dialog box
        public bool Active { get; private set; }

        /// X,Y coordinates of this dialog box
        public Vector2 Position { get; set; }

        /// Width and Height of this dialog box
        public Vector2 Size { get; set; }

        /// Color used to fill dialog box background
        public Color FillColor { get; set; }

        /// Color used for border around dialog box
        public Color BorderColor { get; set; }

        /// Color used for text in dialog box
        public Color DialogColor { get; set; }

        /// Thickness of border
        public int BorderWidth { get; set; }

        /// Background fill texture (built from FillColor)
        private readonly Texture2D _fillTexture;

        /// Border fill texture (built from BorderColor)
        private readonly Texture2D _borderTexture;

        /// Collection of pages contained in this dialog box
        private List<string> _pages;

        /// Margin surrounding the text inside the dialog box
        private const float DialogBoxMargin = 24f;

        /// Size (in pixels) of a wide alphabet letter (W is the widest letter in almost every font) 
        private Vector2 _characterSize = Winder.spriteFont.MeasureString(new StringBuilder("W", 1));

        /// The amount of characters allowed on a given line
        /// NOTE: If you want to use a font that is not monospaced, this will need to be reevaluated
        private int MaxCharsPerLine => (int) Math.Floor((Size.X - DialogBoxMargin)/_characterSize.X);

        /// Determine the maximum amount of lines allowed per page
        /// NOTE: This will change automatically with font size
        private int MaxLines => (int) Math.Floor((Size.Y - DialogBoxMargin)/_characterSize.Y) - 1;

        /// The index of the current page
        private int _currentPage;

        /// The stopwatch interval (used for blinking indicator)
        private int _interval;

        /// The position and size of the dialog box fill rectangle
        private Rectangle TextRectangle => new Rectangle(Position.ToPoint(), Size.ToPoint());

        /// The position and size of the bordering sides on the edges of the dialog box
        private List<Rectangle> BorderRectangles => new List<Rectangle>
        {
            // Top (contains top-left & top-right corners)
            new Rectangle(TextRectangle.X - BorderWidth, TextRectangle.Y - BorderWidth,
                TextRectangle.Width + BorderWidth*2, BorderWidth),

            // Right
            new Rectangle(TextRectangle.X + TextRectangle.Size.X, TextRectangle.Y, BorderWidth, TextRectangle.Height),

            // Bottom (contains bottom-left & bottom-right corners)
            new Rectangle(TextRectangle.X - BorderWidth, TextRectangle.Y + TextRectangle.Size.Y,
                TextRectangle.Width + BorderWidth*2, BorderWidth),

            // Left
            new Rectangle(TextRectangle.X - BorderWidth, TextRectangle.Y, BorderWidth, TextRectangle.Height)
        };

        /// The starting position of the text inside the dialog box
        private Vector2 TextPosition => new Vector2(Position.X + DialogBoxMargin/2, Position.Y + DialogBoxMargin/2);

        /// Stopwatch used for the blinking (next page) indicator
        private Stopwatch _stopwatch;

        /// Default constructor
        public DialogBox()
        {
            BorderWidth = 2;
            DialogColor = Color.Black;

            FillColor = new Color(1.0f, 1.0f, 1.0f, 0.5f);

            BorderColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);
            

            //_fillTexture = new Texture2D(Winder.graphicsDevice, 1, 1);
           //
            //_borderTexture = new Texture2D(Winder.graphicsDevice, 1, 1);
            //_borderTexture.SetData(new[] {BorderColor});

            _pages = new List<string>();
            _currentPage = 0;

            var sizeX = (int) (Winder.Width*0.5);
            var sizeY = (int) (Winder.Height*0.2);

            Size = new Vector2(sizeX, sizeY);

            var posX = Winder.Width - (Size.X/2f);
            var posY = Winder.Height - Size.Y - 30;

            Position = new Vector2(posX, posY);
        }

        /// Initialize a dialog box
        /// - can be used to reset the current dialog box in case of "I didn't quite get that..."
        /// <param name="text"></param>
        public void Initialize(string text = null)
        {
            Text = text ?? Text;

            _currentPage = 0;

            Show();
        }

        /// Show the dialog box on screen
        /// - invoke this method manually if Text changes
        public void Show()
        {
            Active = true;

            // use stopwatch to manage blinking indicator
            _stopwatch = new Stopwatch();

            _stopwatch.Start();

            _pages = WordWrap(Text);
        }

        /// Manually hide the dialog box
        public void Hide()
        {
            Active = false;

            _stopwatch.Stop();

            _stopwatch = null;
        }

        /// Process input for dialog box
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

        /// Draw the dialog box on screen if it's currently active
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (Active)
            {
                // // Draw each side of the border rectangle
                // foreach (var side in BorderRectangles)
                // {
                //     spriteBatch.Draw(_borderTexture, (20, 20), side, Color.White);
                // }

                // // Draw background fill texture (in this example, it's 50% transparent white)
                // spriteBatch.Draw(_fillTexture, null, TextRectangle);

                // Draw the current page onto the dialog box
                spriteBatch.DrawString(Winder.spriteFont, _pages[_currentPage], TextPosition, DialogColor);

                // Draw a blinking indicator to guide the player through to the next page
                // This stops blinking on the last page
                // NOTE: You probably want to use an image here instead of a string
                if (BlinkIndicator() || _currentPage == _pages.Count - 1)
                {
                    var indicatorPosition = new Vector2(TextRectangle.X + TextRectangle.Width - (_characterSize.X) - 4,
                        TextRectangle.Y + TextRectangle.Height - (_characterSize.Y));

                    spriteBatch.DrawString(Winder.spriteFont, ">", indicatorPosition, Color.Red);
                }
            }
        }

        /// Whether the indicator should be visible or not
        /// <returns></returns>
        private bool BlinkIndicator()
        {
            _interval = (int) Math.Floor((double) (_stopwatch.ElapsedMilliseconds%1000));

            return _interval < 500;
        }

        /// Wrap words to the next line where applicable
        /// <param name="text"></param>
        /// <returns></returns>
        private List<string> WordWrap(string text)
        {
            var pages = new List<string>();

            var capacity = MaxCharsPerLine*MaxLines > text.Length ? text.Length : MaxCharsPerLine*MaxLines;

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
