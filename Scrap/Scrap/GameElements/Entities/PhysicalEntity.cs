using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Scrap.GameElements.Entities
{
    public abstract class PhysicalEntity : Entity
    {
        protected float density;
        protected float restitution;
        protected float friction;
        protected Body body;

        public PhysicalEntity(ScrapGame game):base(game)
        {

        }

        public override Vector2 Position { get { return body.Position; } }
        public override void Update(GameTime gameTime)
        {
            
        }
        public override float Rotation
        {
            get { return body.Rotation; }
            set { body.Rotation = value; }
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
        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, body.WorldCenter, null, Color.White, body.Rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), .01f * (100f / (float)texture.Width), SpriteEffects.None, 0);

        }
        //public virtual Joint Join(Body body)
        //{

        //}

    }
}
