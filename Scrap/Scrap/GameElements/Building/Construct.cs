using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Scrap.GameElements.GameWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap.GameElements.Entities
{

    

    [Serializable]
    public abstract class Construct
    {
        public static Point DirectionToPoint(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    return new Point(-1, 0);
                case Direction.Right:
                    return new Point(1, 0);
                case Direction.Up:
                    return new Point(0, 1);
                case Direction.Down:
                    return new Point(0, -1);
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
        private Segment keyObject;

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
        
        protected List<Joint> joints;
        protected List<Segment> entities;

        public Dictionary<Point, ConstructElement> buildElements;
        
        protected ScrapGame game;
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
            Dictionary<Direction, ConstructElement> adjacentElements = new Dictionary<Direction, ConstructElement>();
            Point gridOffset = position + DirectionToPoint(Direction.Up);

            if (buildElements.Keys.Contains<Point>(gridOffset))
            {
                adjacentElements.Add(Direction.Down, buildElements[gridOffset]);
            }
            gridOffset = position + DirectionToPoint(Direction.Down);
            if (buildElements.Keys.Contains<Point>(gridOffset))
            {
                adjacentElements.Add(Direction.Up, buildElements[gridOffset]);
            }
            gridOffset = position + DirectionToPoint(Direction.Left);
            if (buildElements.Keys.Contains<Point>(gridOffset))
            {
                adjacentElements.Add(Direction.Right, buildElements[gridOffset]);
            }
            gridOffset = position + DirectionToPoint(Direction.Right);
            if (buildElements.Keys.Contains<Point>(gridOffset))
            {
                adjacentElements.Add(Direction.Left, buildElements[gridOffset]);
            }
            return adjacentElements;
        }
        public void AddSegment(Segment segment, Point offset)
        {

            foreach (ConstructElement item in AdjacentElements(offset).Values)
            {
                JoinEntities(segment, item.segment, offset, 0);
                break;
            }
            

        }
        public void AddSegment(Segment segment, Point offset, Direction direction)
        {

        }


        protected void JoinEntities(Segment entityA, Segment entityB, Point newGridOffset, float rotation)
        {

            entities.Add(entityB);
            Vector2 anchorOffset;
            Joint joint;
            entityB.body.SetTransform(entityB.body.Position, entityB.body.Rotation += rotation);
            anchorOffset = new Vector2((entityA.constructElement.offSet - newGridOffset).X * -1.2f, (entityA.constructElement.offSet - newGridOffset).Y * -1.2f);
            
            if (!buildElements.ContainsKey(newGridOffset))
            {
                joint = AddJoint(entityA, entityB, Construct.PointToDirection(newGridOffset-entityA.constructElement.offSet), anchorOffset);
                entityB.constructElement.AddToConstruct(this, newGridOffset, joint);
                entityA.constructElement.branchJoints.Add(joint);
                buildElements.Add(newGridOffset, entityB.constructElement);
                entityB.constructElement.EnableSensors();
                //entityA.constructElement.l
            }
        }
        private Joint AddJoint(Segment entityA, Segment entityB, Scrap.GameElements.Entities.Direction direction, Vector2 anchorOffset)
        {
            Joint joint;
            
            joint = JointFactory.CreateWeldJoint(game.world, entityA.GetJointAnchor(direction), entityB.GetJointAnchor(direction), new Vector2(0, 0), anchorOffset);
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
        {

            foreach (Joint joint in joints)
            {
                game.world.RemoveJoint(joint);
            }
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
