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
    public class FreeWheel : Entity
    {



        public FreeWheel(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            mPosition.X = 0;
            mPosition.Y =0;
            density = 1;
            restitution = .5f;
            friction = .5f;
            objectType = "wheel";


            //type = BodyType.Dynamic;
            //shape = AliensPhysics.Physics.Shape.Circle;



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

            mTexture = Game.Content.Load<Texture2D>("assets/wheel");
           

        }



    }
}
