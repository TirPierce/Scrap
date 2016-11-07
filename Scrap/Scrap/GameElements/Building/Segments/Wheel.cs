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
using Scrap.GameElements.Building;

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
            sprite = new Rendering.Sprite(game.Content.Load<Texture2D>("wheel"), 100,100, 1f);
            wheel = BodyFactory.CreateCircle(game.world, .49f, 1f, (object)this);
            wheel.Restitution = .5f;
            wheel.BodyType = BodyType.Dynamic;
            wheel.Position = position;
            wheel.Friction = .9f;
            //wheel.CollisionCategories = new Category();


            body = BodyFactory.CreateRectangle(game.world, .5f, .5f, .5f, (object)this);
            body.Position = position;
            body.BodyType = BodyType.Dynamic;
            body.Restitution = .5f;
            body.Friction = .9f;

            wheel.SleepingAllowed = body.SleepingAllowed = false;

            bodyJoint = JointFactory.CreateRevoluteJoint(game.world, body, wheel, new Vector2(0, 0), new Vector2(0, 0));
            
        }
        public override Direction[] JointDirections()
        {
            Direction[] validDirections = { Direction.Down, Direction.Right, Direction.Up, Direction.Left };
            return validDirections;
        }
        public override void Update(GameTime gameTime)
        {
            
        }
        public override Body GetJointAnchor(Direction direction)
        {
            //if direction.up has anchorable point return it
            return body;
        }
        public override void Draw(SpriteBatch batch)
        {
            switch (constructElement.Status)
            {
                case ElementStatus.Locked:
                    sprite.Draw(batch, wheel.WorldCenter, wheel.Rotation, Color.Cyan);
                    break;
                case ElementStatus.Selected:
                    sprite.Draw(batch, wheel.WorldCenter, wheel.Rotation, Color.Green);
                    break;
                case ElementStatus.Attached:
                    sprite.Draw(batch, wheel.WorldCenter, wheel.Rotation, Color.White);
                    break;
                case ElementStatus.Free:
                    sprite.Draw(batch, wheel.WorldCenter, wheel.Rotation, Color.White);
                    break;
                default:
                    sprite.Draw(batch, wheel.WorldCenter, wheel.Rotation, Color.White);
                    break;
            }
        }
        //public override Joint Join(Entity otherEntity, Direction direction)
        //{
        //    return JointFactory.CreateRevoluteJoint(game.world, otherEntity.wheel, wheel, new Vector2(0, 1), new Vector2(0, 0));
            
        //} 
    }
}
