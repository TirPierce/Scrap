using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Scrap.UserInterface
{
    class Camera:ICameraService
    {
        float MaxMagnification;
        float MinMagnification;
        Vector2 MinPos;
        Vector2 MaxPos;
        public Vector2 hubWorld;
        float skyRotation = 0f;
        bool day = true;
        int dayLength = 5000;
        int timePassed = 0;

        private Matrix transformation;

        public Matrix Transformation
        {
            get { return transformation; }
            
        }
        private double rotation;

        public double Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
        private float magnification;//difference betweem world coor and screen

        public float Magnification
        {
            get { return magnification; }
            set { magnification = value; }
        }

        private Vector2 cameraPosition;

        public Vector2 Position
        {
            get { return cameraPosition; }
            set { cameraPosition = value; }
        }
        private bool follow = false;
        private Vector2 targetToFollow;//different to position incase we want to snap back. 

        private Game game;
        private Texture2D mFarBackground;
        private Texture2D mNearBackground;
        private MouseState mouseLast;
        /*This class is used to translate world coordinates into screen coordinates
         * 
         */
        public Camera(Game _game)
        {
            game = _game;

            game.Services.AddService(typeof(ICameraService), this);//registers this as the camera
            rotation = 0;
            transformation = Matrix.Identity;
            magnification = 40f;
            cameraPosition.X = 1050;
            cameraPosition.Y = 479;
            MaxMagnification= 80f;
            MinMagnification=1f;
            MinPos = new Vector2(0, 100);
            MaxPos = new Vector2(9000, 800);

        }
        public void FocusCamera(Vector2 Target)
        {
            follow = false;
            
        }
        public void Follow(ref Vector2 Target)
        {
            follow = true;
            targetToFollow = Target;
        }
        public void DrawBackground(SpriteBatch batch)
        {
          
            //batch.Draw(GameWorld.TerrainAdv.cheat, new Vector2(Position.X*.7f - 20, Position.Y*.7f - 20), null, Color.White, 0, Vector2.Zero, .05f, SpriteEffects.None, 0);
            //
            //batch.Draw(mFarBackground, new Vector2(Position.X * .7f, Position.Y * .7f), null, Color.White, skyRotation, new Vector2(mFarBackground.Width / 2f, mFarBackground.Height / 2f), .3f, SpriteEffects.None, 0);
            batch.Draw(mFarBackground, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipVertically, 0);
        }

        public void Zoom(int newMagnification)
        {
            magnification = newMagnification;
            
        }

        public void Update()
        {

            GamePadState currentState = GamePad.GetState(0);
            MouseState mouseState = Mouse.GetState();
            if (currentState.IsConnected)
            {
                if (currentState.DPad.Up == ButtonState.Pressed)
                {
                    if (magnification > MinMagnification)
                    {
                        magnification -= 1f;
                    }

                }
                if (currentState.DPad.Down == ButtonState.Pressed)
                {
                    if (magnification < MaxMagnification)
                    {
                        magnification += 1f;
                    }

                }
                if (currentState.DPad.Left == ButtonState.Pressed)
                {
                    rotation -= .005f;
                }
                if (currentState.DPad.Right == ButtonState.Pressed)
                {
                    rotation += .005f;
                }
            }
            else
            {


                magnification = MathHelper.Clamp(magnification+=(mouseState.ScrollWheelValue-mouseLast.ScrollWheelValue)*.01f, MinMagnification, MaxMagnification);
                
            }
            /*
            timePassed += 20;//time in one update

            if (timePassed >= dayLength)
            {
                timePassed = 0;
                day = !day;
            }
            if (day && skyRotation != 0f)
            {
                skyRotation += .03f;
                if (skyRotation < .06f || skyRotation > (2f * MathHelper.Pi) - .06f)
                {
                    skyRotation = 0f;
                }
            }
            if (!day && skyRotation != MathHelper.Pi)
            {
                skyRotation += .03f;
                if (skyRotation <  MathHelper.Pi+.06f && skyRotation > (MathHelper.Pi) - .06f)
                {
                    skyRotation = MathHelper.Pi;
                }
            }
            */
            setTransformation();
            mouseLast = mouseState;

        }
        private void setTransformation()
        {
            transformation = Matrix.CreateTranslation(new Vector3(-cameraPosition.X, -cameraPosition.Y, 0)) *
                                         Matrix.CreateRotationZ((float)Rotation) *
                                         Matrix.CreateScale(new Vector3(magnification, magnification, 1)) *
                                         Matrix.CreateTranslation(new Vector3(game.GraphicsDevice.Viewport.Width * 0.5f, game.GraphicsDevice.Viewport.Height * 0.5f, 0));
            
        }
        public void SetBackground(Texture2D near, Texture2D far)
        {
            mFarBackground = far;
            mNearBackground = near;
        }
    }
}
