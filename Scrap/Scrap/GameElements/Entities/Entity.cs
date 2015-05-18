using FarseerPhysics.Dynamics.Joints;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Scrap.GameElements.Entities 
{

    [Serializable]
    public abstract class Entity
    {

        protected Texture2D texture;

        private Vector2 position;
        private float rotation;
        protected string objectType;
        protected ScrapGame game;

        public virtual Vector2 Position { set; get; }
        public virtual float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public Entity()
        {
        }

        public Entity(ScrapGame game)
        {
            this.game = game;
            game.entityList.Add(this);
        }

        public virtual void Init(ScrapGame game)
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
