using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Scrap.GameElements.Entities;
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
        private Orientation orientation;
        public Orientation GetOrientationRelativeToConstruct()
        {
            return new Orientation(orientation.AddDirectionsAsClockwiseAngles(constructElement.orientation.Direction));
        }
        public Orientation GetOrientationRelativeToSegment()
        {
            return orientation;
        }
        public Point offSet;
        bool enabled = false;
        Joint joint;
        
        public Sensor(ConstructElement constructElement, Direction direction, ScrapGame game) 
        {
            
            
            this.constructElement = constructElement;
            this.game = game;
            orientation = new Orientation(direction);
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
            verts.Add(new Vector2(.4f, .6f));
            verts.Add(new Vector2(0f, 1.2f));
            verts.Add(new Vector2(-.4f, .6f));
            
            //constructElement.segment.body 
            //body.CreateFixture(new Shape());
            //constructElement.segment.body.CreateFixture(new PolygonShape(verts, 0);
            body = BodyFactory.CreatePolygon(((ScrapGame)game).world, verts, 1f, this);
            body.SetTransform(constructElement.segment.Position, constructElement.segment.Rotation);
            body.BodyType = BodyType.Dynamic;
            body.SleepingAllowed = false;
            body.CollidesWith = Category.Cat10;
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


            //float cos = (float)Math.Cos(constructElement.segment.body.Rotation);
            //float sin = (float)Math.Sin(constructElement.segment.body.Rotation);
            //Vector2 rotationVector = constructElement.segment.body.Position;
            //rotationVector = new Vector2(offSet.X * cos - offSet.Y * sin, offSet.X * sin + offSet.Y * cos);
            //+ rotationVector
            body.SetTransform(constructElement.segment.body.Position, constructElement.segment.body.Rotation + orientation.ToRadians());
        }
        public void Update()
        {
            if (enabled)
            {
                MoveSensor();
            }
        }

        public Body Body
        {
            get { return body; }
            set { body = value; }
        }
    }
}
