using FarseerPhysics.DebugView;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Scrap.GameElements.Entities;
using Scrap.GameElements;
using System.Collections.Generic;
using GameElements.GameWorld;

namespace Scrap
{
    public class ScrapGame : Game
    {
        //Add any useful objects here puplicly. Entity derivitives hold a reference to this. Downcast the Game reference to ScrapGame.
        
        public List<Entity> entityList = new List<Entity>();
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public Camera camera;
        private Texture2D background;
        InputManager inputManager;
        public World world;
        DebugViewXNA debugView;

        Terrain terrain;
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

            Crate crate1 =new Crate(this, new Vector2(22, 8));
            Crate crate2 =new Crate(this, new Vector2(22, 15));
            Wheel wheel1=new Wheel(this, new Vector2(21.55f, 10));
            Wheel wheel2 = new Wheel(this, new Vector2(22.55f, 10));

            camera.Follow(wheel1, camera.Magnification);
            //Body groundBody = BodyFactory.CreateEdge(world, new Vector2(-200, 20), new Vector2(200,20));
            terrain.LoadContent();
            terrain.CreateGround(world);
        }


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
            
            foreach (Entity item in entityList)
            {
                item.Update(gameTime);
            }

            world.Step(1f / 33f);

            camera.Update(gameTime);

            base.Update(gameTime);
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
