﻿using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Scrap.GameElements.Entities
{
    [Serializable]
    public abstract class PhysicalEntity : Entity
    {
        protected float density;
        protected float restitution;
        protected float friction;
        protected Body body;

        public PhysicalEntity():base()
        {

        }

        public PhysicalEntity(ScrapGame game):base(game)
        {

        }

        public override void Init(ScrapGame game)
        {
            base.Init(game);
        }

        public override Vector2 Position { get { return body.Position; } }
        public override void Update(GameTime gameTime)
        {
            
        }
        public override float Rotation//here //when loading there is no instance of body
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
