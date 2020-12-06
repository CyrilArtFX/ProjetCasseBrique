using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace CasseBriqueGame
{
    public class Ball
    {
        public int sizeX;
        public int sizeY;
        public Vector2 position;
        public Texture2D texture;
        public Color color;
        public float speedX;
        public float speedY;

        private int screenSizeX;
        private int screenSizeY;

        private SoundEffect sidesSound;

        public enum CollisionSector
        {
            UpAndDown,
            LeftAndRight
        }


        public Ball(int sizeX, int sizeY, Vector2 position, float baseSpeedX, float baseSpeedY, GraphicsDevice graphicsDevice, Color color, int screenSizeX, int screenSizeY, SoundEffect sidesSound)
        {
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.position = position;
            speedX = baseSpeedX;
            speedY = baseSpeedY;

            texture = new Texture2D(graphicsDevice, sizeX, sizeY);
            this.color = color;
            this.sidesSound = sidesSound;

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

        public void UpdateBall()
        {
            position.X += speedX;
            position.Y += speedY;

            if (position.X <= 0) position.X = 0;
            if (position.X + sizeX >= screenSizeX) position.X = screenSizeX - sizeX;
            if (position.Y <= 0)
            {
                sidesSound.Play();
                position.Y = 0;
            }
            if (position.Y + sizeY >= screenSizeY) position.Y = screenSizeY * 3;
            if (position.X <= 0 || position.X + sizeX >= screenSizeX)
            {
                sidesSound.Play();
                speedX = -speedX;
            }
            if (position.Y <= 0 || position.Y + sizeY >= screenSizeY) speedY = -speedY;
        }

        //For collisionZone : this is a int between -100 (up or left) and 100 (down or right) which contains the zone on the col where the ball bounce 

        public void Collision(Bar col, CollisionSector sector)
        {
            if(sector == CollisionSector.UpAndDown)
            {
                position.Y += -5;
                speedY = -speedY;
                int collisionZone = (int)((((col.sizeX - ((position.X + sizeX / 2) - col.position.X)) / col.sizeX) - 0.5) * -200);
                speedX = collisionZone / 20;
            }
            if(sector == CollisionSector.LeftAndRight)
            {
                position.X += -speedX;
                speedX = -speedX;
                int collisionZone = (int)((((col.sizeY - ((position.Y + sizeY / 2) - col.position.Y)) / col.sizeY) - 0.5) * -200);
                speedY = collisionZone / 20;
            }
        }

        public void Collision(Brick col, CollisionSector sector)
        {
            if (sector == CollisionSector.UpAndDown)
            {
                position.Y += -speedY;
                speedY = -speedY;
            }
            if (sector == CollisionSector.LeftAndRight)
            {
                position.X += -speedX;
                speedX = -speedX;
            }
        }
    }
}