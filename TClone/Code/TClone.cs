using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace TClone
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    class TClone : Game
    {
        public static readonly int WIDTH = 10;
        public static readonly int HEIGHT = 18;
        public static readonly int TILESIZE = 24;

        public static TClone instance;
        public static Texture2D pixel;
        public static SpriteFont font;

        public static Rectangle playArea = new Rectangle(0, 0, WIDTH * TILESIZE, HEIGHT * TILESIZE);
        public static Rectangle nextArea = new Rectangle(WIDTH * TILESIZE + TILESIZE, TILESIZE, TILESIZE * 5, TILESIZE * 6);
        public static Rectangle holdArea = new Rectangle(WIDTH * TILESIZE + TILESIZE, TILESIZE * 8, TILESIZE * 5, TILESIZE * 6);
        
        GameBoard gameBoard;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        bool startup = true;
        bool paused = false;
        
        public TClone()
        {
            if(instance != null) {
                Exit();
            }
            instance = this;

            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = 384;
            graphics.PreferredBackBufferHeight = 432;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });
            Block.texture = Content.Load<Texture2D>("block");

            font = Content.Load<SpriteFont>("font");
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if(startup && KeystateHelper.IsKeyReleased(Keys.Enter)) {
                startup = false;

                gameBoard = new GameBoard();
            }
            if (!startup && KeystateHelper.IsKeyReleased(Keys.Escape))
                paused = !paused;

            KeystateHelper.Update();
            if(!paused) gameBoard?.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(pixel, playArea, Color.Purple);

            spriteBatch.DrawString(font, "Next: ", new Vector2(WIDTH * TILESIZE + TILESIZE, 4), Color.Black);
            spriteBatch.Draw(pixel, nextArea, Color.Purple);

            spriteBatch.DrawString(font, "Hold: ", new Vector2(WIDTH * TILESIZE + TILESIZE, TILESIZE * 7 + 4), Color.Black);
            spriteBatch.Draw(pixel, holdArea, Color.Purple);

            if (paused)
                spriteBatch.DrawString(font, "PAUSED", new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Color.Black);

            gameBoard?.Draw(spriteBatch);

            if (startup) {
                spriteBatch.DrawString(font, "Welcome to TClone", Vector2.Zero, Color.Black);
                spriteBatch.DrawString(font, "Controls: ", new Vector2(0, font.LineSpacing), Color.Black);
                spriteBatch.DrawString(font, "Arrows: Move Tile", new Vector2(0, font.LineSpacing * 2), Color.Black);
                spriteBatch.DrawString(font, "Control: Rotate Tile", new Vector2(0, font.LineSpacing * 3), Color.Black);
                spriteBatch.DrawString(font, "Shift: Hold Tile", new Vector2(0, font.LineSpacing * 4), Color.Black);
                spriteBatch.DrawString(font, "Pause/Unpause Game: Escape", new Vector2(0, font.LineSpacing * 5), Color.Black);
                spriteBatch.DrawString(font, "Start Game: Enter", new Vector2(0, font.LineSpacing * 6), Color.Black);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
