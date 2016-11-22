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
        public List<Segment> entityList = new List<Segment>();
        public List<Construct> constructList = new List<Construct>();


        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D background, foreground;
        //DebugViewXNA debugView;
        Terrain terrain;
        PlayerConstruct playerConstruct;
        public ConstructBuilder constructBuilder;
        
        public PlayerController playerController;
        Crate crate;



        //Player player; not need. crates added here are for the level
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

            constructBuilder = new ConstructBuilder(this);
            playerConstruct = new PlayerConstruct(this);
            // Just Checking! - Buggle
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }


        protected override void LoadContent()
        {
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 800;

            //graphics.ToggleFullScreen();

            graphics.ApplyChanges();

            spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("sky");
            foreground = Content.Load<Texture2D>("nearback");
            world = new World(new Vector2(0, 1f));

            //debugView = new DebugViewXNA(world);
            camera = new Camera(this);
            camera.Position = new Vector2(22, 20);
            
            //debugView.LoadContent(GraphicsDevice, Content);
            
            playerController.LoadContent();
            
            crate = new Crate(this, new Vector2(365, 55));
            playerConstruct.Rotate(20f * 0.0174532925f);
            playerConstruct.LoadContent(new Vector2(375, 55));
            constructBuilder.LoadContent(playerConstruct);


            terrain.LoadContent();
            terrain.CreateGround(world);

            
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {

            playerController.Update();
            
            constructBuilder.Update();
            if (constructBuilder.BuildMode == BuildMode.Inactive)
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


        }

        protected override void Draw(GameTime gameTime)
        {


            terrain.RenderTerrain();
            GraphicsDevice.Clear(Color.Beige);
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, null);

            //Todo: Idealy pick a Foregrounds Image that has a good Y-Axis height and determine the highest & lowest points of each level in order to make it look good 
            // Background does not scroll, Foreground only scrolls on the X-Axis
            float paralaxBackgroundMultiplier = 1f;
            spriteBatch.Draw(background, -(playerConstruct.Position * paralaxBackgroundMultiplier), new Rectangle(0, 0, background.Width /*800*/, background.Height/* 480*/), Color.White);
            float paralaxForegroundMultiplier = 2f;
            spriteBatch.Draw(foreground, new Vector2(-foreground.Width/2, /*arbitrary offset*/foreground.Height / 2f - (playerConstruct.Position.Y * paralaxForegroundMultiplier)), new Rectangle(0, 0, foreground.Width, foreground.Height), Color.White,0,Vector2.Zero,1.2f,SpriteEffects.None,0);
            terrain.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, camera.Transformation);
            

            foreach (Construct item in constructList)
            {
                item.Draw(spriteBatch);
            }
            foreach (Segment item in entityList)
            {
                item.Draw(spriteBatch);
            }
            constructBuilder.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, null);
            
            
            playerController.Draw(spriteBatch);
            constructBuilder.DrawGUI(spriteBatch);//ToDo: this function is called twice because of the 2 transdorms needed
            spriteBatch.End();

            //debugView.RenderDebugData(camera.Projection, camera.Transformation);


            base.Draw(gameTime);
        }
    }
}
