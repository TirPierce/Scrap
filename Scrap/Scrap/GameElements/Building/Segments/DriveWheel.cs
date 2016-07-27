using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Scrap.GameElements.Building;
using Scrap.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace Scrap.GameElements.Entities
{
    [Serializable]
    class DriveWheel : Segment
    {

        Body wheel;
        Joint bodyJoint;
        public DriveWheel(ScrapGame game, Vector2 position)
            : base(game)
        {
            sprite = new Rendering.Sprite(game.Content.Load<Texture2D>("DriveWheel"), 100,100, 1f);
            wheel = BodyFactory.CreateCircle(game.world, .49f, 8f, (object)this);
            wheel.Restitution = .5f;
            wheel.BodyType = BodyType.Dynamic;
            wheel.Position = position;
            wheel.Friction = 5f;
            //wheel.CollisionCategories = new Category();


            body = BodyFactory.CreateRectangle(game.world, .5f, .5f, .5f, (object)this);
            body.Position = position;
            body.BodyType = BodyType.Dynamic;
            body.Restitution = .5f;
            body.Friction = .9f;

            bodyJoint = JointFactory.CreateRevoluteJoint(game.world, body, wheel, new Vector2(0, 0), new Vector2(0, 0));
            

        }
        public override Direction[] JointDirections()
        {
            Direction[] validDirections = { Direction.Down, Direction.Right, Direction.Up, Direction.Left };
            return validDirections;
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
        public override void Update(GameTime gameTime)
        {
            //ToDo:control hack
            //if ( game.inputManager.WasKeyReleased(Microsoft.Xna.Framework.Input.Keys.Space)&& gameTime.TotalGameTime.Milliseconds % 60 >50 )
            //{
            //    wheel.ApplyTorque(-50f);
            //}
            sprite.Update(gameTime);
        }
        public override void AnalogueInputCallback(float percentage)
        {
            if (constructElement.construct != null)
            {
                percentage *= 10;
                wheel.ApplyTorque(percentage);
            }
        }

    }
}
