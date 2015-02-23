using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using Box2D.XNA;
//using Scrap.Physics;
using System.Collections.Generic;

namespace Scrap.Entities 
{

    public abstract class Entity : Microsoft.Xna.Framework.GameComponent//, IPhysicsObject
    {
        public Point gridPosition; 
        protected Texture2D mTexture;
        protected Texture2D mBackground;
        public Vector2 mPosition;
        //public Body physicsBody;
        protected float rotation;
        protected float density;
        protected float restitution;
        protected float friction;
        public string objectType;
        
    
        //protected AliensPhysics.Physics.Shape shape;
        //protected BodyType type = BodyType.Dynamic;

        //public Body PhysicsBody
        //{
        //    get { return physicsBody; }
        //    set { physicsBody = value;}
        //}
        public Vector2 Position { get { return mPosition; } }
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public Entity(Game game)
            :base(game)
        {
            gridPosition = new Point(0, 0);
            
        }

        public virtual void Initialize()
        {
        }

        public virtual void Update(GameTime gameTime)
        {
            //mPosition = physicsBody.GetWorldCenter();
            //rotation = physicsBody.Rotation;
            
            base.Update(gameTime);
        }
        public virtual void LoadContent()
        { }

        public float Density
        {
            get { return density; }

        }
        public float Restitution
        {
            get { return restitution; }

        }
        public float Friction
        {
            get { return friction; }

        }
        //public BodyType Type
        //{
        //    get { return type; }

        //}
        //public AliensPhysics.Physics.Shape Shape
        //{
        //    get { return shape; }

        //}

        public void Draw(SpriteBatch batch)
        {
            //if (physicsBody == null)
            //{
            //    batch.Draw(mTexture, new Rectangle((int)mPosition.X, (int)mPosition.Y, 100, -100), Color.White);
            //}
            //else 
            //{
           // batch.Draw(mTexture, new Rectangle((int)physicsBody.GetWorldCenter().X, (int)physicsBody.GetWorldCenter().Y, 1, 1), null, Color.White, physicsBody.Rotation, Vector2.Zero, SpriteEffects.None, 0);
            if (mBackground != null)
            {
                //batch.Draw(mBackground, physicsBody.GetWorldCenter(), null, Color.White, physicsBody.Rotation, new Vector2(mTexture.Width / 2f, mTexture.Height / 2f), .01f * (100f / (float)mTexture.Width), SpriteEffects.None, 0);
            }
            //batch.Draw(mTexture, physicsBody.GetWorldCenter(), null,Color.White, physicsBody.Rotation, new Vector2(mTexture.Width / 2f, mTexture.Height / 2f), .01f * (100f / (float)mTexture.Width), SpriteEffects.None, 0);
            //}
            
        }
        public virtual void DrawUI(SpriteBatch batch, Vector2 worldPos, float rotation)
        {
            batch.Draw(mTexture, worldPos, null, Color.White, rotation, new Vector2(mTexture.Width / 2f, mTexture.Height / 2f), .01f * (100f / (float)mTexture.Width), SpriteEffects.None, 0);
        }
        
        //public bool Picked(Microsoft.Xna.Framework.Vector2 worldPos)
        //{
        //    return physicsBody.GetFixtureList().TestPoint(worldPos);//because each game entity is only one fixture

        //}
    }
}
