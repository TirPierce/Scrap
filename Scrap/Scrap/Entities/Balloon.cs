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
    public class Balloon : Entity
    {

        public Balloon(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            mPosition.X = 0;
            mPosition.Y = 0;
            //shape = AliensPhysics.Physics.Shape.Circle;
            density = 4;
            restitution = .05f;
            friction = .5f;
            objectType = "balloon";
        }

        public void Initialize(Vector2 position)
        {
            mPosition = position;
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            //if(!PhysicsEngineMain.pause)
            //    PhysicsBody.ApplyForce(-(PhysicsBody.GetMass()*PhysicsBody.GetWorld().Gravity)+ new Vector2(0, -40f), PhysicsBody.GetWorldCenter());//16


            base.Update(gameTime);
        }

        public override void LoadContent()
        {
            mBackground = Game.Content.Load<Texture2D>("assets/rocketBack");
            mTexture = Game.Content.Load<Texture2D>("assets/balloon");


        }

    }
}