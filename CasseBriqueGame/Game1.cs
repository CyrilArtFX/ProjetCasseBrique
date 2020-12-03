using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System;

namespace CasseBriqueGame
{
    public class Game1 : Game
    {
        public enum State
        {
            Game,
            StartGame,
            GameOver,
            Menu
        }
        public State gameState;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private GraphicsDevice graphicsDevice;
        private int screenSizeX;
        private int screenSizeY;

        private float mousePositionX;
        private float mousePositionY;

        public Bar bar;
        public Ball ball;
        public List<Brick> bricks = new List<Brick>();
        public List<Brick> bricksDelete = new List<Brick>();

        public Button startGameButton;
        public Button returnMenuButton;
        public Button retryButton;
        public Button[] selectDifficultyButtons = new Button[5];

        private int leftClickPressedCooldown = 0;
        private bool isLeftClickPressed = false;

        private String gameOverMessage = "";
        private SpriteFont fontLarge;
        private SpriteFont fontSmall;

        private int difficulty;

        List<SoundEffect> soundEffects = new List<SoundEffect>();
        

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        protected override void Initialize()
        {
            gameState = State.Menu;
            graphicsDevice = this.GraphicsDevice;
            screenSizeX = graphicsDevice.Viewport.Width;
            screenSizeY = graphicsDevice.Viewport.Height;

            soundEffects.Add(Content.Load<SoundEffect>("Sounds/Bar")); // [0]
            soundEffects.Add(Content.Load<SoundEffect>("Sounds/Sides")); // [1]
            soundEffects.Add(Content.Load<SoundEffect>("Sounds/Brick")); // [2]
            soundEffects.Add(Content.Load<SoundEffect>("Sounds/Win")); // [3]
            soundEffects.Add(Content.Load<SoundEffect>("Sounds/Defeat")); // [4]
            soundEffects.Add(Content.Load<SoundEffect>("Sounds/Menu")); // [5]

            bar = new Bar(200, 20, new Vector2(screenSizeX/2 - 100, 6 * screenSizeY/7), graphicsDevice, Color.White, screenSizeX, screenSizeY, soundEffects[0]);
            ball = new Ball(20, 20, new Vector2(screenSizeX/2 - 10, (6 * screenSizeY / 7) - 22), 0, 3, graphicsDevice, Color.White, screenSizeX, screenSizeY, soundEffects[1]);

            startGameButton = new Button(166, 80, new Vector2(screenSizeX / 4 - 88, 4 * screenSizeY / 6), "Start Game", graphicsDevice, Color.White, Color.Black, soundEffects[5]);
            returnMenuButton = new Button(180, 80, new Vector2(screenSizeX / 4 - 90, 4 * screenSizeY / 6), "Return Menu", graphicsDevice, Color.White, Color.Black, soundEffects[5]);
            retryButton = new Button(112, 80, new Vector2((3 * screenSizeX / 4) - 56, 4 * screenSizeY / 6), "Retry", graphicsDevice, Color.White, Color.Black, soundEffects[5]);

            selectDifficultyButtons[0] = new Button(40, 40, new Vector2((3 * screenSizeX / 4) - 66, (4 * screenSizeY / 6) - 3), "1", graphicsDevice, Color.YellowGreen, Color.White, soundEffects[5]);
            selectDifficultyButtons[1] = new Button(40, 40, new Vector2((3 * screenSizeX / 4) - 20, (4 * screenSizeY / 6) - 3), "2", graphicsDevice, Color.Yellow, Color.White, soundEffects[5]);
            selectDifficultyButtons[2] = new Button(40, 40, new Vector2((3 * screenSizeX / 4) + 26, (4 * screenSizeY / 6) - 3), "3", graphicsDevice, Color.Orange, Color.White, soundEffects[5]);
            selectDifficultyButtons[3] = new Button(40, 40, new Vector2((3 * screenSizeX / 4) - 43, (4 * screenSizeY / 6) + 43), "4", graphicsDevice, Color.Crimson, Color.White, soundEffects[5]);
            selectDifficultyButtons[4] = new Button(40, 40, new Vector2((3 * screenSizeX / 4) + 3, (4 * screenSizeY / 6) + 43), "5", graphicsDevice, Color.DarkRed, Color.White, soundEffects[5]);
            selectDifficultyButtons[0].isPressed = true;
            difficulty = 1;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            fontLarge = Content.Load<SpriteFont>("LargeFont");
            fontSmall = Content.Load<SpriteFont>("SmallFont");
        }

