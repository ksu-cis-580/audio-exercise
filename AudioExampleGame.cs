using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using AudioExercise.Collisions;

namespace AudioExercise;

/// <summary>
/// A game demostrating the use of audio content
/// </summary>
public class AudioExampleGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    private CoinSprite[] _coins;
    private SlimeGhostSprite _slimeGhost;
    private SpriteFont _spriteFont;
    private int _coinsLeft;

    private Texture2D _ball;

    /// <summary>
    /// A game demonstrating collision detection
    /// </summary>
    public AudioExampleGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    /// <summary>
    /// Initializes the game 
    /// </summary>
    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        MathHelper.Random rand = new();
        _coins =
        [
            new CoinSprite(new Vector2(rand.NextFloat() * GraphicsDevice.Viewport.Width, rand.NextFloat() * GraphicsDevice.Viewport.Height)),
            new CoinSprite(new Vector2(rand.NextFloat() * GraphicsDevice.Viewport.Width, rand.NextFloat() * GraphicsDevice.Viewport.Height)),
            new CoinSprite(new Vector2(rand.NextFloat() * GraphicsDevice.Viewport.Width, rand.NextFloat() * GraphicsDevice.Viewport.Height)),
            new CoinSprite(new Vector2(rand.NextFloat() * GraphicsDevice.Viewport.Width, rand.NextFloat() * GraphicsDevice.Viewport.Height)),
            new CoinSprite(new Vector2(rand.NextFloat() * GraphicsDevice.Viewport.Width, rand.NextFloat() * GraphicsDevice.Viewport.Height)),
            new CoinSprite(new Vector2(rand.NextFloat() * GraphicsDevice.Viewport.Width, rand.NextFloat() * GraphicsDevice.Viewport.Height)),
            new CoinSprite(new Vector2(rand.NextFloat() * GraphicsDevice.Viewport.Width, rand.NextFloat() * GraphicsDevice.Viewport.Height))
        ];
        _coinsLeft = _coins.Length;
        _slimeGhost = new SlimeGhostSprite();

        base.Initialize();
    }

    /// <summary>
    /// Loads content for the game
    /// </summary>
    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        foreach (var coin in _coins) coin.LoadContent(Content);
        _slimeGhost.LoadContent(Content);
        _spriteFont = Content.Load<SpriteFont>("arial");
        _ball = Content.Load<Texture2D>("ball");
    }

    /// <summary>
    /// Updates the game world
    /// </summary>
    /// <param name="gameTime">The game time</param>
    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        _slimeGhost.Update(gameTime);

        // Detect and process collisions
        _slimeGhost.Color = Color.White;
        foreach (var coin in _coins)
        {
            if(!coin.Collected && coin.Bounds.CollidesWith(_slimeGhost.Bounds))
            {
                _slimeGhost.Color = Color.Red;
                coin.Collected = true;
                _coinsLeft--;
            }
        }

        base.Update(gameTime);
    }

    /// <summary>
    /// Draws the game world
    /// </summary>
    /// <param name="gameTime">The game time</param>
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();
        foreach (var coin in _coins)
        {
            coin.Draw(gameTime, _spriteBatch);
            /*
            var rect = new Rectangle((int)(coin.Bounds.Center.X - coin.Bounds.Radius),
                                        (int)(coin.Bounds.Center.Y - coin.Bounds.Radius),
                                        (int)(2*coin.Bounds.Radius), (int)(2*coin.Bounds.Radius));
            spriteBatch.Draw(ball, rect, Color.White);
            */
        }
        /*
        var rectG = new Rectangle((int)(slimeGhost.Bounds.Center.X - slimeGhost.Bounds.Radius),
                                        (int)(slimeGhost.Bounds.Center.Y - slimeGhost.Bounds.Radius),
                                        (int)(2 * slimeGhost.Bounds.Radius), (int)(2 * slimeGhost.Bounds.Radius));
        spriteBatch.Draw(ball, rectG, Color.White);
        */
        _slimeGhost.Draw(gameTime, _spriteBatch);
        _spriteBatch.DrawString(_spriteFont, $"Coins left: {_coinsLeft}", new Vector2(2,2), Color.Gold);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
