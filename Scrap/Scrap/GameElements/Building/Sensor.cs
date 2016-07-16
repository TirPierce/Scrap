using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Scrap.GameElements.Entities;
using Scrap.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap.GameElements.Building
{

    public class Sensor
    {
        public Body body;
        ScrapGame game;
        public ConstructElement constructElement;
        float rotationRad;
        private Direction orientation;
        public Sprite sprite;

        public Point GetOffsetRelativeToConstruct()
        {



            if (constructElement.orientation == Direction.Down)
            {
                switch (orientation)
                {
                    case Direction.Up:
                        return Orientation.DirectionToPoint(Direction.Down);
                    case Direction.Right:
                        return Orientation.DirectionToPoint(Direction.Left);
                    case Direction.Down:
                        return Orientation.DirectionToPoint(Direction.Up);
                    case Direction.Left:
                        return Orientation.DirectionToPoint(Direction.Right);
                    default:
                        return offSet;
                }
            }
            else if (constructElement.orientation == Direction.Left)
            {
                switch (orientation)
                {
                    case Direction.Up:
                        return Orientation.DirectionToPoint(Direction.Left);
                    case Direction.Right:
                        return Orientation.DirectionToPoint(Direction.Up);
                    case Direction.Down:
                        return Orientation.DirectionToPoint(Direction.Right);
                    case Direction.Left:
                        return Orientation.DirectionToPoint(Direction.Down);
                    default:
                        return offSet;
                }
            }
            else if (constructElement.orientation == Direction.Right)
            {
                switch (orientation)
                {
                    case Direction.Up:
                        return Orientation.DirectionToPoint(Direction.Right);
                    case Direction.Right:
                        return Orientation.DirectionToPoint(Direction.Down);
                    case Direction.Down:
                        return Orientation.DirectionToPoint(Direction.Left);
                    case Direction.Left:
                        return Orientation.DirectionToPoint(Direction.Up);
                    default:
                        return offSet;
                }
            }
            else return offSet;
        }
        public Direction GetOrientationRelativeToSegment()
        {
            return orientation;
        }
        public Point offSet;
        bool enabled = false;
        Joint joint;
        
        public Sensor(ConstructElement constructElement, Direction direction, ScrapGame game) 
        {

            sprite = new Rendering.Sprite(game.Content.Load<Texture2D>("PipeShort"), 0, false, 1f);
            switch (direction)
            {
                case Direction.Up:
                    sprite = new Rendering.Sprite(game.Content.Load<Texture2D>("Up"), 0, false, 1f);
                    break;
                case Direction.Right:
                    sprite = new Rendering.Sprite(game.Content.Load<Texture2D>("Right"), 0, false, 1f);
                    break;
                case Direction.Down:
                    sprite = new Rendering.Sprite(game.Content.Load<Texture2D>("Down"), 0, false, 1f);
                    break;
                case Direction.Left:
                    sprite = new Rendering.Sprite(game.Content.Load<Texture2D>("Left"), 0, false, 1f);
                    break;
                default:
                    break;
            }
            this.constructElement = constructElement;
            this.game = game;
            orientation = direction;
            offSet = Orientation.DirectionToPoint(direction);
            rotationRad = Orientation.DirectionToRadians(direction);

            //Debug.WriteLine("Sensor:" + direction.ToString() + " Construct element :" + constructElement.offSet.ToString());
            //Debug.WriteLine("Sensor offset:" + offSet.ToString() + " Rotation :" + rotationRad.ToString());

            //offSet = new Vector2((float)Math.Cos(rotationRad), (float)Math.Sin(rotationRad)) * ConstructElement.OFFSET;
            CreateBody();
            //element.segment.body.
        }


        private void CreateBody()
        {
            Vertices verts;
            verts = new Vertices();
            verts.Add(new Vector2(.4f, 0f));
            verts.Add(new Vector2(.4f, .4f));
            verts.Add(new Vector2(-.4f, .4f));
            verts.Add(new Vector2(-.4f, 0f));
            
            //constructElement.segment.body 
            //body.CreateFixture(new Shape());
            //constructElement.segment.body.CreateFixture(new PolygonShape(verts, 0);
            body = BodyFactory.CreatePolygon(((ScrapGame)game).world, verts, 1f, this);
            body.SetTransform(constructElement.segment.Position, constructElement.segment.Rotation);
            body.BodyType = BodyType.Dynamic;
            body.SleepingAllowed = false;
            body.CollidesWith = Category.Cat10;
            body.IsSensor = true;
            body.CollisionCategories = Category.Cat10;
            //body.OnCollision += OnCollide;
            body.IgnoreGravity = true;

            //JointFactory.CreateWeldJoint(this.game.world, body, constructElement.segment.body, Vector2.Zero, Vector2.Zero);
            
        }
        public bool OnCollide(Fixture a, Fixture b, Contact contact) 
        {
            return false; 
        }
        public void Disable()
        {
            //body.Position = Vector2.Zero;
            //body.Enabled = false;
            enabled = false;
            body.CollidesWith = Category.Cat11;
            body.CollisionCategories = Category.Cat11;
            //body.Dispose();
        }
        public void Enable()
        {
            body.CollidesWith = Category.Cat10;
            body.CollisionCategories = Category.Cat10;
            enabled = true;
        }
        private void MoveSensor()
        {
            float cos = (float)Math.Cos(constructElement.segment.body.Rotation);
            float sin = (float)Math.Sin(constructElement.segment.body.Rotation);
            Vector2 relativePosition = Orientation.GetRelativePositionOfADirection(this.orientation);
            Vector2 rotationVector = relativePosition;
            rotationVector = new Vector2(rotationVector.X * cos - rotationVector.Y * sin, rotationVector.X * sin + rotationVector.Y * cos);
            //+ rotationVector

            body.SetTransform(constructElement.segment.body.Position + rotationVector * .5f, Orientation.ToRadians(orientation) + this.constructElement.segment.Rotation);
        }
        //                float cos = (float)Math.Cos(body.Rotation - MathHelper.PiOver2);
        //float sin = (float)Math.Sin(body.Rotation - MathHelper.PiOver2);

        private Vector2 Rotate(float angle, float distance, Vector2 centre)
        {
            return new Vector2((float)(distance * Math.Cos(angle)), (float)(distance * Math.Sin(angle))) + centre;
        }
        public Vector2 RotateVector(Vector2 initialVector, float angle)
        {
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);
            Vector2 rotationVector = new Vector2(initialVector.X * cos - initialVector.Y * sin, initialVector.X * sin + initialVector.Y * cos);
            return rotationVector;
        }

        public void Update()
        {
            if (enabled)
            {
                MoveSensor();
            }
        }
        public virtual void Draw(SpriteBatch batch)
        {
            if (enabled)
            {
                sprite.Draw(batch, body.Position,body.Rotation+MathHelper.Pi, Color.White);
            }
        }
        public Body Body
        {
            get { return body; }
            set { body = value; }
        }
    }
}
