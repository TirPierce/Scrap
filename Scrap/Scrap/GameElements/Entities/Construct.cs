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
        }
        public virtual void Update(GameTime gameTime)
        {
        }
    }
}
