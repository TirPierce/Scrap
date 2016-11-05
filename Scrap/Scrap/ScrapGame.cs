using FarseerPhysics.DebugView;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Scrap.GameElements.Entities;
using Scrap.GameElements;
using System.Collections.Generic;
using Scrap.GameElements.GameWorld;
using FarseerPhysics.Factories;
using Scrap.UserInterface;

namespace Scrap
{
    public class ScrapGame : Game
    {
        //Add any useful objects here puplicly. Entity derivitives hold a reference to this.
        //public InputManager inputManager;
        public World world;
        public Camera camera;
        public GUI gui;
        public List<Segment> entityList = new List<Segment>();
        public List<Construct> constructList = new List<Construct>();
        public bool buildMode = false;
        

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D background, foreground;
        DebugViewXNA debugView;
        Terrain terrain;
        ScrapBadger badger;
        ConstructBuilder hudConstruct;
        public HUDButtonMapping hudButtonMapping;
        public PlayerController playerController;
        Crate crate;
        //Player player; not need. crates added here are for the level
        public ScrapGame()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Content";
            playerController = new PlayerController(this);
            gui = new GUI(this);
            terrain = new Terrain(this);
            FarseerPhysics.Settings.PositionIterations = 10;
            FarseerPhysics.Settings.VelocityIterations = 10;
            FarseerPhysics.Settings.MaxPolygonVertices = 1000;

           
            //XmlLoader loadLevel = new XmlLoader(this);
            // Just Checking! - Buggle
        }
        
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("sky");
            foreground = Content.Load<Texture2D>("nearback");
            world = new World(new Vector2(0, 1f));

            debugView = new DebugViewXNA(world);
            camera = new Camera(this);
            camera.Position = new Vector2(22,20);
            debugView.LoadContent(GraphicsDevice, Content);

            playerController.LoadContent();
            badger = new ScrapBadger(this, new Vector2(375, 55));
            crate = new Crate(this, new Vector2(365, 55));
            badger.Rotate(20f * 0.0174532925f);
            
            //Objects in the world. level 1
           // var rocket1 = new Rocket(this, new Vector2(380, 55));
           // var rocket2 = new Rocket(this, new Vector2(390, 55));
            //var rocket3 = new Rocket(this, new Vector2(370, 55));
            //var rocket4 = new Rocket(this, new Vector2(370, 55));
            
            //crate2.Rotation = 1f;
            //JointFactory.CreateWeldJoint(world, crate.body, crate2.body, new Vector2(0, 0), new Vector2(0, 1.2f));
            //XmlLoader loader = new XmlLoader();
            //loader.LoadLevel(ref entityList);

            hudConstruct = new ConstructBuilder(badger, this);
            hudButtonMapping = new HUDButtonMapping(badger, this);
            terrain.LoadContent();
            terrain.CreateGround(world);






            //Init();
        }

        //void Init()
        //{
        //    foreach(Entity current in entityList)
        //    {
        //        current.Init(this);
        //    }
        //}

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        
        protected override void Update(GameTime gameTime)
        {
            
            playerController.Update();
            hudButtonMapping.Update();
            hudConstruct.Update();
            if (!buildMode)
            {
                world.Step(1f / 33f);
                foreach (Construct item in constructList)
                {
                    item.Update(gameTime);
                }
            
                foreach (Segment item in entityList)
                {
                    item.Update(gameTime);
                }
            }

            camera.Update(gameTime);

            base.Update(gameTime);


            //if (inputManager.WasKeyReleased(Keys.P))
            //{
            //    XmlLoader loader = new XmlLoader();
            //    loader.SaveLevel(entityList);
            //}

            //if (inputManager.WasKeyReleased(Keys.L))
            //{
            //    XmlLoader loader = new XmlLoader();
            //    loader.LoadLevel(ref entityList, this);
            //}

            //if (inputManager.WasKeyReleased(Keys.O))
            //{
            //    badger.SetPosition(new Vector2(1, 0),false);
            //}
        }

        protected override void Draw(GameTime gameTime)
        {


            terrain.RenderTerrain();
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, null);
            
            //Todo: Idealy pick a Foregrounds Image that has a good Y-Axis height and determine the highest & lowest points of each level in order to make it look good 
            // Background does not scroll, Foreground only scrolls on the X-Axis
            float paralaxBackgroundMultiplier = 1f;
            spriteBatch.Draw(background, -(badger.Position * paralaxBackgroundMultiplier), new Rectangle(0, 0, background.Width /*800*/, background.Height/* 480*/), Color.White);
            float paralaxForegroundMultiplier = 2f;
            spriteBatch.Draw(foreground, new Vector2(0, /*arbitrary offset*/foreground.Height / 2f - (badger.Position.Y * paralaxForegroundMultiplier)), new Rectangle((int)(badger.Position.X * paralaxForegroundMultiplier), 0, foreground.Width, foreground.Height), Color.White);
            terrain.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, camera.Transformation);
            gui.Draw(spriteBatch);
            
            foreach (Construct item in constructList)
            {
                item.Draw(spriteBatch);
            }
            foreach (Segment item in entityList)
            {
                item.Draw(spriteBatch);
            }
            
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, null);
            hudButtonMapping.Draw(spriteBatch);
            hudConstruct.Draw(spriteBatch);
            playerController.Draw(spriteBatch);
            
            spriteBatch.End();

            //debugView.RenderDebugData(camera.Projection, camera.Transformation);
            

            base.Draw(gameTime);
        }
    }
}
