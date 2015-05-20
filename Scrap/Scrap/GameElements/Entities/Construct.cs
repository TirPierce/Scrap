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
    [Serializable]
    public abstract class Construct
    {
        //ToDo: construct should be a super entiy
        //body.CreateFixture() should be used to create one solid object out of shapes while disabling the current bodies or something

        public Entity KeyObject { get; set; }

        protected List<Joint> joints;
        protected List<Entity> entities;
        protected ScrapGame game;
        public Construct(ScrapGame game)
        {
            this.game = game;
            joints = new List<Joint>();
            entities = new List<Entity>();
            game.constructList.Add(this);
        }
        public virtual void Update(GameTime gameTime)
        {
        }
        protected void JoinEntities(Entity entityA, Entity entityB, Scrap.GameElements.Entities.Entity.Direction direction){
            if (entityA.Container != this)
            {
                entityA.Container = this;
                entities.Add(entityA);
            }
            if (entityB.Container != this)
            {
                entityB.Container = this;
                entities.Add(entityB);
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
                case Entity.Direction.Down:
                    joints.Add(JointFactory.CreateWeldJoint(game.world, entityB.GetJointAnchor(direction), entityA.GetJointAnchor(direction), new Vector2(0,0), new Vector2(0,1.2f)));
                    break;
                default:
                    break;
            }
            
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
