using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
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
        public enum Direction { Left, Right, Up, Down}
        protected Texture2D texture;
        protected string objectType;
        protected ScrapGame game;
        public Body body;


        public virtual Vector2 Position
        {
            get { return body.Position; }
            set { body.Position = value; }
        }
        public virtual float Rotation
        {
            get { return body.Rotation; }
            set { body.Rotation = value; }
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
            batch.Draw(texture, body.WorldCenter, null, Color.White, body.Rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), .01f * (100f / (float)texture.Width), SpriteEffects.None, 0);
        }
        public virtual Body GetJointAnchor(Direction direction)
        {
            //if direction.up has anchorable point return it
            return body;
        }
        
    }
}
