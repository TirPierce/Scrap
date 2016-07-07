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
            sprite = new Rendering.Sprite(game.Content.Load<Texture2D>("Rocket"), 0, false, 1f);
        }


        public Rocket(ScrapGame game, Vector2 position)
            : base(game)
        {
            sprite = new Rendering.Sprite(game.Content.Load<Texture2D>("Rocket"), 0, false, 1f);
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
        public override void Update(GameTime gameTime)
        {
            //ToDo:control hack
            if (InputManager.GetManager().KeyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up) && gameTime.TotalGameTime.Milliseconds % 60 > 50)
            {//TODO: Extend Math or Vector2 to include rotation
                float cos = (float)Math.Cos(body.Rotation - MathHelper.PiOver2);
                float sin = (float)Math.Sin(body.Rotation - MathHelper.PiOver2);

                body.ApplyForce(new Vector2(cos, sin)*50f);
                //body.ApplyForce(new Vector2(force * sin, -force * cos));
            }
        }



    }
}
