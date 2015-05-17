using FarseerPhysics.Dynamics.Joints;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Scrap.GameElements.Entities 
{

    public abstract class Entity
    {

        protected Texture2D texture;

        protected Vector2 position;
        protected float rotation;
        protected string objectType;
        protected ScrapGame game;

        public virtual Vector2 Position { get { return position; } }
        public virtual float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public Entity(ScrapGame game)
        {
            this.game = game;
            game.entityList.Add(this);
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, position, null, Color.White, rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), .01f * (100f / (float)texture.Width), SpriteEffects.None, 0);

        }
    }
}
