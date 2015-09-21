
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Scrap.GameElements.Entities;
using Scrap;
using FarseerPhysics.Common;

namespace Scrap
{
    public class Camera
    {
        enum CameraMode { FixedFollow, SmoothFollow, Manual}


        CameraMode cameraMode = CameraMode.Manual;
        float maxMagnification = 10000f;
        float minMagnification=.00001f;
        private Segment entityToFollow;//ToDo: Implement function to follow target and woble when vehicle stops suddenly. https://docs.google.com/document/d/1iNSQIyNpVGHeak6isbP6AHdHD50gs8MNXF1GCf08efg/pub
        private Game game;
        private float magnification;
        

        public float Magnification
        {
            get { return magnification; }
            set { magnification =MathHelper.Clamp(value, minMagnification, maxMagnification); }
        }
        private Vector2 position;
        Matrix projection;
        private Matrix transformation;
        private double rotation;


        public Matrix Projection
        {
            get { return projection; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        
        public Matrix Transformation
        {
            get {return transformation; }
        }
        
        public double Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
        
        public Camera(Game game)
        {
            this.game = game;

            rotation = 0;
            transformation = Matrix.Identity;
            magnification = 40f;
            position = new Vector2(0, 0);
            maxMagnification= 10000f;
            minMagnification=.00001f;
            projection = Matrix.CreateOrthographicOffCenter(0f, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height, 0f, -10f, 50f);
        }
        public void Zoom(float MagnificationDelta)
        {
            if (Magnification + MagnificationDelta < maxMagnification && Magnification + MagnificationDelta > minMagnification)
                Magnification += MagnificationDelta;

        }
        public void Rotate(float RotateDelta)
        {
            rotation += RotateDelta;

        }
        private void UpdateTransform()
        {
            transformation = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                             Matrix.CreateRotationZ((float)rotation) *
                             Matrix.CreateScale(new Vector3(Magnification, Magnification, 1)) *
                             Matrix.CreateTranslation(new Vector3(game.GraphicsDevice.Viewport.Width * 0.5f, game.GraphicsDevice.Viewport.Height * 0.5f, 0));
        }
        public void Update(GameTime gametime)
        {
            if (cameraMode == CameraMode.FixedFollow)
            {
                Position = entityToFollow.Position;
                
            }
            UpdateTransform();
        }
        public Vector2 MousePick(Point mousePos)
        {
            Vector2 mouseVector = mousePos.ToVector2();
            Matrix reverseCamera = Matrix.Invert(transformation);
            Vector2 worldPos;
            Vector2.Transform(ref mouseVector, ref reverseCamera, out worldPos);
            return worldPos;
        }
        public Vector2 ProjectPoint(Vector2 pos)
        {
            Vector2 worldPos;
            Vector2.Transform(ref pos, ref transformation, out worldPos);
            return worldPos;
        }
        public void Shake(float strength, float duration)
        {

        }
        public void Follow(Segment entity, float magnification)
        {
            cameraMode = CameraMode.FixedFollow;
            entityToFollow = entity;
            Magnification = magnification;
        }
    }
}
