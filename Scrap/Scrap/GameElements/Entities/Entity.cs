using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Scrap.GameElements.Entities 
{

    public abstract class Entity
    {

        protected Texture2D texture;

        public Vector2 position;
        protected float rotation;
        protected float density;
        protected float restitution;
        protected float friction;
        public string objectType;
        private ScrapGame game;

        public Vector2 Position { get { return position; } }
        public float Rotation
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
        public float Density
        {
            get { return density; }
        }
        public float Restitution
        {
            get { return restitution; }
        }
        public float Friction
        {
            get { return friction; }
        }
        public virtual void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, position, null, Color.White);
        }


    }
}
