﻿using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Scrap.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap.GameElements.Entities
{
    [Serializable]
    class DriveWheel : Segment
    {

        Body wheel;
        Joint wheelHubJoint;
        public DriveWheel(ScrapGame game, Vector2 position)
            : base(game)
        {
            sprite = new Sprite(game.Content.Load<Texture2D>("wheel"),0,false);

            wheel = BodyFactory.CreateCircle(game.world, .49f, 2f,this);
            wheel.Restitution = 1f;
            wheel.BodyType = BodyType.Dynamic;
            wheel.Position = position;
            wheel.Friction = 2f;

            wheel.UserData = this;

            body = BodyFactory.CreateRectangle(game.world, .1f, .1f, 2f);
            body.Position = position;
            body.BodyType = BodyType.Dynamic;
            
            
            //body.IgnoreCCD = true;
            
            wheelHubJoint = JointFactory.CreateRevoluteJoint(game.world, body, wheel, new Vector2(0, 0));
            


        }
        public override Vector2 Position
        {
            get { return body.Position; }
            set { body.Position = wheel.Position = value; }
        }
        public override float Rotation
        {
            get { return body.Rotation; }
            set { body.Rotation= wheel.Rotation = value; }
        }
        public override void Update(GameTime gameTime)
        {
            //ToDo:control hack
            //if ( game.inputManager.WasKeyReleased(Microsoft.Xna.Framework.Input.Keys.Space)&& gameTime.TotalGameTime.Milliseconds % 60 >50 )
            //{
            //    wheel.ApplyTorque(-50f);
            //}
        }
        public override Body GetJointAnchor(Direction direction)
        {
            //if direction.up has anchorable point return it
            return body;
        }
        public override Direction[] JointDirections()
        {
            Direction[] validDirections = { Direction.Down, Direction.Right };
            return validDirections;
        }


    }
}
