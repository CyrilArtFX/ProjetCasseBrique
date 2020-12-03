using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace CasseBriqueGame
{
    public class Bar
    {
        public int sizeX;
        public int sizeY;
        public Vector2 position;
        public Texture2D texture;
        public Color color;

        private int screenSizeX;
        private int screenSizeY;

        private SoundEffect barSound;


        public Bar(int sizeX, int sizeY, Vector2 position, GraphicsDevice graphicsDevice, Color color, int screenSizeX, int screenSizeY, SoundEffect barSound)
        {
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.position = position;
            texture = new Texture2D(graphicsDevice, sizeX, sizeY);
            this.color = color;
            this.barSound = barSound;

            SetColorData();
            UpdateScreenSizeDatas(screenSizeX, screenSizeY);
        }

        private void SetColorData()
        {
            Color[] colorData = new Color[sizeX * sizeY];
            for (int i = 0; i < sizeX * sizeY; i++)
                colorData[i] = color;
            texture.SetData<Color>(colorData);
        }

        public void UpdateScreenSizeDatas(int newSizeX, int newSizeY)
        {
            screenSizeX = newSizeX;
            screenSizeY = newSizeY;
        }

        public void UpdateBar(float mouseX, Ball ball)
        {
            position.X = mouseX - sizeX / 2;
            if (position.X < 0) position.X = 0;
            if (position.X + sizeX > screenSizeX) position.X = screenSizeX - sizeX;

            if (ball.position.Y + ball.sizeY >= position.Y && ball.position.Y < position.Y + ball.speedY + 0.5f && ball.position.X + ball.sizeX > position.X && ball.position.X < position.X + sizeX)
            {
                barSound.Play();
                ball.Collision(this, Ball.CollisionSector.UpAndDown);
            }
        }
    }
}