        protected override void Update(GameTime gameTime)
        {
            if(IsActive)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

                mousePositionX = Mouse.GetState().X;
                mousePositionY = Mouse.GetState().Y;
                ButtonState mouseLeftButtonState = Mouse.GetState().LeftButton;

                if(leftClickPressedCooldown >= 1)
                {
                    isLeftClickPressed = false;
                    leftClickPressedCooldown++;
                }
                if(leftClickPressedCooldown >= 20)
                {
                    leftClickPressedCooldown = 0;
                }
                if(mouseLeftButtonState == ButtonState.Pressed && leftClickPressedCooldown == 0)
                {
                    isLeftClickPressed = true;
                    leftClickPressedCooldown++;
                }

                if(gameState == State.Game)
                {
                    ball.UpdateBall();
                    bar.UpdateBar(mousePositionX, ball);
                    foreach (Brick brick in bricks)
                    {
                        brick.UpdateBrick(ball);
                        if (brick.life <= 0) bricksDelete.Add(brick);
                    }
                    DeleteBricks(bricksDelete);
                    bricksDelete.Clear();
                    if (ball.position.Y >= screenSizeY * 2) GameOver(false);
                    if (bricks.Count <= 0) GameOver(true);
                }
                if(gameState == State.StartGame)
                {
                    bar.UpdateBar(mousePositionX, ball);
                    ball.position.X = (bar.position.X + bar.sizeX / 2) - ball.sizeX / 2;

                    if (isLeftClickPressed) gameState = State.Game;
                }
                if(gameState == State.Menu)
                {
                    startGameButton.UpdateButton(mousePositionX, mousePositionY, isLeftClickPressed);
                    if (startGameButton.isPressed) StartGame();
                    for(int i = 0; i < 5; i++)
                    {
                        selectDifficultyButtons[i].UpdateButton(mousePositionX, mousePositionY, isLeftClickPressed);
                        if (selectDifficultyButtons[i].isPressed)
                        {
                            difficulty = i + 1;
                            for(int j = 0; j < 5; j++)
                            {
                                if (j != i) selectDifficultyButtons[j].isPressed = false;
                            }
                        }
                    }
                }
                if(gameState == State.GameOver)
                {
                    returnMenuButton.UpdateButton(mousePositionX, mousePositionY, isLeftClickPressed);
                    retryButton.UpdateButton(mousePositionX, mousePositionY, isLeftClickPressed);
                    if (retryButton.isPressed) StartGame();
                    if (returnMenuButton.isPressed) GoMenu();
                }
                
                base.Update(gameTime);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();

            if(gameState == State.Game || gameState == State.StartGame)
            {
                _spriteBatch.Draw(bar.texture, bar.position, bar.color);
                _spriteBatch.Draw(ball.texture, ball.position, ball.color);
                foreach(Brick brick in bricks)
                {
                    _spriteBatch.Draw(brick.texture, brick.position, brick.color);
                }
            }
            if(gameState == State.GameOver)
            {
                _spriteBatch.DrawString(fontLarge, gameOverMessage, new Vector2(screenSizeX / 2 - screenSizeX / 7, screenSizeY / 4), Color.White);
                _spriteBatch.Draw(returnMenuButton.texture, returnMenuButton.position, returnMenuButton.color);
                _spriteBatch.DrawString(fontSmall, returnMenuButton.buttonText.ToUpper(), new Vector2(returnMenuButton.position.X + 5, returnMenuButton.position.Y + 10), returnMenuButton.textColor);
                _spriteBatch.Draw(retryButton.texture, retryButton.position, retryButton.color);
                _spriteBatch.DrawString(fontSmall, retryButton.buttonText.ToUpper(), new Vector2(retryButton.position.X + 15, retryButton.position.Y + 10), retryButton.textColor);
            }
            if(gameState == State.Menu)
            {
                _spriteBatch.DrawString(fontLarge, "Casse Briques", new Vector2(screenSizeX / 2 - screenSizeX / 4, screenSizeY / 4), Color.White);
                _spriteBatch.Draw(startGameButton.texture, startGameButton.position, startGameButton.color);
                _spriteBatch.DrawString(fontSmall, startGameButton.buttonText.ToUpper(), new Vector2(startGameButton.position.X + 5, startGameButton.position.Y + 10), startGameButton.textColor);
                foreach(Button button in selectDifficultyButtons)
                {
                    _spriteBatch.Draw(button.texture, button.position, button.color);
                    _spriteBatch.DrawString(fontSmall, button.buttonText, new Vector2(button.position.X + 5, button.position.Y + 1), button.textColor);
                }
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

        private void StartGame()
        {
            retryButton.isPressed = false;
            startGameButton.isPressed = false;
            ball.position = new Vector2(screenSizeX / 2 - 10, (6 * screenSizeY / 7) - 22);
            bar.position = new Vector2(screenSizeX / 2 - 100, 6 * screenSizeY / 7);
            bricks.Clear();
            GenerateLevel(difficulty);
            gameState = State.StartGame;
        }

        private void GameOver(bool win)
        {
            if (win)
            {
                soundEffects[3].Play();
                gameOverMessage = "VICTORY !";
            }
            else
            {
                soundEffects[4].Play();
                gameOverMessage = "DEFEAT...";
            }
            gameState = State.GameOver;
        }

        private void GoMenu()
        {
            returnMenuButton.isPressed = false;
            gameState = State.Menu;
        }

        private void GenerateLevel(int levelDifficulty)
        {
            if (levelDifficulty > 5) levelDifficulty = 5;
            if (levelDifficulty < 1) levelDifficulty = 1;
            int numberOfBricks = 6 + 6 * levelDifficulty; //12, 18, 24, 30, 36
            int numberOfLinesWithBricks = 3;
            if (levelDifficulty == 1) numberOfLinesWithBricks = 3;
            if (levelDifficulty == 2 || levelDifficulty == 3) numberOfLinesWithBricks = 4;
            if (levelDifficulty == 4 || levelDifficulty == 5) numberOfLinesWithBricks = 5;

            int[] numberOfBrickByLine = new int[numberOfLinesWithBricks];

            if (levelDifficulty == 1 || levelDifficulty == 2) numberOfBrickByLine[0] = 3;
            if (levelDifficulty == 3 || levelDifficulty == 4) numberOfBrickByLine[0] = 4;
            if (levelDifficulty == 5) numberOfBrickByLine[0] = 5;

            if (levelDifficulty == 1 || levelDifficulty == 2) numberOfBrickByLine[1] = 4;
            if (levelDifficulty == 3 || levelDifficulty == 4) numberOfBrickByLine[1] = 5;
            if (levelDifficulty == 5) numberOfBrickByLine[1] = 6;

            if (levelDifficulty == 1 || levelDifficulty == 2) numberOfBrickByLine[2] = 5;
            if (levelDifficulty == 3 || levelDifficulty == 5) numberOfBrickByLine[2] = 7;
            if (levelDifficulty == 4) numberOfBrickByLine[2] = 6;

            if (levelDifficulty == 2) numberOfBrickByLine[3] = 6;
            if (levelDifficulty == 3 || levelDifficulty == 5) numberOfBrickByLine[3] = 8;
            if (levelDifficulty == 4) numberOfBrickByLine[3] = 7;

            if (levelDifficulty == 4) numberOfBrickByLine[4] = 8;
            if (levelDifficulty == 5) numberOfBrickByLine[4] = 10;

            int sizeYofBricks = (int)(((screenSizeY / 2) / numberOfLinesWithBricks) - 6);
            int[] sizeXofBricksByLine = new int[numberOfLinesWithBricks];
            for(int i = 0; i < numberOfLinesWithBricks; i++)
            {
                sizeXofBricksByLine[i] = (int)((screenSizeX / numberOfBrickByLine[i]) - 6);
            }

            int minLifeOfBricks = 1;
            if (levelDifficulty == 1 || levelDifficulty == 2) minLifeOfBricks = 1;
            if (levelDifficulty == 3 || levelDifficulty == 4) minLifeOfBricks = 2;
            if (levelDifficulty == 5) minLifeOfBricks = 3;
            int maxLifeOfBricks = 5;
            if (levelDifficulty == 1) maxLifeOfBricks = 2;
            if (levelDifficulty == 2) maxLifeOfBricks = 3;
            if (levelDifficulty == 3) maxLifeOfBricks = 4;
            if (levelDifficulty == 4 || levelDifficulty == 5) maxLifeOfBricks = 5;
            Random random = new Random();


            for (int i = 0; i < numberOfLinesWithBricks; i++)
            {
                for(int j = 0; j < numberOfBrickByLine[i]; j++)
                {
                    bricks.Add(new Brick(sizeXofBricksByLine[i], sizeYofBricks, new Vector2((j * (sizeXofBricksByLine[i] + 6)) + 6, ((numberOfLinesWithBricks - 1 - i) * (sizeYofBricks + 6)) + 6), random.Next(minLifeOfBricks, maxLifeOfBricks + 1), graphicsDevice, soundEffects[2]));
                }
            }

        }
    }
}
