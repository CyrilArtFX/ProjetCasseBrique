using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace CasseBriqueGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private GraphicsDevice graphicsDevice;
        private int screenSizeX;
        private int screenSizeY;

        private float mousePositionX;

        public Bar bar;
        public Ball ball;
        public List<Brick> bricks = new List<Brick>();
        public List<Brick> bricksDelete = new List<Brick>();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        protected override void Initialize()
        {
            graphicsDevice = this.GraphicsDevice;
            screenSizeX = graphicsDevice.Viewport.Width;
            screenSizeY = graphicsDevice.Viewport.Height;

            bar = new Bar(200, 20, new Vector2(screenSizeX/2 - 100, 6 * screenSizeY/7), graphicsDevice, Color.White, screenSizeX, screenSizeY);
            ball = new Ball(20, 20, new Vector2(screenSizeX/2 - 10, 4 * screenSizeY/5), 3, 3, graphicsDevice, Color.White, screenSizeX, screenSizeY);
            bricks.Add(new Brick(50, 40, new Vector2(100, 100), 1, graphicsDevice));
            bricks.Add(new Brick(100, 80, new Vector2(300, 100), 5, graphicsDevice));
            bricks.Add(new Brick(50, 40, new Vector2(500, 100), 1, graphicsDevice));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if(IsActive)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

                mousePositionX = Mouse.GetState().X;

                ball.UpdateBall();
                bar.UpdateBar(mousePositionX, ball);
                foreach (Brick brick in bricks)
                {
                    brick.UpdateBrick(ball);
                    if (brick.life <= 0) bricksDelete.Add(brick);
                }
                DeleteBricks(bricksDelete);
                bricksDelete.Clear();

                base.Update(gameTime);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();

            _spriteBatch.Draw(bar.texture, bar.position, bar.color);
            _spriteBatch.Draw(ball.texture, ball.position, ball.color);
            foreach(Brick brick in bricks)
            {
                _spriteBatch.Draw(brick.texture, brick.position, brick.color);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DeleteBricks(List<Brick> bricksToDelete)
        {
            foreach(Brick brickToDelete in bricksToDelete)
            {
                bricks.Remove(brickToDelete);
            }
        }
    }
}
