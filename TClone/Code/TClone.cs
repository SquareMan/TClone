using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace TClone
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class TClone : Game
    {
        public static readonly int WIDTH = 10;
        public static readonly int HEIGHT = 18;
        public static readonly int TILESIZE = 16;

        public static TClone instance;
        public static Texture2D pixel;

        GameBoard gameBoard;

        Rectangle playArea = new Rectangle(0, 0, WIDTH * TILESIZE, HEIGHT * TILESIZE);

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        public TClone()
        {
            if(instance != null) {
                Exit();
            }
            instance = this;
            gameBoard = new GameBoard();

            gameBoard.PlacePrefab(Block.prefab, new Point(5, HEIGHT - 5));
            gameBoard.PlacePrefab(Block.prefab, new Point(5, 1));

            graphics = new GraphicsDeviceManager(this);
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
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            gameBoard.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

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
            gameBoard.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
