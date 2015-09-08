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

namespace Scrap
{
    public class ScrapGame : Game
    {
        //Add any useful objects here puplicly. Entity derivitives hold a reference to this.
        //public InputManager inputManager;
        public World world;
        public Camera camera;
        public List<Segment> entityList = new List<Segment>();
        public List<Construct> constructList = new List<Construct>();

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D background, foreground;
        DebugViewXNA debugView;
        Terrain terrain;
        ScrapBadger badger;
        PlayerController playerController;
        Crate crate;
        public ScrapGame()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Content";
            playerController = new PlayerController(this);
            
            terrain = new Terrain(this);
            FarseerPhysics.Settings.PositionIterations = 10;
            FarseerPhysics.Settings.VelocityIterations = 10;
            FarseerPhysics.Settings.MaxPolygonVertices = 1000;
            
            //XmlLoader loadLevel = new XmlLoader(this);
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
            badger = new ScrapBadger(this, new Vector2(23, 4));

            badger.Rotate(20f * 0.0174532925f);


            crate = new Crate(this, new Vector2(26, 2));
            //XmlLoader loader = new XmlLoader();
            //loader.LoadLevel(ref entityList);


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
            foreach (Construct item in constructList)
            {
                item.Update(gameTime);
            }
            
            foreach (Segment item in entityList)
            {
                item.Update(gameTime);
            }

            world.Step(1f / 33f);

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
            spriteBatch.Draw(background, -badger.Position*.5f, new Rectangle(0, 0, 800, 480), Color.White);
            spriteBatch.Draw(foreground, -badger.Position*4f, new Rectangle(0, 0, 800, 480), Color.White);
            terrain.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, camera.Transformation);
            foreach (Segment item in entityList)
            {
                item.Draw(spriteBatch);
            }

            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, null);
            playerController.Draw(spriteBatch);
            spriteBatch.End();

            debugView.RenderDebugData(camera.Projection, camera.Transformation);
            

            base.Draw(gameTime);
        }
    }
}
