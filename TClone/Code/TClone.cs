using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TClone
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class TClone : Game
    {
        public static readonly int WIDTH = 10;
        public static readonly int HEIGHT = 18;

        public static TClone instance;

        Block[,] blocks = new Block[WIDTH,HEIGHT];
        Rectangle playArea = new Rectangle(0, 0, WIDTH, HEIGHT);

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        public TClone()
        {
            if(instance != null) {
                Exit();
            }
            instance = this;

            for (int x = 0; x < WIDTH; x++) {
                for (int y = 0; y < HEIGHT; y++) {
                    //blocks[x, y] = new Block(new Point(x, y), Color.Yellow);
                }
            }

            PlacePrefab(Block.prefab, new Point(5, 1));

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        public void PlacePrefab(Block.BlockPrefab prefab, Point origin) {
            foreach(Block b in prefab.blocks) {
                int offX = b.position.X + origin.X;
                int offY = b.position.Y + origin.Y;
                blocks[offX, offY] = new Block(new Point(offX, offY), b.color);
            }
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

            Block.texture = Content.Load<Texture2D>("block");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
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

            foreach(Block b in blocks) {
                b?.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            }

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
            
            foreach(Block b in blocks) {
                b?.Draw(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
