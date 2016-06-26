using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Scrap.GameElements.Building;
using Scrap.GameElements.GameWorld;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap.GameElements.Entities
{

    

    [Serializable]
    public abstract class Construct
    {
        protected List<Joint> joints;
        protected List<Segment> entities;
        private Segment keyObject;
        public Dictionary<Point, ConstructElement> buildElements;

        protected ScrapGame game;
        public static Point DirectionToPoint(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    return new Point(-1, 0);
                case Direction.Right:
                    return new Point(1, 0);
                case Direction.Up:
                    return new Point(0, -1);
                case Direction.Down:
                    return new Point(0, 1);
            }
            return new Point(0, 0);
        }
        public static Direction PointToDirection(Point point)
        {
            if(point == new Point(-1, 0))
                 return Direction.Left;
              if(point == new Point(1, 0))
                 return Direction.Right;
            if(point == new Point(0, 1))
                 return Direction.Up;
            return Direction.Down;
        }

        public Segment KeyObject
        {
            get { return keyObject; }
            set 
            {
                keyObject = value;
                entities.Add(keyObject);
                buildElements.Add(Point.Zero,keyObject.constructElement); 
                
            }
        }
        public Construct(ScrapGame game)
        {
            this.game = game;
            joints = new List<Joint>();
            entities = new List<Segment>();
            buildElements = new Dictionary<Point,ConstructElement>();
            game.constructList.Add(this);
        }
        public virtual void Update(GameTime gameTime)
        {
            foreach (var item in buildElements)
            {
                item.Value.Update();
            }
        }
        
        public void SetSegmentDirection(Segment segment, float rotation)
        {
            Debug.WriteLine("Construct.SetSegmentDirection Offset: " + segment.constructElement.offSet.ToString() + " Rotation: " + rotation.ToString());
            this.FreeObject(segment);
            //Point segmentPositon = segment.constructElement.offSet;
            //Joint oldJoint = segment.constructElement.rootJoint;
            //if(this.buildElements.FirstOrDefault(x=

                

            //Body bodyA = oldJoint.BodyA;
            //Body bodyB = oldJoint.BodyB;
            ////Todo: calculate valid directions
            //Vector2 offset = oldJoint.WorldAnchorA - oldJoint.WorldAnchorB;
            //game.world.RemoveJoint(segment.constructElement.rootJoint);
            //segment.body.SetTransform(segment.body.Position, segment.body.Rotation + rotation);
            ////up

            //segment.constructElement.rootJoint = JointFactory.CreateWeldJoint(game.world, bodyA, bodyB, new Vector2(0, 0), offset);
        
        }
        public bool ContainsFixture(Fixture a)
        {
            foreach (Segment entity in entities)
            {
                if (entity.body.FixtureList.Contains(a))
                {
                    return true;
                }
            }
            return false;
        }
        public Dictionary<Direction, ConstructElement> AdjacentElements(Point position)
        {
            Debug.WriteLine("Construct.AdjacentElements for position:" + position.ToString());
            Dictionary<Direction, ConstructElement> adjacentElements = new Dictionary<Direction, ConstructElement>();
            Point gridOffset = position + DirectionToPoint(Direction.Up);

            if (buildElements.Keys.Contains<Point>(gridOffset))
            {
                adjacentElements.Add(Direction.Up, buildElements[gridOffset]);
            }
            gridOffset = position + DirectionToPoint(Direction.Down);
            if (buildElements.Keys.Contains<Point>(gridOffset))
            {
                adjacentElements.Add(Direction.Down, buildElements[gridOffset]);
            }
            gridOffset = position + DirectionToPoint(Direction.Left);
            if (buildElements.Keys.Contains<Point>(gridOffset))
            {
                adjacentElements.Add(Direction.Left, buildElements[gridOffset]);
            }
            gridOffset = position + DirectionToPoint(Direction.Right);
            if (buildElements.Keys.Contains<Point>(gridOffset))
            {
                adjacentElements.Add(Direction.Right, buildElements[gridOffset]);
            }
            Debug.WriteLine("Construct.AdjacentElements Adjacent count:" + adjacentElements.Count.ToString());
            return adjacentElements;
        }
        public void AddSegmentAtSensorPosition(Segment segment, Sensor sensor)
        {//Segment will point up until the user picks the orientation 

            AddNewSegmentToConstruct(sensor.constructElement.segment, segment, Sensor.DirectonToPoint(sensor.direction), 0);

            foreach (ConstructElement item in buildElements.Values)
            {
                //ToDo: heavy handed. Maybe an update sensors function would be better
                item.DisableSensors();
                item.EnableSensors();
            }
        }
        protected void AddNewSegmentToConstruct(Segment recievingSegment, Segment newSegment, Point relativeOffset, float rotation)
        {
            //ToDo: rotation is fucked
            Debug.WriteLine("AddNewSegmentToConstruct recievingSegment:" + recievingSegment.constructElement.offSet.ToString());
            Debug.WriteLine("AddNewSegmentToConstruct newSegment offset: " + (recievingSegment.constructElement.offSet + relativeOffset).ToString());
            entities.Add(newSegment);
            Vector2 anchorOffset;
            Joint joint;
            newSegment.body.SetTransform(newSegment.body.Position, newSegment.body.Rotation += rotation);
            //anchorOffset = new Vector2((entityA.constructElement.offSet + relativeOffset).X * 1.2f, (entityA.constructElement.offSet + relativeOffset).Y * -1.2f);
            anchorOffset = new Vector2((relativeOffset).X * 1.2f, (relativeOffset).Y * 1.2f);

            Debug.WriteLine("AddNewSegmentToConstruct anchorOffset:" + anchorOffset.ToString());
            Debug.WriteLine("AddNewSegmentToConstruct new direction:" + Construct.PointToDirection(relativeOffset).ToString());
            if (!buildElements.ContainsKey(relativeOffset + recievingSegment.constructElement.offSet))
            {

                joint = CreateJointBetweenAnchorsOnSegments(recievingSegment, newSegment, Construct.PointToDirection(relativeOffset), anchorOffset);//magic *-1
                newSegment.constructElement.AddToConstruct(this, relativeOffset + recievingSegment.constructElement.offSet, joint);
                recievingSegment.constructElement.branchJoints.Add(joint);
                buildElements.Add(recievingSegment.constructElement.offSet + relativeOffset, newSegment.constructElement);
                newSegment.constructElement.EnableSensors();
                recievingSegment.constructElement.construct = this;
            }
            else
            {

                Debug.WriteLine("AddNewSegmentToConstruct key exists: " + (relativeOffset + recievingSegment.constructElement.offSet).ToString());
            }
        }
        private Joint CreateJointBetweenAnchorsOnSegments(Segment entityA, Segment entityB, Scrap.GameElements.Entities.Direction direction, Vector2 anchorOffset)
        {
            Joint joint;
            //The offset is applied to the first entity in order to allow the second entity to have alternative rotations
            joint = JointFactory.CreateWeldJoint(game.world, entityA.GetJointAnchor(direction), entityB.GetJointAnchor(direction), anchorOffset, new Vector2(0, 0));
            joints.Add(joint);
            return joint;
        }


        public bool TestPlacement(Vector2 point)
        {
            return false;
        }


        public Vector2 Position
        {
            get { return KeyObject.Position; }
        }

        public void SetPosition(Vector2 pos, bool useWorldCoordinates = true)
        {
            if (useWorldCoordinates)
                pos -= KeyObject.Position;

            foreach (Segment current in entities)
            {
                current.Position += pos;
            }
        }

        public void Rotate(float rot)
        {
            foreach (Segment current in entities)
            {
                current.Rotation += rot;
                if (current != KeyObject)
                {
                    current.Position -= KeyObject.Position;
                    float cos = (float)Math.Cos(rot);
                    float sin = (float)Math.Sin(rot);
                    Vector2 rotationVector = current.Position;
                    rotationVector = new Vector2(current.Position.X * cos - current.Position.Y * sin, current.Position.X * sin + current.Position.Y * cos);
                    current.Position = rotationVector + KeyObject.Position;
                }
            }
        }

        public void Rotate(float rot, Vector2 pos, bool useWorldCoordinates = true)
        {//TODO: test this function (I think it has bugs)
            foreach (Segment current in entities)
            {
                current.Rotation += rot;
                if(!useWorldCoordinates)
                {
                    pos += KeyObject.Position;
                }
                current.Position -= pos;
                float cos = (float)Math.Cos(rot);
                float sin = (float)Math.Sin(rot);
                Vector2 rotationVector = current.Position;
                rotationVector = new Vector2(current.Position.X * cos - current.Position.Y * sin, current.Position.X * sin + current.Position.Y * cos);
                current.Position = rotationVector + pos;
            }
        }
        public void FreeObject(Segment entity)
        {//ToDo: This function is rubbish. Rename and fix. Should remove an object from the construct. Might be dublicate. 
            //Check how objct is fried when dragging away from construct
            entity.constructElement.BreakBranch();
            entity.constructElement.BreakRoot();
            this.buildElements.Remove(entity.constructElement.offSet);
        }

        public void SetRotation(float rot)
        {
            Rotate(rot - KeyObject.Rotation);
        }

        public void SetRotation(float rot, Vector2 pos, bool useWorldCoordinates = true)
        {
            Rotate(rot - KeyObject.Rotation, pos, useWorldCoordinates);
        }
    }
}
