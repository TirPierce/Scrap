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
        
        private Segment keyObject;

        public Segment KeyObject
        {
            get { return keyObject; }
            set 
            {
                keyObject = value;
                keyObject.constructElement = new ConstructElement(this.game, keyObject, this, Point.Zero, null);
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
        public void SetSegmentLock(Point offset, Segment segment)
        {
 
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
        public void JoinEntities(Segment entityA, Segment entityB, Scrap.GameElements.Entities.Direction direction)
        {
            
            entities.Add(entityB);
            Point gridOffset = entityA.constructElement.offSet;
            Vector2 anchorOffset;
            Joint joint;
            
            switch (direction)
            {
                case Direction.Right:
                    gridOffset += new Point(1, 0);
                    anchorOffset=new Vector2(-1.2f, 0);
                    
                    break;
                case Direction.Left:
                    gridOffset += new Point(-1, 0);
                    anchorOffset= new Vector2(1.2f, 0);
                    break;
                case Direction.Up:
                    gridOffset += new Point(0, 1);
                    anchorOffset= new Vector2(0, 1.2f);
                    break;
                case Direction.Down:
                    gridOffset += new Point(0, -1);
                    anchorOffset= new Vector2(0, -1.2f);
                    break;
                default:
                    gridOffset += new Point(0, 1);
                    anchorOffset=new Vector2(0, -1.2f);
                    break;
            }
            if (!buildElements.ContainsKey(gridOffset))
            {
                joint = AddJoint(entityA, entityB, direction, anchorOffset);
                entityB.constructElement = new ConstructElement(this.game, entityB, this, gridOffset, joint);
                entityA.constructElement.branchJoints.Add(joint);
                buildElements.Add(gridOffset, entityB.constructElement);
                entityB.AddSensors();
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
