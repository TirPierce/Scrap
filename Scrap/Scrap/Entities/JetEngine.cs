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
    public class JetEngine : Entity, IAutomotive
    {

        Texture2D mTextureHot;
        Texture2D mTextureCold;
        bool isThrust;
        Vector2 JetMaxPower;

        
         




        public JetEngine(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            mPosition.X = 0;
            mPosition.Y = 0;
            //shape = AliensPhysics.Physics.Shape.Square;
            density = 2;//4
            restitution = .05f;
            friction = .5f;
            objectType = "jet";
        }

        public void Initialize(Vector2 position)
        {
            mPosition = position;

            JetMaxPower = new Vector2(0, -200);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {

            mTexture = mTextureCold; 
            base.Update(gameTime);
        }

        public override void LoadContent()
        {
            mBackground = Game.Content.Load<Texture2D>("assets/rocketBack.png");
            mTextureCold = Game.Content.Load<Texture2D>("assets/rocketFront");
            mTextureHot = Game.Content.Load<Texture2D>("assets/rocketFrontOn");
            mTexture = mTextureCold;

        }
        Vector2 thrust;
        private void ApplyThrust()
        {
            thrust = new Vector2(0, -2);
            //thrust = Vector2.Transform(thrust, Quaternion.CreateFromAxisAngle(new Vector3(0, 0, 1), physicsBody.Rotation));

            //PhysicsBody.ApplyLinearImpulse(thrust, PhysicsBody.GetWorldCenter());
            
            mTexture = mTextureHot;
        }
        private void ApplyThrust(int input)
        {
            thrust = new Vector2(0, -2*input);
            //thrust = Vector2.Transform(thrust, Quaternion.CreateFromAxisAngle(new Vector3(0, 0, 1), physicsBody.Rotation));

            //PhysicsBody.ApplyForce(thrust, PhysicsBody.GetWorldCenter());
            mTexture = mTextureHot;
        }



        public void OnAnalogueIn(int input)
        {
            if (input > 0)
            {
                 ApplyThrust(input);
            }
           
        }
        public void OnDigitalIn(bool input)
        {
            ApplyThrust();
        }
        public override void DrawUI(SpriteBatch batch, Vector2 worldPos, float rotation)
        {
            base.DrawUI(batch, worldPos, 1f);
        }

        

    }
}