using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Scrap.UserInterface;
using Scrap.Entities;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using GameStateManagement;
using System;


namespace Scrap.GameState
{


    public class PlayerMatch : Microsoft.Xna.Framework.GameComponent
    {
        Invention playerCreation;
        GameWorld.Terrain mTerrain;
        JetEngine jet, jet2, jet3, jet4;

        Hub hub;
        Girder girder, girder2, girder3, girder4;
        FreeWheel wheel, wheel2, wheel3, wheel4;
        Camera mCamera;
        //Physics.PhysicsEngineMain mPhysics;

        Texture2D goalTexture;
        Texture2D mObjectArrow;
        Rectangle goalRectangle;
        SoundEffectInstance tension;
        SoundEffectInstance eerie;
        public List<Entity> worldEntityList;
        KeyboardState keyLast;
        internal Camera MCamera
        {
            get { return mCamera; }
            set { mCamera = value; }
        }
        
        public PlayerMatch(Game game)
            : base(game)
        {

            mTerrain = new GameWorld.Terrain(game);
            mCamera = new Camera(game);
            //mPhysics = new Physics.PhysicsEngineMain(game);
            jet = new JetEngine(game);
            wheel = new FreeWheel(game);
            hub = new Hub(game);
            girder = new Girder(game);
            girder2 = new Girder(game);
            girder3 = new Girder(game);
            girder4 = new Girder(game);
            jet2 = new JetEngine(game);
            jet3 = new JetEngine(game);
            jet4 = new JetEngine(game);
            wheel2 = new FreeWheel(game);
            wheel3 = new FreeWheel(game); 
            wheel4 = new FreeWheel(game);

            playerCreation = new Invention(game);
            worldEntityList = new List<Entity>();
            playerCreation.worldEntityList = worldEntityList;
            Accelerometer.Initialize();
            
            
        }

        public override void Initialize()
        {
            goalRectangle = new Rectangle(105, 12, 5, 5);
            //mPhysics.Initialize();
            //mTerrain.CreateGround(mPhysics.world);
            jet.Initialize(new Vector2(100, 7));
            jet.gridPosition = new Point(1, 3);

            jet2.Initialize(new Vector2(102, 7));
            jet2.gridPosition = new Point(3, 3);

            jet3.Initialize(new Vector2(100, 8));
            jet3.gridPosition = new Point(0, 3);

            jet4.Initialize(new Vector2(102, 8));
            jet4.gridPosition = new Point(4, 3);
            //jet2.Rotation = 50;
            hub.Initialize(new Vector2(101, 7));
            hub.gridPosition = new Point(2, 3);

            girder.Initialize(new Vector2(101, 7));
            girder2.Initialize(new Vector2(73, 4));
            girder3.Initialize(new Vector2(74, 4));
            girder4.Initialize(new Vector2(73, 4));
            wheel.Initialize(new Vector2(70, 3));
            wheel2.Initialize(new Vector2(90, 2));
            wheel3.Initialize(new Vector2(100, 3)); 
            wheel4.Initialize(new Vector2(120, 2)); 
            
            //mPhysics.RegisterObject(jet);
            //mPhysics.RegisterObject(jet2);
            //mPhysics.RegisterObject(jet3);
            //mPhysics.RegisterObject(jet4);

            //mPhysics.RegisterObject(hub);
            //mPhysics.RegisterObject(girder);
            //mPhysics.RegisterObject(girder2);
            //mPhysics.RegisterObject(girder3);
            //mPhysics.RegisterObject(girder4);
            //mPhysics.RegisterObject(wheel);
            ////wheel2, wheel3, wheel4, wheel5, wheel6, wheel7, wheel8;
            //mPhysics.RegisterObject(wheel2);
            //mPhysics.RegisterObject(wheel3);
            //mPhysics.RegisterObject(wheel4);


            //mPhysics.RegisterJoint(jet, jet3, new Vector2(0, 0));
            //mPhysics.RegisterJoint(jet2, jet4, new Vector2(0, 0));
            //mPhysics.RegisterJoint(jet, hub, new Vector2(0, 0));
            //mPhysics.RegisterJoint(hub, jet2, new Vector2(0, 0));
            worldEntityList.Add(jet);
            worldEntityList.Add(jet2);
            worldEntityList.Add(jet3);
            worldEntityList.Add(jet4);

            worldEntityList.Add(hub);
            worldEntityList.Add(girder);
            worldEntityList.Add(girder2);
            worldEntityList.Add(girder3);
            worldEntityList.Add(girder4);

            worldEntityList.Add(wheel);
            worldEntityList.Add(wheel2);
            worldEntityList.Add(wheel3);
            worldEntityList.Add(wheel4);

            

            playerCreation.entityList.Add(jet);
            playerCreation.entityList.Add(jet2);
            playerCreation.entityList.Add(jet3);
            playerCreation.entityList.Add(jet4);
            playerCreation.entityList.Add(hub);
            //mPhysics.contactCallback.invention= playerCreation;
            base.Initialize();
        }
        public void LoadContent()
        {
            jet.LoadContent();
            jet2.LoadContent();
            jet3.LoadContent();
            jet4.LoadContent();
            hub.LoadContent();
            girder.LoadContent();
            girder2.LoadContent();
            girder3.LoadContent();
            girder4.LoadContent();
            wheel.LoadContent();
            wheel2.LoadContent();
            wheel3.LoadContent();
            wheel4.LoadContent();

            playerCreation.LoadContent(Game.Content);

            goalTexture = Game.Content.Load<Texture2D>("Goal");
            

            mCamera.SetBackground(Game.Content.Load<Texture2D>("Background/nearback"), Game.Content.Load<Texture2D>("Background/sky"));
           
            //tension = Game.Content.Load<SoundEffect>("Sound/tension").CreateInstance();
            //eerie = Game.Content.Load<SoundEffect>("Sound/tension").CreateInstance();

            mObjectArrow = Game.Content.Load<Texture2D>("arrow");
            //tension.IsLooped = true;
            //eerie.IsLooped = true;

            //tension.Play();
            //eerie.Play();
            mTerrain.LoadContent();
            
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Begin(SpriteSortMode.Immediate,
                        BlendState.AlphaBlend,
                        null,
                        null,
                        null,
                        null,
                        mCamera.Transformation);

            mCamera.DrawBackground(batch);

            

            mTerrain.Draw(batch);
            playerCreation.Draw(batch);
            
            jet.Draw(batch);
            jet2.Draw(batch);
            jet3.Draw(batch);
            jet4.Draw(batch);
            wheel.Draw(batch);

            wheel2.Draw(batch);
            wheel3.Draw(batch);
            wheel4.Draw(batch);


            hub.Draw(batch);
            girder.Draw(batch);
            girder2.Draw(batch);
            girder3.Draw(batch);
            girder4.Draw(batch);

            //Vector2 difference =new Vector2(goalRectangle.Center.X, goalRectangle.Center.Y)- hub.PhysicsBody.GetWorldCenter()  ;
            //float rot = (float)Math.Atan2(difference.Y, difference.X);
            //batch.Draw(mObjectArrow,new Vector2(hub.PhysicsBody.GetWorldCenter().X ,hub.PhysicsBody.GetWorldCenter().Y-5), null,
            //    Color.White,
            //    rot,
            //    new Vector2(mObjectArrow.Width / 2, mObjectArrow.Height / 2), .003f, SpriteEffects.None, 0);

            batch.Draw(goalTexture, goalRectangle, Color.White);
            batch.End();

        }


