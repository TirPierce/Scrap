
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Scrap.GameElements.Entities;

namespace Scrap
{
    public class Camera
    {
        float maxMagnification;
        float minMagnification;
        private Entity targetToFollow;//ToDo: Implement function to follow target and woble when vehicle stops suddenly. https://docs.google.com/document/d/1iNSQIyNpVGHeak6isbP6AHdHD50gs8MNXF1GCf08efg/pub
        private Game game;
        private float magnification;
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
            get { UpdateTransform();  return transformation; }
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
            projection = Matrix.CreateOrthographicOffCenter(0f, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height, 0f, 0f, 1f);
        }
        public void Zoom(float MagnificationDelta)
        {
            if (magnification + MagnificationDelta < maxMagnification && magnification + MagnificationDelta > minMagnification)
                magnification += MagnificationDelta;

        }
        public void Rotate(float RotateDelta)
        {
            rotation += RotateDelta;

        }
        private void UpdateTransform()
        {
            transformation = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                             Matrix.CreateRotationZ((float)rotation) *
                             Matrix.CreateScale(new Vector3(magnification, magnification, 1)) *
                             Matrix.CreateTranslation(new Vector3(game.GraphicsDevice.Viewport.Width * 0.5f, game.GraphicsDevice.Viewport.Height * 0.5f, 0));
        }
    }
}
