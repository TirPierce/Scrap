using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Joints;

namespace Scrap.GameElements.Entities
{
    [Serializable]
    public class Wheel : Entity
    {
        Body wheelHub;
        Joint wheelHubJoint;
        public Wheel(ScrapGame game, Vector2 position)
            : base(game)
        {
            texture = game.Content.Load<Texture2D>("wheel");
            
            body = BodyFactory.CreateCircle(game.world, .49f, 1f);
            body.Restitution = .5f;
            body.BodyType = BodyType.Dynamic;
            body.Position = position;
            body.Friction = .9f;

            wheelHub = BodyFactory.CreateRectangle(game.world, .1f,.1f, .1f);
            wheelHub.Position = position;
            wheelHub.BodyType = BodyType.Dynamic;

            wheelHubJoint = JointFactory.CreateRevoluteJoint(game.world, wheelHub, body, new Vector2(0, 0), new Vector2(0, 0));

        }
        public override void Update(GameTime gameTime)
        {

        }
        public override Body GetJointAnchor(Direction direction)
        {
            //if direction.up has anchorable point return it
            return wheelHub;
        }
        //public override Joint Join(Entity otherEntity, Direction direction)
        //{
        //    return JointFactory.CreateRevoluteJoint(game.world, otherEntity.body, body, new Vector2(0, 1), new Vector2(0, 0));
            
        //} 
    }
}