        public override void Update(GameTime gameTime)
        {


            mTerrain.Update(mCamera);
            jet.Update(gameTime);
            jet2.Update(gameTime);
            jet3.Update(gameTime);
            jet4.Update(gameTime);
            wheel.Update(gameTime);
            wheel2.Update(gameTime);
            wheel3.Update(gameTime);
            wheel4.Update(gameTime);

            hub.Update(gameTime);

            girder.Update(gameTime);
            girder2.Update(gameTime);
            girder3.Update(gameTime);
            girder4.Update(gameTime);




            GamePadState state = GamePad.GetState(0);
            KeyboardState keyState = Keyboard.GetState();

            /*if (state.Buttons.Y == ButtonState.Pressed || (keyState.IsKeyUp(Keys.G) && keyLast.IsKeyDown(Keys.G)))
            {
                playerCreation.Edit();
            }*/
            
            //state = Accelerometer.GetState();
            if (state.IsButtonDown(Buttons.RightTrigger))
                jet2.OnAnalogueIn((int)(state.Triggers.Right * 100));
            else if (keyState.IsKeyDown(Keys.A))
                jet2.OnDigitalIn(true);

            if (state.IsButtonDown(Buttons.LeftTrigger))
                jet.OnAnalogueIn((int)(state.Triggers.Left * 100));
            else if (keyState.IsKeyDown(Keys.S))
                jet.OnDigitalIn(true);

            if (state.IsButtonDown(Buttons.RightShoulder) || keyState.IsKeyDown(Keys.D))
                jet3.OnDigitalIn(true);


            if (state.IsButtonDown(Buttons.LeftShoulder) || keyState.IsKeyDown(Keys.F))
                jet4.OnDigitalIn(true);
            
            
                
                

            
            //mCamera.Position = gman.mPosition;
            mCamera.Position = new Vector2(hub.mPosition.X,hub.mPosition.Y+2);
            //mCamera.Rotation = gman.Rotation;

            ((GUI)(this.Game.Services.GetService(typeof(IGUI)))).SetObjectiveLocation(new Vector2(goalRectangle.Center.X, goalRectangle.Center.Y));


            mCamera.Update();
            //mPhysics.Update(gameTime);
            int win;
            //if (goalRectangle.Contains(new Point((int)hub.PhysicsBody.GetWorldCenter().X,(int)hub.PhysicsBody.GetWorldCenter().Y)))
            //{
            //    win = 0;
            //}
            //if (((GUI)(this.Game.Services.GetService(typeof(IGUI)))).mGameState == GameStateManagement.GamePlayState.BuildMode)
            //{
            //    keyLast = new KeyboardState();
            //}
            //else
            //    keyLast = keyState;
            base.Update(gameTime);
        }
    }
}
