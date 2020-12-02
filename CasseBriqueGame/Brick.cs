using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CasseBriqueGame
{
    public class Brick
    {
        public int sizeX;
        public int sizeY;
        public Vector2 position;
        public Texture2D texture;
        public Color color;

        public int life;


        public Brick(int sizeX, int sizeY, Vector2 position, int life, GraphicsDevice graphicsDevice)
        {
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.position = position;
            this.life = life;

            texture = new Texture2D(graphicsDevice, sizeX, sizeY);
            SetColorData(SetColorUsingLife());
        }

        private Color SetColorUsingLife()
        {
            if (life >= 5) return Color.DarkRed;
            else if (life == 4) return Color.OrangeRed;
            else if (life == 3) return Color.Orange;
            else if (life == 2) return Color.Yellow;
            else if (life == 1) return Color.YellowGreen;
            else return Color.White;
        }

        private void SetColorData(Color color)
        {
            this.color = color;
            Color[] colorData = new Color[sizeX * sizeY];
            for (int i = 0; i < sizeX * sizeY; i++)
                colorData[i] = this.color;
            texture.SetData<Color>(colorData);
        }

        public void UpdateBrick(Ball ball)
        {
            if (ball.position.X + ball.sizeX > position.X && ball.position.X < position.X + sizeX) //The ball is at the same positionX at the brick
            {
                if (ball.position.Y + ball.sizeY >= position.Y && ball.position.Y < position.Y + sizeY) //The ball is at the same position at the brick
                {
                    ball.Collision(this, Ball.CollisionSector.UpAndDown);
                    life--;
                    SetColorData(SetColorUsingLife());
                    //if we get here, that means the ball was at the same positionX at the brick, but was not in the brick, then the ball enters
                    //the brick by the Up or the Down side
                }
            }
            else if (ball.position.Y + ball.sizeY >= position.Y && ball.position.Y < position.Y + sizeY) //The ball is at the same height at the brick
            {
                if(ball.position.X + ball.sizeX > position.X && ball.position.X < position.X + sizeX) //The ball is at the same position at the brick
                {
                    ball.Collision(this, Ball.CollisionSector.LeftAndRight);
                    life--;
                    SetColorData(SetColorUsingLife());
                    //if we get here, that means the ball was at the same heigt at the brick, but was not in the brick, then the ball enters
                    //the brick by the Left or the Right side
                }
            }
        }

    }
}
