using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using Scrap.Physics;
//using Box2D.XNA;
using Microsoft.Xna.Framework.Input;


namespace Scrap.Entities
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Hub : Entity//, IPhysicsObject
    {


        public Hub(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            //mPosition.X = 0;
            //mPosition.Y =0;

            density = 1;
            restitution = .1f;
            friction = .01f;
            objectType = "hub";

            //type = BodyType.Dynamic;
            //shape = AliensPhysics.Physics.Shape.Square;


        }

        public void Initialize(Vector2 position)
        {
            mPosition = position;


            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {


            base.Update(gameTime);
        }

        public override void LoadContent()
        {

            mTexture = Game.Content.Load<Texture2D>("assets/control");


        }


        public void DrawUI(Microsoft.Xna.Framework.Graphics.SpriteBatch batch, Microsoft.Xna.Framework.Vector2 worldPos, float rotation)
        {
            batch.Draw(mTexture, worldPos, null, Color.White, rotation, new Vector2(mTexture.Width / 2f, mTexture.Height / 2f), .01f * (100f / (float)mTexture.Width), SpriteEffects.None, 0);
        }



    }
}
