using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap.GameElements.Entities
{
    
    public class BuildElement
    {
        Entity entity;
        Body body;
        public BuildElement(ScrapGame game, Entity entity)
        {
            body = BodyFactory.CreateRectangle(game.world, 1.2f, 1.2f, 0, null);
            body.IsSensor = true;
            body.IsKinematic = false;
            
            this.entity = entity;
           // body.IgnoreGravity = true;
           // body.IsKinematic = true;
            body.Position = entity.Position;
            AddSensors(entity);
            body.FixtureList.Remove(body.FixtureList[0]);
           // JointFactory.CreateWeldJoint(game.world, entity.body, body, new Vector2( 0,0), new Vector2(0, 0));
        }
        private void AddSensors(Entity entity)
        {
            foreach (var direction in entity.JointDirections())
            {
                if (direction == Entity.Direction.Right)
                    FixtureFactory.AttachRectangle(1.2f, 1.2f, 0, Vector2.UnitX * 1.2f, body, direction).IsSensor = true;
                if (direction == Entity.Direction.Left)
                    FixtureFactory.AttachRectangle(1.2f, 1.2f, 0, Vector2.UnitX * -1.2f, body, direction).IsSensor = true;
                if (direction == Entity.Direction.Down)
                    FixtureFactory.AttachRectangle(1.2f, 1.2f, 0, Vector2.UnitY * 1.2f, body, direction).IsSensor = true;
                if (direction == Entity.Direction.Up)
                    FixtureFactory.AttachRectangle(1.2f, 1.2f, 0, Vector2.UnitY * -1.2f, body, direction).IsSensor = true;

            }
        }



    }
    [Serializable]
    public abstract class Construct
    {
        //ToDo: construct should be a super entiy
        //body.CreateFixture() should be used to create one solid object out of shapes while disabling the current bodies or something

        public Entity KeyObject { get; set; }
        
        protected List<Joint> joints;
        protected List<Entity> entities;
        protected List<BuildElement> buildElements;
        protected ScrapGame game;
        public Construct(ScrapGame game)
        {
            this.game = game;
            joints = new List<Joint>();
            entities = new List<Entity>();
            buildElements = new List<BuildElement>();
            game.constructList.Add(this);


        }
        public virtual void Update(GameTime gameTime)
        {

        }

        protected void JoinEntities(Entity entityA, Entity entityB, Scrap.GameElements.Entities.Entity.Direction direction){
            if (entityA.Container != this)//ToDo: review
            {
                entityA.Container = this;
                entities.Add(entityA);
                buildElements.Add(new BuildElement(this.game, entityA));
            }
            if (entityB.Container != this)
            {
                entityB.Container = this;
                entities.Add(entityB);
                buildElements.Add(new BuildElement(this.game, entityB));
            }
            

            switch (direction)
            {
                case Entity.Direction.Right:
                    joints.Add(JointFactory.CreateWeldJoint(game.world, entityA.GetJointAnchor(direction), entityB.GetJointAnchor(direction), new Vector2( 1.2f,0), new Vector2(0, 0)));
                    break;
                case Entity.Direction.Left:
                    joints.Add(JointFactory.CreateWeldJoint(game.world, entityA.GetJointAnchor(direction), entityB.GetJointAnchor(direction), new Vector2(0, 0), new Vector2( 1.2f,0)));
                    break;
                case Entity.Direction.Up:
                    joints.Add(JointFactory.CreateWeldJoint(game.world, entityB.GetJointAnchor(direction), entityA.GetJointAnchor(direction), new Vector2(0, 0), new Vector2(0, 1.2f)));
                    break;
                case Entity.Direction.Down://ToDo: possible bug
                    joints.Add(JointFactory.CreateWeldJoint(game.world, entityB.GetJointAnchor(direction), entityA.GetJointAnchor(direction), new Vector2(0,0), new Vector2(0,1.2f)));
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

            foreach (Entity current in entities)
            {
                current.Position += pos;
            }
        }

        public void Rotate(float rot)
        {
            foreach (Entity current in entities)
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
            foreach (Entity current in entities)
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
        public void FreeObject(Entity entity)
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
