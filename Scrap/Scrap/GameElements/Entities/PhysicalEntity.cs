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

        //public virtual Joint Join(Body body)
        //{

        //}

    }
}
