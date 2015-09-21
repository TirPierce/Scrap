using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarseerPhysics.Dynamics.Joints;

namespace Scrap.GameElements.Entities
{
    [Serializable]
    public class Wheel : Segment
    {
        Body wheel;
        Joint bodyJoint;
        public Wheel(ScrapGame game, Vector2 position)
            : base(game)
        {
            texture = game.Content.Load<Texture2D>("wheel");

            wheel = BodyFactory.CreateCircle(game.world, .49f, 1f, this);
            wheel.Restitution = .5f;
            wheel.BodyType = BodyType.Dynamic;
            wheel.Position = position;
            wheel.Friction = .9f;



            body = BodyFactory.CreateRectangle(game.world, .5f, .5f, .5f);
            body.Position = position;
            body.BodyType = BodyType.Dynamic;
            body.Restitution = .5f;
            body.Friction = .9f;
            

            bodyJoint = JointFactory.CreateRevoluteJoint(game.world, body, wheel, new Vector2(0, 0), new Vector2(0, 0));

        }

        public override void Update(GameTime gameTime)
        {
            
        }
        public override Body GetJointAnchor(Direction direction)
        {
            //if direction.up has anchorable point return it
            return body;
        }
        //public override Joint Join(Entity otherEntity, Direction direction)
        //{
        //    return JointFactory.CreateRevoluteJoint(game.world, otherEntity.wheel, wheel, new Vector2(0, 1), new Vector2(0, 0));
            
        //} 
    }
}
