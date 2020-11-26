using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

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


        public Ball(int sizeX, int sizeY, Vector2 position, float baseSpeedX, float baseSpeedY, GraphicsDevice graphicsDevice, Color color, int screenSizeX, int screenSizeY)
        {
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.position = position;
            speedX = baseSpeedX;
            speedY = baseSpeedY;

            texture = new Texture2D(graphicsDevice, sizeX, sizeY);
            this.color = color;

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

        private void UpdateScreenSizeDatas(int newSizeX, int newSizeY)
        {
            screenSizeX = newSizeX;
            screenSizeY = newSizeY;
        }

        public void UpdateBall()
        {
            position.X += speedX;
            position.Y += speedY;

            if (position.X <= 0 || position.X + sizeX >= screenSizeX) speedX = -speedX;
            if (position.Y <= 0 || position.Y + sizeY >= screenSizeY) speedY = -speedY; 
        }
    }
}