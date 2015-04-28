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


    public class TerrainPlayerMatch : Microsoft.Xna.Framework.GameComponent
    {
        UIManager uiManager;
        Invention invention;
        GameWorld.TerrainAdv mTerrain;
        JetEngine jet, jet2, jet3, jet4;

        Hub hub;
        Girder girder, girder2, girder3, girder4;
        FreeWheel wheel, wheel2, wheel3, wheel4;
        Balloon balloon1, balloon2, balloon3, balloon4;
        WheelEngine wheelEngine1, wheelEngine2;
        public static Camera mCamera;
        //Physics.PhysicsEngineMain mPhysics;

        Texture2D goalTexture;
        Texture2D mObjectArrow;
        Rectangle goalRectangle;
        SoundEffectInstance tension;
        SoundEffectInstance eerie;
        public List<Entity> worldEntityList;
        KeyboardState keyLast;
        Effect desaturate;
        internal Camera MCamera
        {
            get { return mCamera; }
            set { mCamera = value; }
        }

        public TerrainPlayerMatch(Game game)
            : base(game)
        {
           
            mTerrain = new GameWorld.TerrainAdv(game);
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
            balloon1 = new Balloon(game);
            balloon2 = new Balloon(game);
            balloon3 = new Balloon(game);
            balloon4 = new Balloon(game);

            wheelEngine1 = new WheelEngine(game);
            wheelEngine2 = new WheelEngine(game);

            invention = new Invention(game);
            worldEntityList = new List<Entity>();
            invention.worldEntityList = worldEntityList;
            Accelerometer.Initialize();
            
            
        }

        public override void Initialize()
        {
            goalRectangle = new Rectangle(105, 12, 5, 5);
            //mPhysics.Initialize();
            invention.Initialize();
            //mTerrain.CreateGround(mPhysics.world);

            jet.Initialize(new Vector2(100, 38));
            invention.grid[0, 2] = jet;
            jet.gridPosition = new Point(0, 2);

            jet2.Initialize(new Vector2(101, 38));
            invention.grid[1, 2] = jet2;
            jet2.gridPosition = new Point(1, 2);

            jet3.Initialize(new Vector2(103, 38));
            invention.grid[3, 2] = jet3;
            jet3.gridPosition = new Point(3, 2);

            jet4.Initialize(new Vector2(104, 38));
            invention.grid[4, 2] = jet4;
            jet4.gridPosition = new Point(4, 2);

            //jet2.Rotation = 50;
            hub.Initialize(new Vector2(102, 38));
            invention.grid[2, 2] = hub;
            hub.gridPosition = new Point(2, 2);

            girder.Initialize(new Vector2(101, 7));
            girder2.Initialize(new Vector2(73, 4));
            girder3.Initialize(new Vector2(74, 4));
            girder4.Initialize(new Vector2(73, 4));
            wheel.Initialize(new Vector2(70, 3));
            wheel2.Initialize(new Vector2(90, 2));
            wheel3.Initialize(new Vector2(100, 3)); 
            wheel4.Initialize(new Vector2(120, 2));

            balloon1.Initialize(new Vector2(140, 60));
            balloon2.Initialize(new Vector2(130, 60));
            balloon3.Initialize(new Vector2(135, 60));
            balloon4.Initialize(new Vector2(145, 60));

            wheelEngine1.Initialize(new Vector2(140, 62));
            wheelEngine2.Initialize(new Vector2(135, 62));

            //mPhysics.RegisterObject(wheelEngine1);
            //mPhysics.RegisterObject(wheelEngine2);
            //mPhysics.RegisterObject(balloon1);
            //mPhysics.RegisterObject(balloon2);
            //mPhysics.RegisterObject(balloon4);
            //mPhysics.RegisterObject(balloon3);
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

            worldEntityList.Add(balloon1);
            worldEntityList.Add(balloon2);
            worldEntityList.Add(balloon3);
            worldEntityList.Add(balloon4);

            worldEntityList.Add(wheelEngine1);
            worldEntityList.Add(wheelEngine2);
            

            invention.entityList.Add(jet);
            invention.entityList.Add(jet2);
            invention.entityList.Add(jet3);
            invention.entityList.Add(jet4);
            invention.entityList.Add(hub);
            //mPhysics.contactCallback.invention= invention;

            /*
            playerCreation.sensors.Position = hub.physicsBody.Position;
            Box2D.XNA.WeldJointDef weldjoint = new Box2D.XNA.WeldJointDef();
            weldjoint.Initialize(hub.physicsBody, playerCreation.sensors, hub.physicsBody.GetWorldCenter());
            weldjoint.collideConnected = false;
            mPhysics.world.CreateJoint(weldjoint);
            */
            base.Initialize();
        }
        public void LoadContent()
        {
            foreach (Entity ent in worldEntityList)
            {
 
            }
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
            balloon1.LoadContent();
            balloon2.LoadContent();
            balloon3.LoadContent();
            balloon4.LoadContent();

            wheelEngine1.LoadContent();
            wheelEngine2.LoadContent();

            invention.LoadContent(Game.Content);


            //desaturate = Game.Content.Load<Effect>("Effects/desaturate");
            goalTexture = Game.Content.Load<Texture2D>("Goal");
            

            mCamera.SetBackground(Game.Content.Load<Texture2D>("Background/nearback"), Game.Content.Load<Texture2D>("Day_Night"));
            uiManager = new UIManager(worldEntityList, invention, mCamera);//, mPhysics);
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
            
            mTerrain.RenderTerrain();
            Game.GraphicsDevice.Clear(Color.Blue);

            batch.Begin();

            mCamera.DrawBackground(batch);
            mTerrain.Draw(batch);
            batch.End();

            batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, desaturate, mCamera.Transformation);
            foreach (Entity ent in worldEntityList)
            {
                if (Math.Abs(ent.Position.X - mCamera.Position.X) > 5)
                    ent.Draw(batch);
            }
            batch.End();

            batch.Begin(SpriteSortMode.Immediate,
                        BlendState.AlphaBlend,
                        null,
                        null,
                        null,
                        null,
                        mCamera.Transformation);

            //mCamera.DrawBackground(batch);

            
            invention.Draw(batch);
            uiManager.Draw(batch);
            foreach (Entity ent in worldEntityList)
            {
                if(Math.Abs(ent.Position.X-mCamera.Position.X)<5)
                    ent.Draw(batch);
            }
            
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

            uiManager.Update();
            mTerrain.Update();
            foreach (Entity ent in worldEntityList)
            {
                ent.Update(gameTime);
            }
            

            GamePadState state = GamePad.GetState(0);
            KeyboardState keyState = Keyboard.GetState();

            /*if (state.Buttons.Y == ButtonState.Pressed || (keyState.IsKeyUp(Keys.G) && keyLast.IsKeyDown(Keys.G)))
            {
                invention.Edit();
            }*/
            
            //state = Accelerometer.GetState();
            if (state.IsButtonDown(Buttons.RightTrigger))
                jet2.OnAnalogueIn((int)(state.Triggers.Right * 100));
            else if (keyState.IsKeyDown(Keys.S))
                jet2.OnDigitalIn(true);

            if (state.IsButtonDown(Buttons.LeftTrigger))
                jet.OnAnalogueIn((int)(state.Triggers.Left * 100));
            else if (keyState.IsKeyDown(Keys.A))
                jet.OnDigitalIn(true);

            if (state.IsButtonDown(Buttons.RightShoulder) || keyState.IsKeyDown(Keys.D))
                jet3.OnDigitalIn(true);


            if (state.IsButtonDown(Buttons.LeftShoulder) || keyState.IsKeyDown(Keys.F))
                jet4.OnDigitalIn(true);


            if (keyState.IsKeyDown(Keys.E))
            {
                wheelEngine1.OnAnalogueIn(-1);
                wheelEngine2.OnAnalogueIn(-1);
            }
            if (keyState.IsKeyDown(Keys.R))
            {
                wheelEngine1.OnAnalogueIn(1);
                wheelEngine2.OnAnalogueIn(1);
            }
            
            //mCamera.Position = gman.mPosition;
            //mCamera.Position = new Vector2(hub.mPosition.X,hub.mPosition.Y);
            //mCamera.hubWorld = hub.PhysicsBody.GetWorldCenter(); 
            //mCamera.Rotation = gman.Rotation;

            //((GUI)(this.Game.Services.GetService(typeof(IGUI)))).SetObjectiveLocation(new Vector2(goalRectangle.Center.X, goalRectangle.Center.Y));



            mCamera.Update();
            /*
            if (Mouse.GetState().LeftButton == ButtonState.Pressed || (keyState.IsKeyUp(Keys.H) && keyLast.IsKeyDown(Keys.H)))
            {
                Matrix inverseViewMatrix = Matrix.Invert(mCamera.Transformation);

                Vector2 worldMousePosition = Vector2.Transform(new Vector2(Mouse.GetState().X, Mouse.GetState().Y), inverseViewMatrix);


                foreach (Entity ent in worldEntityList)
                {
                    if ((ent.Position - worldMousePosition).LengthSquared() < 2)
                    {
                        playerCreation.Contact(ent.PhysicsBody);
                    }
                }
            }
            */

            
            //mPhysics.RegisterJoint(hub.physicsBody, playerCreation.sensors, Vector2.Zero);


            //mPhysics.Update(gameTime);
            invention.Update();
            int win;
            //if (goalRectangle.Contains(new Point((int)hub.PhysicsBody.GetWorldCenter().X,(int)hub.PhysicsBody.GetWorldCenter().Y)))
            //{
            //    win = 0;
            //}
            /*if (((GUI)(this.Game.Services.GetService(typeof(IGUI)))).mGameState == GameStateManagement.GamePlayState.BuildMode)
            {
                keyLast = new KeyboardState();
            }
            else*/
            keyLast = keyState;
            base.Update(gameTime);
        }
    }
}
