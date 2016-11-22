using FarseerPhysics.Dynamics.Joints;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Scrap.GameElements.Entities;
using Scrap.Rendering;

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
        void OnJointBreak(Joint joint, float force)
        {
            (joint.BodyB.FixtureList[0].UserData as Segment).constructElement.RemoveFromConstruct();
        }
        public Linker(ScrapGame game, Joint joint)
        {
            this.joint = joint;
            this.game = game;
            sprite = new Rendering.Sprite(game.Content.Load<Texture2D>("Linker"),32, 32, .3f);
            joint.Breakpoint = 500f;
            joint.Broke += OnJointBreak;
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
