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

        public Vector2 Position { get { return position; } }
        public virtual float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public Entity(ScrapGame game)
        {
            this.game = game;
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, position, null, Color.White);
        }
    }
}
