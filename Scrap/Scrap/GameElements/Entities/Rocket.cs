using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Scrap.GameElements.Entities
{
    class Rocket : Entity
    {
        public float force = 100f;

        public Rocket(ScrapGame game)
            : base(game)
        {
            texture = game.Content.Load<Texture2D>("Rocket");
        }


        public Rocket(ScrapGame game, Vector2 position)
            : base(game)
        {
            texture = game.Content.Load<Texture2D>("Rocket");
            body = BodyFactory.CreateRoundedRectangle(((ScrapGame)game).world,1f,1f,.2f,.2f,5,1f);
            body.BodyType = BodyType.Dynamic;
            body.Position = position;
            body.Restitution = .1f;
            body.Friction = .9f;
        }

        public override void Update(GameTime gameTime)
        {
            //ToDo:control hack
            if (game.inputManager.WasKeyReleased(Microsoft.Xna.Framework.Input.Keys.Up) && gameTime.TotalGameTime.Milliseconds % 60 > 50)
            {//TODO: Extend Math or Vector2 to include rotation
                float cos = (float)Math.Cos(body.Rotation);
                float sin = (float)Math.Sin(body.Rotation);

                // Vector2.Transform(thrust, Quaternion.CreateFromAxisAngle(new Vector3(0, 0, 1), physicsBody.Rotation));
                //body.ApplyLinearImpulse(new Vector2(0 * cos - force * sin, 0 * sin + force * cos));
                body.ApplyForce(new Vector2(force * sin, -force * cos));
                //body.ApplyLinearImpulse(new Vector2(force * sin, -force * cos));
            }
        }



    }
}
