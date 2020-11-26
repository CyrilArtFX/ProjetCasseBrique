using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

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


        public Bar(int sizeX, int sizeY, Vector2 position, GraphicsDevice graphicsDevice, Color color, int screenSizeX, int screenSizeY)
        {
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.position = position;
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
    }
}
