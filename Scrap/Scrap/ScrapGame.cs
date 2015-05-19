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
        public InputManager inputManager;
        public World world;
        public Camera camera;
        public List<Entity> entityList = new List<Entity>();
        public List<Construct> constructList = new List<Construct>();

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D background;
        DebugViewXNA debugView;
        Terrain terrain;
        ScrapBadger badger;
        public ScrapGame()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            inputManager = new InputManager();
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
            world = new World(new Vector2(0, 1f));

            debugView = new DebugViewXNA(world);
            camera = new Camera(this);
            camera.Position = new Vector2(22,20);
            debugView.LoadContent(GraphicsDevice, Content);


            badger = new ScrapBadger(this);

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
            inputManager.Update();

            if(inputManager.WasKeyReleased(Keys.Q))
            {
                camera.Rotate(- .005f);
            }
            if( inputManager.WasKeyReleased(Keys.E))
            {
                camera.Rotate(+.005f);
            }
            if (inputManager.WasKeyReleased(Keys.D))
            {
                camera.Position += new Vector2(1f,0f);
            }
            if (inputManager.WasKeyReleased(Keys.A))
            {
                camera.Position += new Vector2(-1f, 0f);
            }
            if (inputManager.WasKeyReleased(Keys.W))
            {
                camera.Position += new Vector2(0f, -1f);
            }
            if (inputManager.WasKeyReleased(Keys.S))
            {
                camera.Position += new Vector2(0f, 1f);
            }
            if (inputManager.WasKeyReleased(Keys.Space))
            {
                camera.Position = new Vector2(0f, 0f);
            }
            camera.Zoom(inputManager.ScroleWheelDelta()*.01f);//ToDo: Camera Controls need to be changed
            foreach (Construct item in constructList)
            {
                item.Update(gameTime);
            }
            
            foreach (Entity item in entityList)
            {
                item.Update(gameTime);
            }

            world.Step(1f / 33f);

            camera.Update(gameTime);

            base.Update(gameTime);


            if (inputManager.WasKeyReleased(Keys.P))
            {
                XmlLoader loader = new XmlLoader();
                loader.SaveLevel(entityList);
            }

            if (inputManager.WasKeyReleased(Keys.L))
            {
                XmlLoader loader = new XmlLoader();
                loader.LoadLevel(ref entityList, this);
            }
        }

        protected override void Draw(GameTime gameTime)
        {


            terrain.RenderTerrain();
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null,null);
            spriteBatch.Draw(background, new Rectangle(0, 0, 800, 480), Color.White);
            terrain.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, camera.Transformation);
            foreach (Entity item in entityList)
            {
                item.Draw(spriteBatch);
            }
            spriteBatch.End();

            debugView.RenderDebugData(camera.Projection, camera.Transformation);
            

            base.Draw(gameTime);
        }
    }
}
