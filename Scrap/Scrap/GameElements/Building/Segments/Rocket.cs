using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Scrap.GameElements.Building;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Scrap.GameElements.Entities
{
    class Rocket : Segment
    {
        public float force = 100f;

        public Rocket(ScrapGame game)
            : base(game)
        {
            sprite = new Rendering.Sprite(game.Content.Load<Texture2D>("Rocket"), 100,100, 1f);
        }


        public Rocket(ScrapGame game, Vector2 position)
            : base(game)
        {
            sprite = new Rendering.Sprite(game.Content.Load<Texture2D>("Rocket"), 100,100, 1f);
            sprite.AddAnimation("fire", 1, 3, 1, true);
            body = BodyFactory.CreateRoundedRectangle(((ScrapGame)game).world, 1f, 1f, .2f, .2f, 5, 1f, (object)this);
            body.BodyType = BodyType.Dynamic;
            body.Position = position;
            body.Restitution = .1f;
            body.Friction = .9f;
            
        }
        public override Direction[] JointDirections()
        {
            Direction[] validDirections = { Direction.Down, Direction.Right, Direction.Up, Direction.Left };
            return validDirections;
        }
        public override void AnalogueInputCallback(float percentage)
        {//ToDo: this is a bit hacky
            if (constructElement.construct != null)
            {
                if (percentage > 0)
                {
                    percentage *= 50;

                    float cos = (float)Math.Cos(body.Rotation - MathHelper.PiOver2);
                    float sin = (float)Math.Sin(body.Rotation - MathHelper.PiOver2);

                    body.ApplyForce(new Vector2(cos, sin) * percentage);
                    sprite.SetCurrentAnimation("fire");

                }
                else
                    sprite.SetCurrentAnimation("Default");
            }
        }

        public override void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
        }



    }
}
