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
    public class WheelEngine : Entity, IAutomotive
    {



        public WheelEngine(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            mPosition.X = 0;
            mPosition.Y = 0;
            //shape = AliensPhysics.Physics.Shape.Circle;
            density = 4;
            restitution = .05f;
            friction = .9f;
            objectType = "motorwheel";
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

            mTexture = Game.Content.Load<Texture2D>("GameObjects/wheel");



        }
        public void OnAnalogueIn(int input)
        {
            //PhysicsBody.ApplyTorque(input * 50f);
            
           
        }
        public void OnDigitalIn(bool input)
        {

        }


    }
}