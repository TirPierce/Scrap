using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Scrap.Rendering;
using Scrap.UserInterface;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Scrap.GameElements.Entities
{
    public enum Direction { Up, Right, Down, Left};
    


    public abstract class Segment
    {
        public static float DirectionToRadians(Direction direction)
        {
            switch (direction)
            {

                case Direction.Up:
                    return MathHelper.Pi;
                case Direction.Right:
                    return 3* MathHelper.PiOver2;
                case Direction.Down:
                    return 0;
                case Direction.Left:
                    return MathHelper.PiOver2;
                default:
                    return 0;
            }
        }

        public Sprite sprite;
        protected string objectType;
        protected ScrapGame game;
        


        public Body body;
        public ConstructElement constructElement;

        public virtual Vector2 Position
        {
            get { return body.Position; }
            set { body.Position = value; }
        }
        public virtual float Rotation
        {
            get { return body.Rotation; }
            set { body.Rotation = value; }
        }
        public Segment(ScrapGame game)
        {
            this.game = game;
            game.entityList.Add(this);
            constructElement = new ConstructElement(game, this);
            
        }
        public virtual void Update(GameTime gameTime)
        {
   
        }
        
        public bool IsPointContained(ref Vector2 point)
        {
            Transform t = new Transform();
            body.GetTransform(out t);
            return body.FixtureList[0].Shape.TestPoint(ref t, ref point);

        }
        
        public virtual Direction[] JointDirections()
        {
            Direction[] validDirections = { Direction.Up, Direction.Down, Direction.Left, Direction.Right };
            return validDirections;
        }

        public virtual void Draw(SpriteBatch batch)
        {
            switch (constructElement.Status)
            {
                case ElementStatus.Locked:
                    sprite.Draw(batch, body.WorldCenter, body.Rotation, Color.Cyan);
                    break;
                case ElementStatus.Selected:
                    sprite.Draw(batch, body.WorldCenter, body.Rotation, Color.Green);
                    break;
                case ElementStatus.Attached:
                    sprite.Draw(batch, body.WorldCenter, body.Rotation, Color.White);
                    break;
                case ElementStatus.Free:
                    sprite.Draw(batch, body.WorldCenter, body.Rotation, Color.White);
                    break;
                default:
                    sprite.Draw(batch, body.WorldCenter, body.Rotation, Color.White);
                    break;
            }
            
        }
        public virtual Body GetJointAnchor(Direction direction)
        {
            //if direction.xy has anchorable point return it
            return body;
        }

        //Sensor creation

    }
}
