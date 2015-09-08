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
        //ToDo: construct should be a super entiy
        //body.CreateFixture() should be used to create one solid object out of shapes while disabling the current bodies or something

        private Segment keyObject;

        public Segment KeyObject
        {
            get { return keyObject; }
            set 
            {
                keyObject = value;
                keyObject.body.UserData = this;
                entities.Add(keyObject);
                buildElements.Add(new ConstructElement(this.game, keyObject,this)); 
                
            }
        }
        
        protected List<Joint> joints;
        protected List<Segment> entities;
        protected List<ConstructElement> buildElements;
        protected ScrapGame game;
        public Construct(ScrapGame game)
        {
            this.game = game;
            joints = new List<Joint>();
            entities = new List<Segment>();
            buildElements = new List<ConstructElement>();
            game.constructList.Add(this);
        }
        public virtual void Update(GameTime gameTime)
        {
            foreach (var item in buildElements)
            {
                item.Update();
            }
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
        public void JoinEntities(Segment entityA, Segment entityB, Scrap.GameElements.Entities.Segment.Direction direction)
        {
            entityB.Construct = this;
            entities.Add(entityB);
            buildElements.Add(new ConstructElement(this.game, entityB,this));
            
            switch (direction)
            {
                case Segment.Direction.Right:
                    joints.Add(JointFactory.CreateWeldJoint(game.world, entityA.GetJointAnchor(direction), entityB.GetJointAnchor(direction), new Vector2( 0,0), new Vector2(-1.2f, 0)));
                    break;
                case Segment.Direction.Left:
                    joints.Add(JointFactory.CreateWeldJoint(game.world, entityA.GetJointAnchor(direction), entityB.GetJointAnchor(direction), new Vector2(0, 0), new Vector2( 1.2f,0)));
                    break;
                case Segment.Direction.Up:
                    joints.Add(JointFactory.CreateWeldJoint(game.world, entityA.GetJointAnchor(direction), entityB.GetJointAnchor(direction), new Vector2(0, 0), new Vector2(0, 1.2f)));
                    break;
                case Segment.Direction.Down://ToDo: possible bug
                    joints.Add(JointFactory.CreateWeldJoint(game.world, entityA.GetJointAnchor(direction), entityB.GetJointAnchor(direction), new Vector2(0, 0), new Vector2(0, -1.2f)));
                    break;
                default:
                    break;
            }
            
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
                joint.Breakpoint=0;
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
