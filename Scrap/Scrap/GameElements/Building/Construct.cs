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
        public void RecalculateAdjacentSegmentsAndActivateSensors()
        {
            foreach (ConstructElement item in buildElements.Values)
            {
                item.EnableSensors();
            }
 
        }
        public void SetSegmentDirection(Segment segment, Direction direction)
        {
            Debug.WriteLine("Construct.SetSegmentDirection Offset: " + segment.constructElement.offSet.ToString() + " Rotation: " + direction.ToString());

            var recievingSegment = this.buildElements[segment.constructElement.adjacentElements.First()].segment;
            var offset = segment.constructElement.offSet - recievingSegment.constructElement.offSet;
            this.FreeObject(segment);
            AddNewSegmentToConstruct(recievingSegment, segment, offset, direction);
        
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
        public List<Point> AdjacentElements(Point position)
        {
            Debug.WriteLine("Construct.AdjacentElements for position:" + position.ToString());
            List<Point> adjacentElements = new List<Point>();
            Point gridOffset = position + Orientation.DirectionToPoint(Direction.Up);

            if (buildElements.Keys.Contains<Point>(gridOffset))
            {
                adjacentElements.Add(gridOffset);
            }
            gridOffset = position + Orientation.DirectionToPoint(Direction.Down);
            if (buildElements.Keys.Contains<Point>(gridOffset))
            {
                adjacentElements.Add(gridOffset);
            }
            gridOffset = position + Orientation.DirectionToPoint(Direction.Left);
            if (buildElements.Keys.Contains<Point>(gridOffset))
            {
                adjacentElements.Add(gridOffset);
            }
            gridOffset = position + Orientation.DirectionToPoint(Direction.Right);
            if (buildElements.Keys.Contains<Point>(gridOffset))
            {
                adjacentElements.Add(gridOffset);
            }
            Debug.WriteLine("Construct.AdjacentElements Adjacent count:" + adjacentElements.Count.ToString());

            return adjacentElements;
        }
        public void AddSegmentAtSensorPosition(Segment segment, Sensor sensor)
        {//Segment will point up until the user picks the orientation 

            AddNewSegmentToConstruct(sensor.constructElement.segment, segment, Orientation.DirectionToPoint(sensor.direction), Direction.Up);
            RecalculateAdjacentSegmentsAndActivateSensors();
        }
        protected void AddNewSegmentToConstruct(Segment recievingSegment, Segment newSegment, Point relativeOffset, Direction direction)
        {
            
            float rotation = Orientation.DirectionToRadians(direction);
            Debug.WriteLine("AddNewSegmentToConstruct recievingSegment:" + recievingSegment.constructElement.offSet.ToString());
            Debug.WriteLine("AddNewSegmentToConstruct newSegment offset: " + (recievingSegment.constructElement.offSet + relativeOffset).ToString());
            entities.Add(newSegment);
            Vector2 anchorOffset;
            Joint joint;
            
            newSegment.body.SetTransform(newSegment.Position, recievingSegment.Rotation + rotation);
            
            anchorOffset = new Vector2((relativeOffset).X * 1.2f, (relativeOffset).Y * 1.2f);

            Debug.WriteLine("AddNewSegmentToConstruct anchorOffset:" + anchorOffset.ToString());
            Debug.WriteLine("AddNewSegmentToConstruct new direction:" + Orientation.PointToDirection(relativeOffset).ToString());
            if (!buildElements.ContainsKey(relativeOffset + recievingSegment.constructElement.offSet))
            {

                joint = CreateJointBetweenAnchorsOnSegments(recievingSegment, newSegment, Orientation.PointToDirection(relativeOffset), anchorOffset);
                newSegment.constructElement.AddToConstruct(this, relativeOffset + recievingSegment.constructElement.offSet, joint, direction);
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
        private Joint CreateJointBetweenAnchorsOnSegments(Segment entityA, Segment entityB, Direction direction, Vector2 anchorOffset)
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
