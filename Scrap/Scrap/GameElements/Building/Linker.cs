using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Scrap.GameElements.Entities;
using Scrap.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap.GameElements.Building
{
    class Linker
    {
        public Sprite sprite;
        protected ScrapGame game;
        protected Joint joint;
        protected Vector2 position;
        
        public ConstructElement constructElement;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Linker(ScrapGame game, Joint joint)
        {
            this.joint = joint;
            this.game = game;
            sprite = new Rendering.Sprite(game.Content.Load<Texture2D>("Linker"), 0, false,.3f);
        }
        public virtual void Update()
        {
            position = joint.BodyA.Position - joint.BodyB.Position;
            position = joint.BodyB.Position + position * .5f;
        }
        
        public virtual void Draw(SpriteBatch batch)
        {
            sprite.Draw(batch, position, 0f, Color.White);
        }
    }
}
