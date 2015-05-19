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
    }
}
