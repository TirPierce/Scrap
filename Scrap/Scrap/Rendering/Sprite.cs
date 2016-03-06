using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap.Rendering
{
    public class Sprite
    {
        Dictionary<String, Tuple<int, int>> animations;
        String currentAnimation;
        int currentFrame;

        public void Update(GameTime gameTime)
        {

        }
        private void AddAnimation(string name, int startFrame, int endFrame)
        {
            animations.Add(name, new Tuple<int, int>(startFrame, endFrame));
        }
        public void Draw(SpriteBatch batch, Vector2 worldCenter, float rotation, Color color)
        {
            batch.Draw(texture, worldCenter, null, color, rotation, new Vector2(FrameWidth / 2f, FrameHeight / 2f), .01f * (100f / (float)FrameWidth), SpriteEffects.None, 1);
        }
        public Texture2D Texture
        {
            get { return texture; }
        }
        Texture2D texture;
        public float FrameTime
        {
            get { return frameTime; }
        }
        float frameTime;
        public bool IsLooping
        {
            get { return isLooping; }
        }
        bool isLooping;
        public int FrameCount
        {
            get { return Texture.Width / FrameWidth; }
        }
        public int FrameWidth
        {
            get { return Texture.Height; }
        }
        public int FrameHeight
        {
            get { return Texture.Height; }
        }    
        public Sprite(Texture2D texture, float frameTime, bool isLooping)
        {
            this.texture = texture;
            this.frameTime = frameTime;
            this.isLooping = isLooping;
        }
    }
}

