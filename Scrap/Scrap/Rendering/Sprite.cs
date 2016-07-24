using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap.Rendering
{
    struct Animation
    {
        public Animation(string name, int startFrame, int endFrame, float frameDuration, bool isLooping)
        {
            this.name= name;
            this.startFrame = startFrame;
            this.endFrame=endFrame;
            this.frameDuration=frameDuration;
            this.isLooping = isLooping;
        }
        public string name; 
        public int startFrame; 
        public int endFrame; 
        float frameDuration; 
        public bool isLooping;
    }
    public class Sprite
    {
        Dictionary<String,Animation> animations;
        Animation currentAnimation;
        public int frameWidth;
        public int frameHeight;
        int currentFrame;
        float scale =1f;
         
        public void SetCurrentAnimation(String animationName)
        {
            if (animations.ContainsKey(animationName))
            {
                if (currentAnimation.name != animationName )
                {
                    currentAnimation = animations[animationName];
                    currentFrame = 0;
                }
                else if(currentAnimation.name == animationName && !currentAnimation.isLooping)
                {//If animation is looping then it should continue
                    currentFrame = 0;
                }
            }
        }
        public String GetCurrentAnimation()
        {
            return currentAnimation.name;
        }
        public void Update(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.Milliseconds % 60 > 50) 
            { 
                currentFrame++;
                if (currentFrame >= currentAnimation.endFrame)
                    currentFrame = currentAnimation.startFrame;
            }
            
        }
        public void AddAnimation(string name, int startFrame, int endFrame, float frameDuration, bool isLooping)
        {
            animations.Add(name, new Animation(name, startFrame, endFrame, frameDuration, isLooping));
        }
        public void Draw(SpriteBatch batch, Vector2 worldCenter, float rotation, Color color)
        {//ToDo: itteration through this texture should jump line on the y when it gets to bottom line
            
            if(currentAnimation.name == "fire")
            {
                int t = 0;
            }
            int globalFrameIndex = currentFrame + currentAnimation.startFrame;

            int frameY = globalFrameIndex > 0 ? globalFrameIndex/(texture.Width / frameWidth): 0;
            int frameX = globalFrameIndex > 0 ? globalFrameIndex %(texture.Width / frameWidth) : 0;
            Rectangle sourceRectangle = new Rectangle(frameX*frameWidth, frameY*frameHeight, frameWidth, frameHeight);
            batch.Draw(texture, worldCenter, sourceRectangle, color, rotation, new Vector2(frameWidth/2,frameHeight/2), 1f / (float)frameWidth * scale, SpriteEffects.None, 1);
        }
        public Texture2D Texture
        {
            get { return texture; }
        }
        Texture2D texture;

        public Sprite(Texture2D texture,int frameWidth, int frameHeight, float scale)
        {
            this.scale = scale;
            this.texture = texture;
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            animations = new Dictionary<string, Animation>();
            AddAnimation("Default", 0,0, 1, true);
        }
    }
}

