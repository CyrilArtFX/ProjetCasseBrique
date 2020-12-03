using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace CasseBriqueGame
{
    public class Button
    {
        public int sizeX;
        public int sizeY;
        public Vector2 position;
        public string buttonText;
        public Texture2D texture;
        public Color baseColor;
        public Color color;
        public Color textColor;

        public bool isPressed = false;
        private SoundEffect menuSound;


        public Button(int sizeX, int sizeY, Vector2 position, string buttonText, GraphicsDevice graphicsDevice, Color color, Color textColor, SoundEffect menuSound)
        {
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.position = position;
            this.buttonText = buttonText;
            texture = new Texture2D(graphicsDevice, sizeX, sizeY);
            baseColor = color;
            this.color = color;
            this.textColor = textColor;

            this.menuSound = menuSound;

            SetColorData();
        }

        private void SetColorData()
        {
            Color[] colorData = new Color[sizeX * sizeY];
            for (int i = 0; i < sizeX * sizeY; i++)
                colorData[i] = color;
            texture.SetData<Color>(colorData);
        }

        public void UpdateButton(float mouseX, float mouseY, bool leftClick)
        {
            if ((mouseX > position.X && mouseX < position.X + sizeX) && (mouseY > position.Y && mouseY < position.Y + sizeY))
            {
                color = Color.Gray;
                if (leftClick)
                {
                    menuSound.Play();
                    isPressed = true;
                }
            }
            else if(!isPressed) color = baseColor;
            if (isPressed) color = Color.DarkGray;
        }
    }
}
