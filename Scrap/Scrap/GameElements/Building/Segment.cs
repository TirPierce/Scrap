using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Scrap.GameElements.Entities
{
    public enum Direction { Left, Right, Up, Down };
    public enum SegmentStatus { Locked, Selected, Attached, Free };
    [Serializable]
    public abstract class Segment
    {
        public const float OFFSET = 1.2f;
        public Texture2D texture;
        protected string objectType;
        protected ScrapGame game;
        private SegmentStatus status = SegmentStatus.Free;//ToDo: Make {locked, selected,attached, free} enum status variable

        public SegmentStatus Status
        {
            get { return status; }
            set { status = value; }
        }
        public Body body;
        public ConstructElement constructElement;
        List<Fixture> sensors = new List<Fixture>();

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
            GenerateGUIButtons();
        }
        public virtual void Update(GameTime gameTime)
        {
            switch (status)
            {
                case SegmentStatus.Locked:
                    
                    break;
                case SegmentStatus.Selected:
                    
                    break;
                case SegmentStatus.Attached:
                    
                    break;
                case SegmentStatus.Free:
                    
                    break;
                default:
                    
                    break;
            }      
        }
        public bool TestEntity(ref Vector2 point)
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
        public void DrawGUI()
        {
        }
        public virtual void Draw(SpriteBatch batch)
        {
            switch (status)
            {
                case SegmentStatus.Locked:
                    batch.Draw(texture, body.WorldCenter, null, Color.Cyan, body.Rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), .01f * (100f / (float)texture.Width), SpriteEffects.None, 1);

                    break;
                case SegmentStatus.Selected:
                    batch.Draw(texture, body.WorldCenter, null, Color.Green, body.Rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), .01f * (100f / (float)texture.Width), SpriteEffects.None, 1);

                    break;
                case SegmentStatus.Attached:
                    batch.Draw(texture, body.WorldCenter, null, Color.White, body.Rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), .01f * (100f / (float)texture.Width), SpriteEffects.None, 1);

                    break;
                case SegmentStatus.Free:
                    batch.Draw(texture, body.WorldCenter, null, Color.White, body.Rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), .01f * (100f / (float)texture.Width), SpriteEffects.None, 1);

                    break;
                default:
                    batch.Draw(texture, body.WorldCenter, null, Color.White, body.Rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), .01f * (100f / (float)texture.Width), SpriteEffects.None, 0);

                    break;
            }
        }
        public virtual Body GetJointAnchor(Direction direction)
        {
            //if direction.xy has anchorable point return it
            return body;
        }

        //Sensor creation
        public void RemoveSensors()
        {
            foreach (Fixture sensor in sensors)
            {
                sensor.Dispose();
            }
        }

        private void PlaceSegment()//temp function
        {
            status = SegmentStatus.Attached;
        }
        public void EnableButtons(List<Direction> validDirections)
        {

        }
        private void GenerateGUIButtons()
        {
            foreach (Direction direction in JointDirections())//The 
            {
                this.game.gui.AddButton(this, direction, new Action(PlaceSegment));//Buttons should be inactive when created. Drawing should be based on button activity
            }
        }
        public virtual void AddSensors()
        {
            Fixture sensorFixture;
            Vertices verts;
            foreach (var direction in JointDirections())
            {
                switch (direction)
                {
                    case Direction.Left:
                        verts = new Vertices();
                        verts.Add(new Vector2(-.6f, -.4f));
                        verts.Add(new Vector2(-.6f, .4f));
                        verts.Add(new Vector2(-1.2f, 0f));
                        break;
                    case Direction.Right:
                        verts = new Vertices();
                        verts.Add(new Vector2(1.2f, 0f));
                        verts.Add(new Vector2(.6f, .4f));
                        verts.Add(new Vector2(.6f, -.4f));
                        break;
                    case Direction.Up:
                        verts = new Vertices();
                        verts.Add(new Vector2(-.4f, -.6f));
                        verts.Add(new Vector2(0f, -1.2f));
                        verts.Add(new Vector2(.4f, -.6f));
                        break;
                    case Direction.Down:
                        verts = new Vertices();
                        verts.Add(new Vector2(.4f, .6f));
                        verts.Add(new Vector2(0f, 1.2f));
                        verts.Add(new Vector2(-.4f, .6f));
                        break;
                    default:
                        verts = new Vertices();
                        break;
                }
                sensorFixture = FixtureFactory.AttachPolygon(verts, 0f, body, direction);

                sensors.Add(sensorFixture);
                sensorFixture.OnCollision += OnCollide;
                sensorFixture.CollidesWith = Category.Cat10;
                sensorFixture.CollisionCategories = Category.Cat10;

            }

        }
        public static Vector2 GetSensorOffset(Scrap.GameElements.Entities.Direction direction)
        {
            Vector2 offset = Vector2.Zero;
            if (direction == Direction.Right)
            {
                offset = Vector2.UnitX * OFFSET;

            }
            if (direction == Direction.Left)
            {
                offset = Vector2.UnitX * -OFFSET;

            }
            if (direction == Direction.Down)
            {
                offset = Vector2.UnitY * OFFSET;

            }
            if (direction == Direction.Up)
            {
                offset = Vector2.UnitY * -OFFSET;

            }
            return offset;
        }
        public bool OnCollide(Fixture a, Fixture b, Contact contact)
        {
            if (this.game.playerController.selectedSegment != null)
            {
                game.playerController.OnConstructSensorTriggered(this.constructElement, (Direction)a.UserData);
            }
            return true;

        }

    }
}
