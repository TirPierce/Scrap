using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Scrap
{
    public class ScrapGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Camera camera;
        private Texture2D background;
        InputManager inputManager;
        public ScrapGame()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            camera = new Camera(this); 
            inputManager = new InputManager();
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
            camera.Zoom(inputManager.ScroleWheelDelta()*.01f);//eehhh
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive, null, null, null, null, camera.Transformation);

            spriteBatch.Draw(background, new Rectangle(0, 0, 800, 480), Color.White);

            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
