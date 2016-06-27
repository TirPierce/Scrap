﻿using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Scrap.GameElements.Entities
{
    [Serializable]
    public class Crate : Segment
    {
        public Crate(ScrapGame game)
            : base(game)
        {
            sprite = new Rendering.Sprite(game.Content.Load<Texture2D>("Crate"),0,false);
            //texture = game.Content.Load<Texture2D>("Crate");
        }
        //Category

        public Crate(ScrapGame game, Vector2 position)
            : base(game)
        {
            sprite = new Rendering.Sprite(game.Content.Load<Texture2D>("Crate"), 0, false);
            body = BodyFactory.CreateRoundedRectangle(((ScrapGame)game).world, 1f, 1f, .2f, .2f, 5, 2f, this);
            body.BodyType = BodyType.Dynamic;
            body.Position = position;
            body.Restitution = .1f;
            body.Friction = .9f;
            
        }
        public override Direction[] JointDirections()
        {
            Direction[] validDirections = { Direction.Down, Direction.Right};
            return validDirections;
        }
        public override void Update(GameTime gameTime)
        {
        }



    }
}
