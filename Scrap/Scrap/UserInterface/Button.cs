using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;


namespace Scrap.UserInterface
{
    /// <summary>
    /// Does not contain logic beyond maintaining its appearance. 
    /// </summary>
    /// 
    class Button
    {
        protected Texture2D mTexture;
        public Rectangle mButtonArea;


        public Button(Point position, int width, int height, Texture2D sprite)
        {
            mTexture = sprite;
            mButtonArea = new Rectangle(position.X, position.Y, width, height);

        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(mTexture, mButtonArea, Color.White);
        }

        public bool OverButton(Point clickPos)
        {
            return mButtonArea.Contains(clickPos);
        }

    }
}
